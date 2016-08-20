using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.Trees
{
    public class Spansize
    {
        public int Lines { get; set; }
        /// <summary>
        /// Only makes sense if it is a oneline.
        /// </summary>
        public int Columns { get; set; }

        public Spansize(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
        }
    }
}
