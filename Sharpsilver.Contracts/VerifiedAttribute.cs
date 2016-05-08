﻿using System;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// The annotated class or method makes use of formal verification and should be translated into Silver.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerifiedAttribute : Attribute

    {
    }
}
