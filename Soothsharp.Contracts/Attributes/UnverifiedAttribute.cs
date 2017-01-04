using System;

namespace Soothsharp.Contracts
{
    /// <summary>
    /// The annotated class or method doesn't use formal verification and should not be translated to Viper.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UnverifiedAttribute : Attribute
    {
    }
}
