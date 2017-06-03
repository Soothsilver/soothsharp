using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Soothsharp.Translation.Trees.CSharp.Statements;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp
{
    internal class BlockSharpnode : StatementSharpnode
    {
        private BlockSyntax BlockSyntax;
        private List<StatementSharpnode> Statements;

        public BlockSharpnode(BlockSyntax originalNode) : base(originalNode)
        {
            this.BlockSyntax = originalNode;
            this.Statements = originalNode.Statements.Select(RoslynToSharpnode.MapStatement).ToList();
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var statements = new List<StatementSilvernode>();
            var verificationConditions = new List<ContractSilvernode>();
            var diagnostics = new List<Error>();
            bool inFunctionOrPredicateBlockReturnStatementAlreadyOccured = false;
            foreach(var statement in this.Statements)
            {
                var statementResult = statement.Translate(context);
                if (statementResult.PrependTheseSilvernodes.Any())
                {
                    statements.AddRange(statementResult.PrependTheseSilvernodes);
                }
                if (statementResult.Silvernode != null)
                {
                    if (statementResult.Silvernode.IsVerificationCondition())
                    {
                        verificationConditions.Add(statementResult.Silvernode as ContractSilvernode);
                        if (statementResult.Silvernode is RequiresSilvernode ||
                            statementResult.Silvernode is EnsuresSilvernode)
                        {
                            if (this.BlockSyntax.Parent is MethodDeclarationSyntax ||
                                this.BlockSyntax.Parent is ConstructorDeclarationSyntax)
                            {

                            }
                            else
                            {
                                diagnostics.Add(new Translation.Error(
                                    Diagnostics.SSIL129_MethodContractsAreOnlyForMethods,
                                    statementResult.Silvernode.OriginalNode));
                            }
                        }
                        if (statementResult.Silvernode is InvariantSilvernode)
                        {
                            if (this.BlockSyntax.Parent is ForStatementSyntax ||
                                this.BlockSyntax.Parent is WhileStatementSyntax ||
                                this.BlockSyntax.Parent is DoStatementSyntax)
                            {

                            }
                            else
                            {

                                diagnostics.Add(new Translation.Error(
                                    Diagnostics.SSIL130_InvariantsAreOnlyForLoops,
                                    statementResult.Silvernode.OriginalNode));
                            }
                        }
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
            BlockSilvernode block = new BlockSilvernode(this.BlockSyntax, statements);
            verificationConditions.Sort();
            return new TranslationResult
            {
                Silvernode = block,
                Errors = diagnostics,
                Contracts = verificationConditions
            };
        }
    }
}