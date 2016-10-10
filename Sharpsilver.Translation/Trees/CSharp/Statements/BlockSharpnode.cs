using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;
using System.Linq;
using Sharpsilver.Translation.Trees.CSharp.Statements;

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
            var statements = new List<StatementSilvernode>();
            var verificationConditions = new List<VerificationConditionSilvernode>();
            var diagnostics = new List<Error>();
            bool inFunctionOrPredicateBlockReturnStatementAlreadyOccured = false;
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
                        if (context.IsFunctionOrPredicateBlock)
                        {

                            if (statement.GetType() == typeof(ReturnStatementSharpnode))
                            {
                                if (inFunctionOrPredicateBlockReturnStatementAlreadyOccured)
                                {
                                    diagnostics.Add(new Error(Diagnostics.SSIL122_FunctionsAndPredicatesCannotHaveMoreThanOneReturnStatement,
                                        statement.OriginalNode));
                                }
                                else
                                {
                                    inFunctionOrPredicateBlockReturnStatementAlreadyOccured = true;
                                }
                            }
                            else
                            {
                                diagnostics.Add(new Error(Diagnostics.SSIL121_FunctionsAndPredicatesCannotHaveStatements,
                                    statement.OriginalNode));

                            }
                        }
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
            verificationConditions.Sort();
            return new TranslationResult
            {
                Silvernode = block,
                Errors = diagnostics,
                VerificationConditions = verificationConditions
            };
        }
    }
}