using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Sil.Translation
{
    public class Error
    {
        public string Caption;
        public string ErrorCode;
        public string CsharpFilename;
        public int CsharpColumn;
        public int CsharpLine;
    }
}
