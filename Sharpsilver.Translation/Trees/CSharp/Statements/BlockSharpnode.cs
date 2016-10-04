using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;
using System.Linq;
using Sharpsilver.Translation;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.CSharp
{
    internal class BlockSharpnode : StatementSharpnode
    {
        public BlockSyntax BlockSyntax;
        public List<StatementSharpnode> Statements;

        public BlockSharpnode(BlockSyntax originalNode) : base(originalNode)
        {
            BlockSyntax = originalNode;
            Statements = originalNode.Statements.Select(RoslynToSharpnode.MapStatement).ToList();
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            List<StatementSilvernode> statements = new List<StatementSilvernode>();
            List<VerificationConditionSilvernode> verificationConditions = new List<VerificationConditionSilvernode>();
            List<Error> diagnostics = new List<Error>();
            foreach(var statement in Statements)
            {
                var statementResult = statement.Translate(context);
                if (statementResult.Silvernode != null)
                {
                    if (statementResult.Silvernode.IsVerificationCondition())
                    {
                        verificationConditions.Add(statementResult.Silvernode as VerificationConditionSilvernode);
                        // TODO trigger warning if father is not method
                    }
                    else
                    {
                        StatementSilvernode statementSilvernode;
                        if ((statementResult.Silvernode is StatementSilvernode))
                        {
                            statementSilvernode = (StatementSilvernode)statementResult.Silvernode;
                        }
                        else
                        {
                            statementSilvernode = new ExpressionStatementSilvernode(statementResult.Silvernode, statementResult.Silvernode.OriginalNode);
                        }
                        statements.Add(statementSilvernode);
                    }
                }
                diagnostics.AddRange(statementResult.Errors);
            }
            BlockSilvernode block = new BlockSilvernode(BlockSyntax, statements);
            TranslationResult r = new TranslationResult {Silvernode = block};
            verificationConditions.Sort();
            r.VerificationConditions = verificationConditions;
            r.Errors = diagnostics;
            return r;
        }
    }
}