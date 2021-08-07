using System;
using System.Runtime.InteropServices;

namespace ProcessArray
{
    public class AuxWrapper1
    {
        [DllImport("../cpp/build/libaux.dylib")]
        public static extern int aux_sum_double(
           double[] data, IntPtr nelem, double[] outptr
        );
    }
    public class AuxWrapper2
    {
        [DllImport("../cpp/build/libaux.dylib")]
        public static extern int aux_sum_double(
           IntPtr data, IntPtr nelem, double[] outptr
        );
    }
    class Program
    {
        static void Main(string[] args)
        {
            var data = new double[4] { 1, 2, 3, 4 };
            var outptr = new double[1] { 0 };
            var size_p = new IntPtr(data.GetLength(0));

            Console.WriteLine("calling array sig:");
            AuxWrapper1.aux_sum_double(data, size_p, outptr);
            Console.WriteLine("result: " + outptr[0]);

            outptr[0] = 0;

            Console.WriteLine("calling IntPtr + GCHandle:");
            GCHandle h_data = GCHandle.Alloc(data, GCHandleType.Pinned);
            AuxWrapper2.aux_sum_double(h_data.AddrOfPinnedObject(), size_p, outptr);

            Console.WriteLine("result: " + outptr[0]);
        }
    }
}
