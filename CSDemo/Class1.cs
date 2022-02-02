using System;
using System.Runtime.InteropServices;
using System.Text;

public unsafe partial class FooHandle : SafeHandle {

  // Constructor for a FooHandle
  //   - calls native allocator
  //   - exception on failure
  public FooHandle(): base(IntPtr.Zero, ownsHandle: true) {
    var h = stackalloc LibFoo.handle_t*[1];
    LibFoo.handle_alloc(h);
    if (h[0] == (void*)0) {
      throw new Exception("Failed to allocate!");
    }
    SetHandle(h[0]);
  }

  // Deallocator: call native free with CER guarantees from SafeHandle
  override protected bool ReleaseHandle() {
    // Free the native object
    LibFoo.handle_free(this);

    // Invalidate the contained pointer
    SetHandle(IntPtr.Zero);

    return true;
  }

  // Conversions, getters, operators
  public UInt64 get() { return (UInt64)this.handle; }
  private protected void SetHandle(LibFoo.handle_t* h) { SetHandle((IntPtr)h); }
  private protected FooHandle(IntPtr value) : base(value, ownsHandle : false) {}
  public override bool IsInvalid => this.handle == IntPtr.Zero;
  public static implicit operator IntPtr(FooHandle h) => h.handle;
  public static implicit operator LibFoo.handle_t*(FooHandle h) => (LibFoo.handle_t*)h.handle;
  public static implicit operator FooHandle(LibFoo.handle_t* value) => new FooHandle((IntPtr)value);

}
public unsafe partial class LibFoo {
  public unsafe struct handle_t {};

  [DllImport("libfoo.dylib")]
  public unsafe static extern void
  doit(int valin, int *res);

  [DllImport("libfoo.dylib")]
  public static extern void handle_alloc(handle_t** handle);

  [DllImport("libfoo.dylib")]
  public static extern void handle_free(handle_t* handle);

  [DllImport("libfoo.dylib")]
  public static extern void return_string(char** p);

}

namespace CSDemo {

// from https://github.com/dotnet/ClangSharp/blob/67c1e5243b9d58f2b28f10e3f9a82f7537fd9d88/sources/ClangSharp.Interop/Internals/SpanExtensions.cs
// MIT License
public static unsafe class SpanExtensions
{
    public static string AsString(this Span<byte> self) => AsString((ReadOnlySpan<byte>)self);

    public static string AsString(this ReadOnlySpan<byte> self)
    {
        if (self.IsEmpty)
        {
            return string.Empty;
        }

        fixed (byte* pSelf = self)
        {
            return Encoding.UTF8.GetString(pSelf, self.Length);
        }
    }
}

public class Class1 {

  FooHandle theFoo = new FooHandle();

  public unsafe string get_string() {
    char* bp;
    LibFoo.return_string(&bp);
    Console.WriteLine("char* in C# is: {0:X}", (UInt64)bp);

    var span = new ReadOnlySpan<byte>(bp, int.MaxValue);
    return span.Slice(0, span.IndexOf((byte)'\0')).AsString();
  }
  public void runIt() {
    Console.WriteLine("C# handle_t* is: {0:X}", (UInt64)this.theFoo.get());

    Console.WriteLine("String from C# is: {0}", get_string());
  }
}

public class Class2 {
  public static void Main() {
      var c1 = new Class1();
      c1.runIt();
  }
}

} // namespace CSDemo
