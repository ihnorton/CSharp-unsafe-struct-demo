using System;
using System.Runtime.InteropServices;

namespace ProcessArray
{
    public class AuxWrapper
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
            Console.WriteLine("Hello World!");

            var data = new double[4]{1,2,3,4};
            var outptr = new double[1]{0};
            var size_p = new IntPtr(data.GetLength(0));

            // works w/
            //   double[] data, IntPtr nelem, double[] outptr
            //AuxWrapper.aux_sum_double(data, size_p, outptr);

            // works w/ this
            //   IntPtr data, IntPtr nelem, double[] outptr
            GCHandle h_data = GCHandle.Alloc(data, GCHandleType.Pinned);
            AuxWrapper.aux_sum_double(h_data.AddrOfPinnedObject(), size_p, outptr);

            Console.WriteLine("result: " + outptr[0]);
        }
    }
}
