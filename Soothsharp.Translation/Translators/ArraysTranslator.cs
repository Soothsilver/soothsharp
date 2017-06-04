using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Translators
{
    /// <summary>
    /// Groups functionality related to translating C# arrays.
    /// </summary>
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

        /// <summary>
        /// Generates the Viper text that should be appended to all files that make use of arrays.
        /// </summary>
        /// <returns></returns>
        public Silvernode GenerateGlobalSilvernode()
        {
            return new TextSilvernode(IntegerArrayGlobalSilvertext);
        }

        /// <summary>
        /// Creates Viper text that changes an element of an arrray.
        /// </summary>
        /// <param name="originalNode">The Roslyn node that represents an array write. </param>
        /// <param name="container">The array.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The new value of container[index].</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates Viper text that reads an element of an array.
        /// </summary>
        /// <param name="originalNode">The Roslyn node that represents an array read.</param>
        /// <param name="containerSilvernode">The array.</param>
        /// <param name="indexSilvernode">The index.</param>
        /// <returns></returns>
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
