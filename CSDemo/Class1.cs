using System;
using System.Runtime.InteropServices;
public static class LibFoo {

  public unsafe struct handle_t {};

  [DllImport("libfoo.dylib")]
  public unsafe static extern void
  doit(int valin, int *res);

  [DllImport("libfoo.dylib")]
  public unsafe static extern void handle_alloc(handle_t **h);
}

namespace CSDemo {
public unsafe class Class1 {
  public unsafe static void Main() {
    int res = 0;
    LibFoo.doit(2, &res);

    Console.WriteLine("result: {0}", res);

    LibFoo.handle_t* h;
    LibFoo.handle_alloc(&h);

    Console.WriteLine("C# handle_t* is: {0:X}", (UInt64)h);
  }
}
}
