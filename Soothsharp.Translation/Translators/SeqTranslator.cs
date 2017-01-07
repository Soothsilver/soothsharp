using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Contracts;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    internal static class SeqTranslator
    {
        private const string CONTRACTS_NAMESPACE = nameof(Soothsharp) + "." + nameof(Contracts) + ".";
        private const string SEQ_CLASS = SeqTranslator.CONTRACTS_NAMESPACE + nameof(Seq<float>) + ".";
        public const string SeqClassWithoutEndDot = SeqTranslator.CONTRACTS_NAMESPACE + nameof(Seq<float>);
        public const string SeqLength = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.Length);
        public const string SeqAccess = SeqTranslator.SEQ_CLASS + "this";
        public const string Contains = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.Contains);
        public const string TakeDrop = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.TakeDrop);
        public const string Take = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.Take);
        public const string Drop = SeqTranslator.SEQ_CLASS + nameof(Seq<float>.Drop);
        public const string OperatorPlus = SeqTranslator.SEQ_CLASS + "operator +";

        public static TranslationResult Constructor(List<ExpressionSharpnode> arguments, TranslationContext context, ITypeSymbol typeArgument, SyntaxNode originalNode)
        {
          
            var silvernodes = new List<Silvernode>();
            var errors = new List<Error>();
            // TODO add purifiable thingies before here
            foreach (var arg in arguments)
            {
                var res = arg.Translate(context.ChangePurityContext(PurityContext.Purifiable));
                silvernodes.Add(res.Silvernode);
                errors.AddRange(res.Errors);
            }
            Silvernode result;
            if (arguments.Count == 0)
            {
                Error err;
                SilverType silverType = TypeTranslator.TranslateType(typeArgument, null, out err);
                if (err != null) errors.Add(err);
                result = new SimpleSequenceSilvernode(originalNode,
                    "Seq[", new TypeSilvernode(null, silverType), "]()");

            }
            else
            {
                result = new SimpleSequenceSilvernode(originalNode,
                   "Seq(",
                   string.Join(", ", silvernodes),
                   ")");
            }
            return TranslationResult.FromSilvernode(result, errors);
        }
    }
}
