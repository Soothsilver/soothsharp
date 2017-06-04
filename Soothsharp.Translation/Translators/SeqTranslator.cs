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
    /// <summary>
    /// Groups functionality related to translating the <see cref="Seq{T}"/> class to Viper sequences. 
    /// </summary>
    internal static class SeqTranslator
    {
        private const string CONTRACTS_NAMESPACE = nameof(Soothsharp) + "." + nameof(Contracts) + ".";
        private const string SEQ_CLASS = CONTRACTS_NAMESPACE + nameof(Seq<float>) + ".";
        public const string SeqClassWithoutEndDot = CONTRACTS_NAMESPACE + nameof(Seq<float>);
        public const string SeqLength = SEQ_CLASS + nameof(Seq<float>.Length);
        public const string SeqAccess = SEQ_CLASS + "this";
        public const string Contains = SEQ_CLASS + nameof(Seq<float>.Contains);
        public const string TakeDrop = SEQ_CLASS + nameof(Seq<float>.TakeDrop);
        public const string Take = SEQ_CLASS + nameof(Seq<float>.Take);
        public const string Drop = SEQ_CLASS + nameof(Seq<float>.Drop);
        public const string OperatorPlus = SEQ_CLASS + "operator +";

        public static TranslationResult Constructor(List<ExpressionSharpnode> arguments, TranslationContext context, ITypeSymbol typeArgument, SyntaxNode originalNode)
        {
          
            var silvernodes = new List<Silvernode>();
            var errors = new List<Error>();
            List<StatementSilvernode> prepend = new List<Trees.Silver.StatementSilvernode>();
            // Translate initial members
            foreach (var arg in arguments)
            {
                var res = arg.Translate(context.ChangePurityContext(PurityContext.Purifiable));
                silvernodes.Add(res.Silvernode);
                errors.AddRange(res.Errors);
                prepend.AddRange(res.PrependTheseSilvernodes);
            }
            Silvernode result;
            if (arguments.Count == 0)
            {
                // No arguments = use the Seq[Int] construction
                Error err;
                SilverType silverType = TypeTranslator.TranslateType(typeArgument, null, out err);
                if (err != null) errors.Add(err);
                result = new SimpleSequenceSilvernode(originalNode,
                    "Seq[", new TypeSilvernode(null, silverType), "]()");

            }
            else
            {
                // Some arguments - use the Seq construction with type inference
                // ReSharper disable once UseObjectOrCollectionInitializer
                List<Silvernode> args = new List<Silvernode>();
                args.Add("Seq(");
                for (int i = 0; i < silvernodes.Count; i++)
                {
                    args.Add(silvernodes[i]);
                    if (i != silvernodes. Count - 1)
                    {
                        args.Add(", ");
                    }
                }
                args.Add(")");
                result = new SimpleSequenceSilvernode(originalNode, args.ToArray());
            }
            return TranslationResult.FromSilvernode(result, errors).AndPrepend(prepend);
        }
    }
}
