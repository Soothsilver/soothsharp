using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Translators;
using Soothsharp.Translation.Trees.CSharp.Invocation;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions.Invocation
{
    class InvocationAccArray : InvocationSubroutine
    {
        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            var expressions = ConvertToSilver(arguments, context);
            this.SilverType = SilverType.Bool;

            List<Silvernode> silverArguments = new List<Silver.Silvernode>();
            if (arguments.Count >= 1)
            {
                silverArguments.Add(new SimpleSequenceSilvernode(arguments[0].OriginalNode, expressions[0], ".",
                    ArraysTranslator.IntegerArrayContents));
            }
            if (arguments.Count >= 2)
            {
                silverArguments.Add(expressions[1]);
            }

            this.Silvernode = new CallSilvernode(new Identifier("acc"), silverArguments, SilverType, originalNode);
        }
    }
}
