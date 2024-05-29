using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil
{
    public abstract class CsvFileCommon
    {
        private const int DelimiterIndex = 0;

        private const int QuoteIndex = 1;

        protected char[] SpecialChars = new char[] { ',', '\"', '\r', '\n' };

        public char Delimiter
        {
            get
            {
                return this.SpecialChars[0];
            }
            set
            {
                this.SpecialChars[0] = value;
            }
        }

        public char Quote
        {
            get
            {
                return this.SpecialChars[1];
            }
            set
            {
                this.SpecialChars[1] = value;
            }
        }

        protected CsvFileCommon()
        {
        }
    }
}
