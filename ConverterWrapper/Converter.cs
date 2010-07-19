using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ConverterWrapper
{
    public class Converter
    {
        //bindings for the conversion that takes a stream
        [DllImport("ConverterDLL.dll", EntryPoint = "Convert", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool Convert(byte[] in_datastream, int in_size, ref IntPtr out_datastream, ref int out_size, char[] input_type, char[] output_type, ref IntPtr errorstring);

        //bindings for the conversions that take file names
        [DllImport("ConverterDLL.dll", EntryPoint = "ConvertFiles", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool ConvertFiles(char[] infilename, char[] out_filename, ref IntPtr error);

        //Convert using filenames
        public bool Convert(string infilename, string outfilename)
        {
            //Copy strings to char arrays
            char[] infilenamearray = new char[infilename.Length];
            char[] outfilenamearray = new char[outfilename.Length];

            infilename.CopyTo(0, infilenamearray, 0, infilename.Length);
            outfilename.CopyTo(0, outfilenamearray, 0, outfilename.Length);

            IntPtr error = new IntPtr();            
            bool success = ConvertFiles(infilenamearray, outfilenamearray, ref error);
            Console.WriteLine(success);
            return success;
        }
        public System.IO.Stream Convert(System.IO.Stream stream, string oldtype, string newtype)
        {
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            //Copy strings to char arrays
            char[] oldtypearray = oldtype.ToCharArray();
            char[] newtypearray = oldtype.ToCharArray();


            //Create buffer for the data in the stream
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);

            int outSize = new int();

            IntPtr error = new IntPtr();
            IntPtr outdata = new IntPtr();
            bool success = Convert(buffer, (int)stream.Length, ref outdata, ref outSize, oldtypearray, newtypearray, ref error);
            Console.WriteLine(success);

            //the conversion failed
            if (outSize <= 0)
                return null;

            byte[] newdata = new byte[outSize];

            Marshal.Copy(outdata, newdata, 0, outSize);

            System.IO.Stream newstream = new System.IO.MemoryStream();
            newstream.Write(newdata, 0, outSize);

            return newstream;
        }
    }
}
