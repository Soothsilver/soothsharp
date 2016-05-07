using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;

namespace Sharpsilver.LanguageServices.Silver
{
    public class Resolver : Sharpsilver.LanguageServices.Silver.IASTResolver
    {
        #region IASTResolver Members


        public IList<Sharpsilver.LanguageServices.Silver.Declaration> FindCompletions(object result, int line, int col)
        {
            // Used for intellisense.
            List<Sharpsilver.LanguageServices.Silver.Declaration> declarations = new List<Sharpsilver.LanguageServices.Silver.Declaration>();

            // Add keywords defined by grammar
            foreach (KeyTerm key in Configuration.Grammar.KeyTerms.Values)
            {
                if (key.Flags.HasFlag(TermFlags.IsKeyword))
                {
                    declarations.Add(new Declaration("", key.Name, 206, key.Name));
                }
            }

            declarations.Sort();
            return declarations;
        }

        public IList<Sharpsilver.LanguageServices.Silver.Declaration> FindMembers(object result, int line, int col)
        {
            List<Sharpsilver.LanguageServices.Silver.Declaration> members = new List<Sharpsilver.LanguageServices.Silver.Declaration>();

            return members;
        }

        public string FindQuickInfo(object result, int line, int col)
        {
            return "unknown";
        }

        public IList<Sharpsilver.LanguageServices.Silver.Method> FindMethods(object result, int line, int col, string name)
        {
            return new List<Sharpsilver.LanguageServices.Silver.Method>();
        }

        #endregion
    }
}
