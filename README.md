# Soothsharp: A C# to Viper translator
**Soothsharp** is a transcompiler that transforms C# code into code files of the Viper Intermediate Language (`.vpr`), a verification language based on permission logics.

This project is currently in active development.

The corresponding thesis text is being created at: https://docs.google.com/document/d/1o0VfYGBnORk7PDMHPIxl39hZ8iz5wEKCxPs34RVM4YY/edit?usp=sharing


## Installation
You will need Visual Studio 2015 or newer (tested on 2015 only).

1. Clone this repository locally.
2. Put the `Viper` directory into your `%PATH%` environment variable. The `INSTALL.txt` file in that directory gives more details.
3. Rebuild the solution.
    * If the fails, right-click the solution in the Solution Explorer and click "Restore NuGet packages", then try to rebuild the solution again.
4. Run the "csverify GUI" project.
5. You may find examples in the "Examples" project.

## Example

This class file defines a tuple of two integers that can be verifiably swapped.

```
using System;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples.Algorithms
{
    public class Tuple
    {
        public int First;
        public int Second;

        public Tuple(int first, int second)
        {
            Ensures(Acc(First) && Acc(Second));

            First = first;
            Second = second;
        }

        public void Swap()
        {
            Requires(Acc(First) && Acc(Second));
            Ensures(Acc(First) && Acc(Second));
            Ensures(First == Old(Second) && Second == Old(First));

            int temp = First;
            First = Second;
            Second = temp;
        }
    }
}
```