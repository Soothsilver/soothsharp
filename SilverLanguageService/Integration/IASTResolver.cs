using System;
using System.Collections.Generic;

namespace Sharpsilver.LanguageServices.Silver
{
    interface IASTResolver
    {
        IList<Declaration> FindCompletions(object result, int line, int col);
        IList<Declaration> FindMembers(object result, int line, int col);
        string FindQuickInfo(object result, int line, int col);
        IList<Method> FindMethods(object result, int line, int col, string name);
    }
}