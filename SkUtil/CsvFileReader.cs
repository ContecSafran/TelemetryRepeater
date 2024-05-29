using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil
{

    public class CsvFileReader : CsvFileCommon, IDisposable
    {
        private StreamReader Reader;

        private string CurrLine;

        private int CurrLinePos;

        private EmptyLineBehavior EmptyLineBehavior;

        public long Length;

        private long CurPos;

        public long ReadCurPos
        {
            get
            {
                return this.CurPos;
            }
        }

        public CsvFileReader(Stream stream, EmptyLineBehavior emptyLineBehavior = 0)
        {
            this.Reader = new StreamReader(stream);
            this.EmptyLineBehavior = emptyLineBehavior;
            this.Length = this.Reader.BaseStream.Length;
        }

        public CsvFileReader(string path, Encoding OpenEncoding, EmptyLineBehavior emptyLineBehavior = 0)
        {
            this.Reader = new StreamReader(path, OpenEncoding);
            this.EmptyLineBehavior = emptyLineBehavior;
            this.Length = this.Reader.BaseStream.Length;
        }

        public void Dispose()
        {
            this.Reader.Dispose();
        }

        private string ReadQuotedColumn()
        {
            CsvFileReader currLinePos = this;
            currLinePos.CurrLinePos = currLinePos.CurrLinePos + 1;
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                if (this.CurrLinePos == this.CurrLine.Length)
                {
                    this.CurrLine = this.Reader.ReadLine();
                    this.CurPos = this.Reader.BaseStream.Position;
                    this.CurrLinePos = 0;
                    if (this.CurrLine == null)
                    {
                        return builder.ToString();
                    }
                    builder.Append(Environment.NewLine);
                }
                else
                {
                    if (this.CurrLine[this.CurrLinePos] == base.Quote)
                    {
                        int nextPos = this.CurrLinePos + 1;
                        if (nextPos >= this.CurrLine.Length || this.CurrLine[nextPos] != base.Quote)
                        {
                            break;
                        }
                        CsvFileReader csvFileReader = this;
                        csvFileReader.CurrLinePos = csvFileReader.CurrLinePos + 1;
                    }
                    string currLine = this.CurrLine;
                    CsvFileReader csvFileReader1 = this;
                    int num = csvFileReader1.CurrLinePos;
                    int num1 = num;
                    csvFileReader1.CurrLinePos = num + 1;
                    builder.Append(currLine[num1]);
                }
            }
            if (this.CurrLinePos < this.CurrLine.Length)
            {
                CsvFileReader currLinePos1 = this;
                currLinePos1.CurrLinePos = currLinePos1.CurrLinePos + 1;
                builder.Append(this.ReadUnquotedColumn());
            }
            return builder.ToString();
        }

        public bool ReadRow(List<string> columns)
        {
            string column;
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }
        Label0:
            this.CurrLine = this.Reader.ReadLine();
            this.CurrLinePos = 0;
            if (this.CurrLine == null)
            {
                return false;
            }
            this.CurPos = this.Reader.BaseStream.Position;
            if (this.CurrLine.Length == 0)
            {
                switch (this.EmptyLineBehavior)
                {
                    case EmptyLineBehavior.NoColumns:
                        {
                            columns.Clear();
                            return true;
                        }
                    case EmptyLineBehavior.Ignore:
                        {
                            goto Label0;
                        }
                    case EmptyLineBehavior.EndOfFile:
                        {
                            return false;
                        }
                }
            }
            int numColumns = 0;
            while (true)
            {
                column = (this.CurrLinePos >= this.CurrLine.Length || this.CurrLine[this.CurrLinePos] != base.Quote ? this.ReadUnquotedColumn() : this.ReadQuotedColumn());
                if (numColumns >= columns.Count)
                {
                    columns.Add(column);
                }
                else
                {
                    columns[numColumns] = column;
                }
                numColumns++;
                if (this.CurrLine == null || this.CurrLinePos == this.CurrLine.Length)
                {
                    break;
                }
                CsvFileReader currLinePos = this;
                currLinePos.CurrLinePos = currLinePos.CurrLinePos + 1;
            }
            if (numColumns < columns.Count)
            {
                columns.RemoveRange(numColumns, columns.Count - numColumns);
            }
            return true;
        }

        private string ReadUnquotedColumn()
        {
            int startPos = this.CurrLinePos;
            this.CurrLinePos = this.CurrLine.IndexOf(base.Delimiter, this.CurrLinePos);
            if (this.CurrLinePos == -1)
            {
                this.CurrLinePos = this.CurrLine.Length;
            }
            if (this.CurrLinePos <= startPos)
            {
                return string.Empty;
            }
            return this.CurrLine.Substring(startPos, this.CurrLinePos - startPos);
        }
    }
}
