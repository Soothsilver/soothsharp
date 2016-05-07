using System;
using System.Collections.Generic;

namespace Sharpsilver.LanguageServices.Silver
{
    public struct Method
    {
        public string Name;
        public string Description;
        public string Type;
        public IList<Parameter> Parameters;
    }

    public struct Parameter
    {
        public string Name;
        public string Display;
        public string Description;
    }
}