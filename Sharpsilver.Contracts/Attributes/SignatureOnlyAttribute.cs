using System;

namespace Soothsharp.Contracts
{
    /// <summary>
    /// The annotated method makes use of formal verification and should be translated into Silver. 
    /// However, its body should only be scanned for verification conditions that should be asserted to be true; 
    /// other statements will not be translated and will be ignored by Sharpsilver. 
    /// If the annotated node is a class, then this attribute applies to all of its methods instead.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SignatureOnly : Attribute

    {
    }
}
