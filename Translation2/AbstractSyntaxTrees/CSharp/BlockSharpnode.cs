using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
using System.Linq;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class BlockSharpnode : Sharpnode
    {
        public BlockSyntax BlockSyntax;
        public List<StatementSharpnode> Statements = new List<StatementSharpnode>();

        public BlockSharpnode(BlockSyntax originalNode) : base(originalNode)
        {
            BlockSyntax = originalNode;
            Statements = originalNode.Statements.Select(stmt => RoslynToSharpnode.MapStatement(stmt)).ToList();
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            List<Silvernode> statements = new List<Silvernode>();
            List<Silvernode> verificationConditions = new List<Silvernode>();
            List<Error> diagnostics = new List<Error>();
            foreach(var statement in Statements)
            {
                var statementResult = statement.Translate(context);
                if (statementResult.SilverSourceTree != null)
                {
                    if (statementResult.SilverSourceTree.IsVerificationCondition())
                    {
                        verificationConditions.Add(statementResult.SilverSourceTree);
                        // TODO trigger warning if father is not method
                    }
                    else
                    {
                        statements.Add(statementResult.SilverSourceTree);
                    }
                }
                diagnostics.AddRange(statementResult.ReportedDiagnostics);
            }
            BlockSilvernode block = new BlockSilvernode(BlockSyntax, statements);
            TranslationResult r = new TranslationResult();
            r.SilverSourceTree = block;
            r.VerificationConditions = verificationConditions;
            r.ReportedDiagnostics = diagnostics;
            return r;
        }
    }
}