using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
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
  public static extern void return_string(sbyte** p);

  [DllImport("libfoo.dylib")]
  public static extern void return_int32_array(void** p, UInt32* size);

  [DllImport("libfoo.dylib")]
  public static extern void set_uint64(UInt64 size_put, UInt64* size);
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

public unsafe partial class LibC {
  public unsafe struct handle_t {};

  [DllImport("libc")]
  public unsafe static extern void
  free(void* p);
}

public unsafe class OutString : IDisposable {
  public sbyte* char_p;

 public OutString() { char_p = null; }
 public static implicit operator string(OutString s) {
    if (s.char_p == null) {
      return String.Empty;
    } else {
      var span = new ReadOnlySpan<byte>((char*)s.char_p, int.MaxValue);
      return span.Slice(0, span.IndexOf((byte)'\0')).AsString();
    }
  }

  public void Dispose() => Dispose(true);

  protected virtual void Dispose(bool disposing) {
    if (char_p != null) {
      LibC.free(char_p);
    }
    char_p = null;
  }

  ~OutString() {
    Dispose(false);
  }
}
public class Class1 {

  FooHandle theFoo = new FooHandle();

  internal unsafe struct BufferSizes {
    public fixed UInt64 data[1];
    public fixed UInt64 offsets[1];
    public fixed UInt64 validity[1];

    public BufferSizes(UInt64 data, UInt64 offsets, UInt64 validity) {
      this.data[0] = data;
      this.offsets[0] = offsets;
      this.validity[0] = validity;
    }
    public override string ToString() {
      fixed (UInt64* d = this.data)
      fixed (UInt64* o = this.offsets)
      fixed (UInt64* v = this.validity) {
        return *d + " " + *o + " " + *v;
      }
    }
  }

  public unsafe string get_string() {
    var s = new OutString();
    fixed (sbyte** char_p = &s.char_p) {
      LibFoo.return_string(char_p);
    }

    return s;
  }

  public unsafe Int32[] get_array() {
    ReadOnlySpan<byte> bytes_span;
    Int32 size = 0;
    void* p = null;

    Console.WriteLine("here!");
    LibFoo.return_int32_array(&p, (UInt32*)&size);

    Console.WriteLine("p is: {0:X}", (UInt64)p);
    Console.WriteLine("size is: {0}", size);

    bytes_span = new ReadOnlySpan<byte>(p, size);

    var res = MemoryMarshal.Cast<byte, Int32>(bytes_span).Slice(0).ToArray();
    Console.WriteLine("res is: {0}", res.ToString());
    return res;
  }
  public void runIt() {
    Console.WriteLine("C# handle_t* is: {0:X}", (UInt64)this.theFoo.get());

    Console.WriteLine("\n----\n");

    var s = get_string();
    Console.WriteLine("String from C# is: {0}", s);
    GC.Collect();
  }

  // usage testing, replaced by get_array
  public Int32 do_span() {
    var a1 = new Int32[3]{1000,2,3};

    var a1_bytes = MemoryMarshal.AsBytes(a1.AsSpan());
    var a2_span = MemoryMarshal.Cast<byte, Int32>(a1_bytes);

    return a2_span.Slice(0,1)[0];
  }

  public unsafe void do_sizes() {
    Dictionary<string, Class1.BufferSizes> sizes = new Dictionary<string, BufferSizes>();

    sizes.Add("foo", new Class1.BufferSizes(0,0,0));

    var x = sizes["foo"];
    LibFoo.set_uint64(11, x.data);

    Console.WriteLine("val return is: {0}", sizes["foo"]);
    Console.WriteLine("===============");
  }
}

public class Class2 {
  public static void Main() {
      var c1 = new Class1();
      c1.do_sizes();

      c1.runIt();

      Console.WriteLine("--------- --------");

      var res = c1.get_array();
      Console.WriteLine("array from C is: '{0}'", string.Join(", ", res));
  }
}

} // namespace CSDemo
