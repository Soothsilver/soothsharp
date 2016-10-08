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

        protected SubroutineSilvernode(SyntaxNode originalNode,
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
        protected override void OptimizePost()
        {
            BlockSilvernode block = Children.FirstOrDefault(s => s is BlockSilvernode) as BlockSilvernode;
            if (block != null)
            {
                int howManyGotos = block.Descendants.Count(sn => sn is GotoSilvernode && ((GotoSilvernode)sn).Label == Constants.SilverMethodEndLabel);
                StatementSilvernode lastStatement = block.Statements.Count >= 1 ? block.Statements[block.Statements.Count - 1] : null;
                StatementSilvernode preLastStatement = block.Statements.Count >= 2 ? block.Statements[block.Statements.Count - 2] : null;
                if (lastStatement != null &&
                    (lastStatement.GetType() == typeof(LabelSilvernode) && ((LabelSilvernode)lastStatement).Label == Constants.SilverMethodEndLabel) &&
                    preLastStatement != null &&
                    (preLastStatement.GetType() == typeof(GotoSilvernode) && ((GotoSilvernode)preLastStatement).Label == Constants.SilverMethodEndLabel) &&
                    howManyGotos == 1)
                {
                    block.Statements.RemoveRange(block.Statements.Count - 2, 2);
                }
                else if (howManyGotos == 0 && (lastStatement.GetType() == typeof(LabelSilvernode) && ((LabelSilvernode)lastStatement).Label == Constants.SilverMethodEndLabel))
                {
                    block.Statements.RemoveAt(block.Statements.Count - 1);
                }

            }
        }
    
    }
    
}
