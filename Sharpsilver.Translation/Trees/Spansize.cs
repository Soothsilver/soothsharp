namespace Soothsharp.Translation.Trees
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
