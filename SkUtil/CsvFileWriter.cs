using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil
{
    public class CsvFileWriter : CsvFileCommon, IDisposable
    {
        private StreamWriter Writer;

        private string OneQuote;

        private string TwoQuotes;

        private string QuotedFormat;

        public CsvFileWriter(Stream stream)
        {
            this.Writer = new StreamWriter(stream);
        }

        public CsvFileWriter(string path, Encoding encode, bool Append)
        {
            this.Writer = new StreamWriter(path, Append, encode);
        }

        public void Dispose()
        {
            this.Writer.Dispose();
        }

        public void WriteRow(List<string> columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }
            if (this.OneQuote == null || this.OneQuote[0] != base.Quote)
            {
                this.OneQuote = string.Format("{0}", base.Quote);
                this.TwoQuotes = string.Format("{0}{0}", base.Quote);
                this.QuotedFormat = string.Format("{0}{{0}}{0}", base.Quote);
            }
            for (int i = 0; i < columns.Count; i++)
            {
                if (i > 0)
                {
                    this.Writer.Write(base.Delimiter);
                }
                if (columns[i].IndexOfAny(this.SpecialChars) != -1)
                {
                    this.Writer.Write(this.QuotedFormat, columns[i].Replace(this.OneQuote, this.TwoQuotes));
                }
                else
                {
                    this.Writer.Write(columns[i]);
                }
            }
            this.Writer.WriteLine();
        }
    }
}
