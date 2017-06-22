# Soothsharp: A C# to Viper translator
**Soothsharp** is a transcompiler that transforms C# code into code files of the Viper Intermediate Language (`.vpr`), a verification language based on permission logics.

This project is currently in active development.

The corresponding thesis text is being created at: https://docs.google.com/document/d/1o0VfYGBnORk7PDMHPIxl39hZ8iz5wEKCxPs34RVM4YY/edit?usp=sharing


## Installation
You will need Visual Studio 2015 or newer (tested on 2015 only).

1. Clone this repository locally.
2. Put the `Viper` directory into your `%PATH%` environment variable. The `INSTALL.txt` file in that directory gives more details.
3. Install additional programs required by the Viper tools.
    1. Install Java.
    2. Install Z3 4.4.0 and put the path to `z3.exe` in an environment variable named `Z3_EXE`. Alternatively, you may put `z3.exe` and all associated files into a directory in your `%PATH%` variable.
    3. Install Boogie by building it from source and putting the path to `boogie.exe` in an environment variable named `BOOGIE_EXE`. This is only required for the Carbon backend.
    4. After setting environment variables, you may need to restart Visual Studio and any command-line windows you use for the changes to take effect. 
4. Rebuild the solution.
    * If the fails, right-click the solution in the Solution Explorer and click "Restore NuGet packages", then try to rebuild the solution again.
5. Run the "csverify GUI" project.
6. You may find examples in the "Examples" project.

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