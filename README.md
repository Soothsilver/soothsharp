#Sharpsilver C#-to-Silver Translator
**Sharpsilver** is a transcompiler that transforms C# code into code files of the Silver Intermediate Language (`.sil`), a verification language based on permission logics.

This project is currently in active development.

### Examples

For example, the following C# code:
```csharp
//SUCCEEDS
using Sharpsilver.Contracts;
using AlternativeNameForContract = Sharpsilver.Contracts.Contract;

namespace Sharpsilver.TranslationTests.Files
{
    static class Invariants
    {
        public static int sum(int n)
        {
            Sharpsilver.Contracts.Contract.Requires(n > 0);
            Contract.Ensures(AlternativeNameForContract.IntegerResult == n * (n + 1) / 2);
            int i = 0;
            int s = 0;
            while (i < n)
            {
                AlternativeNameForContract.Invariant(s == i * (i + 1) / 2);
                AlternativeNameForContract.Invariant(i <= n);
                i = i + 1;
                s = s + i; 
            }
            return s;
        }
    }
}
```
might be translated into something like this:
```
method Invariants_sum (n : Int) returns (res : Int)
        requires (n > 0)
        ensures (res == n * (n + 1) \ 2)
{
        var i : Int
        i := 0
        var s : Int
        s := 0
        while (i < n)
                invariant (s == i * (i + 1) \ 2)
                invariant (i <= n)
        {
                i := i + 1
                s := s + i
        }
        res := s
}
```
This would then be passed to a back-end verifier, which could output something like this:
```
Silicon 1.1-SNAPSHOT (b5ccd60f3d2a)
(c) Copyright ETH Zurich 2012 - 2016

Silicon finished in 3,057 seconds.
No errors found.
```
and thus, the C# code would be successfully verified as correct.