using System;
using Soothsharp.Translation.Trees.CSharp;

namespace Soothsharp.Translation.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a <see cref="Sharpnode"/>'s Translate method is called for a node that should never be translated.
    /// </summary>
    class TranslationNotSupportedException : Exception
    {
        public TranslationNotSupportedException(string nodeType) : base($"Nodes of type {nodeType} cannot be translated.")
        {
        }
    }
}
