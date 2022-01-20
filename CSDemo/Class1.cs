using System;
using System.Runtime.InteropServices;
public static class LibFoo {

  [DllImport("libfoo.dylib")]
  internal unsafe static extern void
  doit(int valin, int *res);
}

namespace CSDemo {
public class Class1 {
  public unsafe static void Main() {
    int res = 0;
    LibFoo.doit(2, &res);

    Console.WriteLine("result: {0}", res);
  }
}
}
