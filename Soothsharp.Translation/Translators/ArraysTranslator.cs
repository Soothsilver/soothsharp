using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Translators
{
    public class ArraysTranslator
    {
        public const string IntegerArrayRead = "arrayRead";
        public const string IntegerArrayWrite = "arrayWrite";
        public const string IntegerArrayAccess = "arrayAccessPermission";
        public const string IntegerArrayContents = "arrayContents";
        public readonly string IntegerArrayGlobalSilvertext =
            $@"
field {ArraysTranslator.IntegerArrayContents} : Seq[Int]
define {ArraysTranslator.IntegerArrayAccess}(array) acc(array.{ArraysTranslator.IntegerArrayContents})
define {ArraysTranslator.IntegerArrayWrite}(array, index, value) {{ assert index >= 0; assert index < |array.{ArraysTranslator.IntegerArrayContents}|; array.{ArraysTranslator.IntegerArrayContents} := array.{ArraysTranslator.IntegerArrayContents}[..index] ++ Seq(value) ++ array.{ArraysTranslator.IntegerArrayContents}[(index+1)..]; }}
function {ArraysTranslator.IntegerArrayRead}(array : Ref, index : Int) : Int
    requires acc(array.{ArraysTranslator.IntegerArrayContents}, wildcard)
    requires |array.{ArraysTranslator.IntegerArrayContents}| > index
{{
    array.{ArraysTranslator.IntegerArrayContents}[index]
}}";

        public bool ArraysWereUsed { get; set; } = true;
        public const string ArrayLength = nameof(System) + "." + nameof(System.Array) + "." + nameof(System.Array.Length);

        public Silvernode GenerateGlobalSilvernode()
        {
            return new TextSilvernode(IntegerArrayGlobalSilvertext, null);
        }

        public SimpleSequenceStatementSilvernode ArrayWrite(SyntaxNode originalNode, Silvernode container, Silvernode index, Silvernode value)
        {
            return new SimpleSequenceStatementSilvernode(originalNode,
                IntegerArrayWrite + "(",
                container,
                ", ",
                index,
                ", ",
                value,
                ")");
        }

        public Silvernode ArrayRead(SyntaxNode originalNode, Silvernode containerSilvernode, Silvernode indexSilvernode)
        {
            return new SimpleSequenceSilvernode(originalNode,
                IntegerArrayRead + "(",
                containerSilvernode,
                ", ",
                indexSilvernode,
                ")");
        }
    }
}
