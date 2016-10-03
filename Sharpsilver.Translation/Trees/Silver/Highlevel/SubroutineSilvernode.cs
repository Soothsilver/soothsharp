using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.Silver
{
    abstract class SubroutineSilvernode : ComplexSilvernode
    {
        protected IdentifierSilvernode Identifier;
        protected BlockSilvernode Block;
        protected TypeSilvernode ReturnType;
        protected string ReturnValueName;
        protected List<VerificationConditionSilvernode> VerificationConditions;
        protected List<ParameterSilvernode> Parameters;

        public SubroutineSilvernode(SyntaxNode originalNode,
            IdentifierSilvernode identifier,
            List<ParameterSilvernode> parameters,
            string returnValueName,
            TypeSilvernode returnType,
            List<VerificationConditionSilvernode> verificationConditions,
            BlockSilvernode block) : base(originalNode)
        {
            this.Identifier = identifier;
            this.Parameters = parameters;
            this.ReturnValueName = returnValueName;
            this.ReturnType = returnType;
            this.VerificationConditions = verificationConditions;
            this.Block = block;
        }

        protected void AddVerificationConditions(List<Silvernode> children)
        {
            if (VerificationConditions.Any())
            {
                children.Add("\n");
                children.AddRange(
                    VerificationConditions.WithSeparator<Silvernode>("\n").SelectMany(
                    condition =>
                    {
                        if (condition is TextSilvernode) return new[] { condition };
                        return new[] { "\t", condition };
                    }
                    ));
                children.Add("\n");
            }
            else
            {
                children.Add(" ");
            }
        }
        protected void AddBlock(List<Silvernode> children)
        {
            if (Block != null)
            {
                children.Add(Block);
            }
        }
    }
}
