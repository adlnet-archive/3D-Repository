using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

/* DLL linkage data
CONVERTLIBRARY_DLL  bool __cdecl Convert(const char* in_datastream,int in_size, char*& out_datastream, int& out_size, char* input_type,const char* outtype,char*& error);
CONVERTLIBRARY_DLL  bool __cdecl ConvertFiles(const char* infilename, char* out_filename,char*& error);

CONVERTLIBRARY_DLL  bool __cdecl CountVerticies(const char* in_datastream,int in_size, int& out_count,char*& error);
CONVERTLIBRARY_DLL  bool __cdecl CountVerticiesFile(const char* in_filename, int& out_count,char*& error);

CONVERTLIBRARY_DLL  bool __cdecl GetTextureReferences(const char* in_datastream,const char* type,int in_size, char*& outtext,char*& error);
CONVERTLIBRARY_DLL  bool __cdecl GetTextureReferencesFile(const char* infilename, char*& outtext,char*& error);

CONVERTLIBRARY_DLL  bool __cdecl GetTransformPropertiesFile(const char* infilename, char*& outtext,char*& error);
CONVERTLIBRARY_DLL  bool __cdecl GetTransformProperties(const char* in_datastream, int in_size, char* type, char*& outtext,char*& error);

CONVERTLIBRARY_DLL  bool __cdecl VerifyTypeFile(const char* infilename,const char* filetype, char*& results,char*& error);
CONVERTLIBRARY_DLL  bool __cdecl VerifyType(const char* in_datastream, int in_size, char* type,char*& error);
 * 
 * CONVERTLIBRARY_DLL  bool __cdecl SetParserDir(const char* in_datastream)
 * */
namespace ConverterWrapper
{
    public class Utility3D
    {
        //conversion functions
        [DllImport("ConverterDLL.dll", EntryPoint = "Startup", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool Initialize(char[] pluginPath);
        [DllImport("ConverterDLL.dll", EntryPoint = "Shutdown", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool Shutdown();
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        //IS the underlying DLL initialized?
        static bool Initialized = false;

        //initialize the library
        public void Initialize(string inPath)
        {
            if (Initialized == false)
            {
                //find sub-dll's here
                SetDllDirectory(inPath);

                //build the current path, copy to string
                inPath = System.IO.Path.Combine(inPath, "osgPlugins-2.9.7");
                char[] charpath = new char[inPath.Length];
                inPath.CopyTo(0, charpath, 0, inPath.Length);

                //send to dll
                Initialize(charpath);
                Initialized = true;
            }
        }
        //reset the dll search path
        unsafe public void Denitialize()
        {
            SetDllDirectory("");
            Shutdown();
        }



        //functions to parse data, find dependancies




    }
}