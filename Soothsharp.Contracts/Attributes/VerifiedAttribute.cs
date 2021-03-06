﻿using System;

namespace Soothsharp.Contracts
{
    /// <summary>
    /// The annotated class or method makes use of formal verification and should be translated into Viper.
    /// If a class is annotated, then all member methods except those marked [Unverified] are marked for verification as well.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerifiedAttribute : Attribute

    {
    }
}
