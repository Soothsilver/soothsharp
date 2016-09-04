using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation
{
    class AllInOptimizer 
    {
        public void Optimize(Silvernode silvernode)
        {
            foreach(var child in silvernode.Children)
            {
                this.Optimize((dynamic)child);
            }
        }
        public void Optimize(HighlevelSequenceSilvernode masterTree)
        {

        }
    }
}
