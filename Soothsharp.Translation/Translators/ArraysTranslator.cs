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
field {IntegerArrayContents} : Seq[Int]
define {IntegerArrayAccess}(array) acc(array.{IntegerArrayContents})
define {IntegerArrayWrite}(array, index, value) {{ assert index >= 0; assert index < |array.{IntegerArrayContents}|; array.{IntegerArrayContents} := array.{IntegerArrayContents}[..index] ++ Seq(value) ++ array.{IntegerArrayContents}[(index+1)..]; }}
function {IntegerArrayRead}(array : Ref, index : Int) : Int
    requires acc(array.{IntegerArrayContents}, wildcard)
    requires |array.{IntegerArrayContents}| > index
{{
    array.{IntegerArrayContents}[index]
}}";

        public bool ArraysWereUsed { get; set; } = true;
        public const string ArrayLength = nameof(System) + "." + nameof(Array) + "." + nameof(Array.Length);

        public Silvernode GenerateGlobalSilvernode()
        {
            return new TextSilvernode(IntegerArrayGlobalSilvertext);
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
