using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ConverterWrapper
{
    //functions to verify file type and find files
    public class Verifier
    {
        [DllImport("ConverterDLL.dll", EntryPoint = "VerifyTypeFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool VerifyFile(char[] infilename, char[] outfilename, ref IntPtr errorstring);

        [DllImport("ConverterDLL.dll", EntryPoint = "VerifyType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool Verify(byte[] data, int size, char[] type, ref IntPtr text, ref IntPtr errorstring);

        //veriry wheter or not the supplied file is of the supplied type
        public bool Verify(string infilename, string type)
        {
            //copy strings
            char[] infilenamearray = new char[infilename.Length];
            char[] infiletype = new char[type.Length];

            infilename.CopyTo(0, infilenamearray, 0, infilename.Length);
            type.CopyTo(0, infiletype, 0, infiletype.Length);
            IntPtr error = new IntPtr();

            //call DLL
            bool success = VerifyFile(infilenamearray, infiletype, ref error);

            //Get error code
            string errorstring = Marshal.PtrToStringAnsi(error);
            Console.WriteLine(errorstring);

            //Return result
            return success;
        }

        //Verify if the given stream contains the given type of data
        public bool Verify(System.IO.Stream stream, string type)
        {
            //copy strings
            char[] infiletype = new char[type.Length];

            type.CopyTo(0, infiletype, 0, infiletype.Length);
            IntPtr error = new IntPtr();

            //Create buffer for the data in the stream
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);

            //Send to the DLL
            bool success = Verify(buffer, (int)stream.Length, infiletype, ref error, ref error);
            GC.ReRegisterForFinalize(buffer);

            //read the error
            string errorstring = Marshal.PtrToStringAnsi(error);
            Console.WriteLine(errorstring);
            return success;
        }
    }
}
