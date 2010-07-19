using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace ConverterWrapper
{
    public class Parser
    {

        //The count verticies function that takes a filename
        [DllImport("ConverterDLL.dll", EntryPoint = "CountVerticiesFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool CountVerticiesFile(char[] filename, ref int out_count, ref int out_polys, ref IntPtr error);

        //The GetTextureReferences function that takes a filename, returns a comma seperated list
        [DllImport("ConverterDLL.dll", EntryPoint = "GetTextureReferencesFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool GetTextureReferencesFile(char[] infilename, ref IntPtr outstring, ref IntPtr error);

        [DllImport("ConverterDLL.dll", EntryPoint = "GetTransformPropertiesFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool GetTransformPropertiesFile(char[] infilename, ref IntPtr outstring, ref IntPtr error);


        //The GetTextureReferences function that takes a filename, returns a comma seperated list
        [DllImport("ConverterDLL.dll", EntryPoint = "GetTextureReferences", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool GetTextureReferences(byte[] data, char[] type, int size, ref IntPtr outstring, ref IntPtr error);

        [DllImport("ConverterDLL.dll", EntryPoint = "GetTransformProperties", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool GetTransformProperties(byte[] data, int size, char[] type, ref IntPtr outstring, ref IntPtr error);

        //The count verticies function that takes a filename
        [DllImport("ConverterDLL.dll", EntryPoint = "CountVerticies", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool CountVerticies(byte[] data, int size, char[] type, ref int out_count, ref int out_Polys, ref IntPtr error);

        //The GetTextureReferences function that takes a filename, returns a comma seperated list
        [DllImport("ConverterDLL.dll", EntryPoint = "GetTexturesOnObjects", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool GetTexturesOnObjects(byte[] data, char[] type, int size, ref IntPtr outstring, ref IntPtr error);

        //The GetTextureReferences function that takes a filename, returns a comma seperated list
        [DllImport("ConverterDLL.dll", EntryPoint = "GetTexturesOnObjectsFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        unsafe private static extern bool GetTexturesOnObjectsFile(char[] infilename, ref IntPtr outstring, ref IntPtr error);



        public struct VertexCount
        {
            public int Polys;
            public int Verts;
            public VertexCount(int in1, int in2)
            {
                Verts = in1;
                Polys = in2;
            }
        }
        //return the vertex count of the object
        public VertexCount CountVerts(string infilename)
        {
            char[] infilenamearray = new char[infilename.Length];

            //copy input string to a char array
            infilename.CopyTo(0, infilenamearray, 0, infilename.Length);

            int count = 0;
            int polys = 0;
            try
            {
                IntPtr error = new IntPtr();
                bool success = CountVerticiesFile(infilenamearray, ref count, ref polys, ref error);
                if (!success)
                    return new VertexCount(-1, -1);
            }
            catch
            {
                return new VertexCount(-1, -1);
            }

            return new VertexCount(count, polys);
        }

        //return the vertex count of the object
        public VertexCount CountVerts(System.IO.Stream stream, string type)
        {
            stream.Seek(0, SeekOrigin.Begin);
            char[] typearray = new char[type.Length];

            //copy input string to a char array
            type.CopyTo(0, typearray, 0, type.Length);

            int count = 0;
            int polys = 0;
            try
            {
                //copy the data to the buffer
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                IntPtr error = new IntPtr();

                //call the DLL
                bool success = CountVerticies(buffer, (int)stream.Length, typearray, ref count, ref polys, ref error);
                GC.ReRegisterForFinalize(buffer);
                //Get the error codes
                string errorstring = Marshal.PtrToStringAnsi(error);
                if (!success)
                    return new VertexCount(-1, -1);
            }
            catch
            {
                return new VertexCount(-1, -1);
            }

            return new VertexCount(count, polys);
        }

        //Get a list of references to textures
        public string[] GetTextureReferences(string infilename)
        {
            string[] refs;
            lock (this)
            {
                //copy the filename string
                char[] infilenamearray = new char[infilename.Length];
                infilename.CopyTo(0, infilenamearray, 0, infilename.Length);

                IntPtr files = new IntPtr();
                //char[] files = new char[1024];
                string errorstring = "";
                try
                {

                    IntPtr error = new IntPtr();
                    //Call the dll
                    bool success = GetTextureReferencesFile(infilenamearray, ref files, ref error);
                    //Get the error
                    errorstring = Marshal.PtrToStringAnsi(error);
                    //Output the error string in the reference if the call failed
                    if (!success)
                    {
                        refs = new string[1];
                        refs[0] = errorstring;
                        return refs;
                    }
                }
                //return exception in reference if call failed
                catch
                {
                    refs = new string[1];
                    refs[0] = "Exception!";
                    return refs;
                }

                //read the files string
                string sfiles = Marshal.PtrToStringAnsi(files);

                //if there are no textures, show the error. This might just read, "no textures"
                if (sfiles == null)
                {
                    refs = new string[1];
                    refs[0] = errorstring;
                    return refs;
                }

                //split up the files and return
                char[] token = { ',' };
                refs = sfiles.Split(token, StringSplitOptions.RemoveEmptyEntries);
            }
            return refs;
        }

        //Get a list of references to textures
        public string[] GetTextureReferences(System.IO.Stream stream, string type)
        {

            stream.Seek(0, SeekOrigin.Begin);
            string[] refs;
            {
                //Copy the strings
                char[] typearray = new char[type.Length];

                if (typearray == null)
                    Console.WriteLine("Allocation Failure!");

                type.CopyTo(0, typearray, 0, type.Length);

                IntPtr files = new IntPtr();
                string errorstring = "";

                try
                {
                    //create a buffer for the data
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    IntPtr error = new IntPtr();
                    //Pass data to the DLL
                    bool success = GetTextureReferences(buffer, typearray, (int)stream.Length, ref files, ref error);
                    buffer = null;
                    GC.Collect();
                    //Get the error codes
                    errorstring = Marshal.PtrToStringAnsi(error);

                    //If failed, copy error to the first reference
                    if (!success)
                    {
                        refs = new string[1];
                        refs[0] = errorstring;
                        return refs;
                    }
                }
                catch
                {
                    refs = new string[1];
                    refs[0] = "Exception!";
                    return refs;
                }

                string sfiles = Marshal.PtrToStringAnsi(files);

                if (sfiles == null)
                {
                    refs = new string[1];
                    refs[0] = errorstring;
                    return refs;
                }

                char[] token = { ',' };
                refs = sfiles.Split(token, StringSplitOptions.RemoveEmptyEntries);
            }
            return refs;
        }

        //Get a list of references to textures
        public System.Collections.Generic.Dictionary<string, string> GetObjectTextureMapping(System.IO.Stream stream, string type)
        {

            stream.Seek(0, SeekOrigin.Begin);
            System.Collections.Generic.Dictionary<string, string> mapping = new Dictionary<string, string>();
            string[] refs;
            {
                //Copy the strings
                char[] typearray = new char[type.Length];

                if (typearray == null)
                    Console.WriteLine("Allocation Failure!");

                type.CopyTo(0, typearray, 0, type.Length);

                IntPtr files = new IntPtr();
                string errorstring = "";

                try
                {
                    //create a buffer for the data
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    IntPtr error = new IntPtr();
                    //Pass data to the DLL
                    bool success = GetTexturesOnObjects(buffer, typearray, (int)stream.Length, ref files, ref error);
                    buffer = null;
                    GC.Collect();
                    //Get the error codes
                    errorstring = Marshal.PtrToStringAnsi(error);

                    //If failed, copy error to the first reference
                    if (!success)
                    {
                        refs = new string[1];
                        refs[0] = errorstring;
                        return mapping;
                    }
                }
                catch
                {
                    refs = new string[1];
                    refs[0] = "Exception!";
                    return mapping;
                }

                string sfiles = Marshal.PtrToStringAnsi(files);

                if (sfiles == null)
                {
                    refs = new string[1];
                    refs[0] = errorstring;
                    return mapping;
                }

                char[] token = { ',' };
                refs = sfiles.Split(token, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < refs.Length; i += 2)
                {
                    mapping[refs[i + 1]] = refs[i];
                }
            }
            return mapping;
        }

        //Get a list of references to textures
        public System.Collections.Generic.Dictionary<string, string> GetObjectTextureMapping(string filename)
        {


            System.Collections.Generic.Dictionary<string, string> mapping = new Dictionary<string, string>();
            string[] refs;
            {
                //Copy the strings
                char[] filenamearray = new char[filename.Length];

                if (filename == null)
                    Console.WriteLine("Allocation Failure!");

                filename.CopyTo(0, filenamearray, 0, filename.Length);

                IntPtr files = new IntPtr();
                string errorstring = "";

                try
                {

                    IntPtr error = new IntPtr();
                    //Pass data to the DLL
                    bool success = GetTexturesOnObjectsFile(filenamearray, ref files, ref error);


                    //Get the error codes
                    errorstring = Marshal.PtrToStringAnsi(error);

                    //If failed, copy error to the first reference
                    if (!success)
                    {
                        refs = new string[1];
                        refs[0] = errorstring;
                        return mapping;
                    }
                }
                catch
                {
                    refs = new string[1];
                    refs[0] = "Exception!";
                    return mapping;
                }

                string sfiles = Marshal.PtrToStringAnsi(files);

                if (sfiles == null)
                {
                    refs = new string[1];
                    refs[0] = errorstring;
                    return mapping;
                }

                char[] token = { ',' };
                refs = sfiles.Split(token, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < refs.Length; i += 2)
                {
                    mapping[refs[i + 1]] = refs[i];
                }
            }
            return mapping;
        }

        public class ModelTransformProperties
        {
            public string UnitName;
            public float UnitMeters;
            public string UpAxis;
        };
        public class ModelData
        {
            public ModelTransformProperties TransformProperties;
            public string[] ReferencedTextures;
            public VertexCount VertexCount;
            public System.Collections.Generic.Dictionary<string, string> TextureMappings;
        };
        public string AxisNumberToString(int number)
        {
            if (number == 0)
                return "X";
            if (number == 1)
                return "Y";
            if (number == 2)
                return "Z";
            return "Y";
        }
        public ModelTransformProperties GetDocumentProperties(string infilename)
        {
            ModelTransformProperties props = new ModelTransformProperties();

            char[] infilenamearray = new char[infilename.Length];

            infilename.CopyTo(0, infilenamearray, 0, infilename.Length);

            IntPtr propstring = new IntPtr();
            //char[] files = new char[1024];
            try
            {
                IntPtr error = new IntPtr();
                bool success = GetTransformPropertiesFile(infilenamearray, ref propstring, ref error);
                if (!success)
                {
                    props.UnitName = "ERROR LOADING!";
                    return props;
                }
            }
            catch
            {
                props.UnitName = "ERROR LOADING!";
                return props;
            }



            string sfiles = Marshal.PtrToStringAnsi(propstring);

            if (sfiles == null)
            {
                props.UnitName = "COULD NOT READ DATA!";
                return props;
            }

            char[] token = { ',' };
            string[] proplist;
            proplist = sfiles.Split(token, StringSplitOptions.RemoveEmptyEntries);
            foreach (string prop in proplist)
            {
                string[] namevalue;
                namevalue = prop.Split(':');
                if (namevalue[0] == "UnitName")
                    props.UnitName = namevalue[1];
                if (namevalue[0] == "UnitMeter")
                    props.UnitMeters = Convert.ToSingle(namevalue[1]);
                if (namevalue[0] == "Up_axis")
                    props.UpAxis = AxisNumberToString(Convert.ToInt32(namevalue[1]));
            }

            return props;
        }
        public ModelData GetModelData(string infilename)
        {
            ModelData data = new ModelData();
            ModelTransformProperties props = GetDocumentProperties(infilename);
            data.TransformProperties = props;
            data.VertexCount = CountVerts(infilename);
            data.ReferencedTextures = GetTextureReferences(infilename);
            data.TextureMappings = GetObjectTextureMapping(infilename);
            return data;
        }
        public ModelTransformProperties GetDocumentProperties(System.IO.Stream stream, string type)
        {
            stream.Seek(0, SeekOrigin.Begin);
            ModelTransformProperties props = new ModelTransformProperties();

            char[] typearray = new char[type.Length];

            type.CopyTo(0, typearray, 0, type.Length);

            IntPtr propstring = new IntPtr();
            //char[] files = new char[1024];
            try
            {
                IntPtr error = new IntPtr();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                bool success = GetTransformProperties(buffer, (int)stream.Length, typearray, ref propstring, ref error);
                GC.ReRegisterForFinalize(buffer);
                if (!success)
                {
                    props.UnitName = "ERROR LOADING!";
                    return props;
                }
            }
            catch
            {
                props.UnitName = "ERROR LOADING!";
                return props;
            }



            string sfiles = Marshal.PtrToStringAnsi(propstring);

            if (sfiles == null)
            {
                props.UnitName = "COULD NOT READ DATA!";
                return props;
            }

            char[] token = { ',' };
            string[] proplist;
            proplist = sfiles.Split(token, StringSplitOptions.RemoveEmptyEntries);
            foreach (string prop in proplist)
            {
                string[] namevalue;
                namevalue = prop.Split(':');
                if (namevalue[0] == "UnitName")
                    props.UnitName = namevalue[1];
                if (namevalue[0] == "UnitMeter")
                    props.UnitMeters = Convert.ToSingle(namevalue[1]);
                if (namevalue[0] == "Up_axis")
                    props.UpAxis = AxisNumberToString(Convert.ToInt32(namevalue[1]));
            }




            return props;
        }
        public ModelData GetModelData(System.IO.Stream stream, string type)
        {
            ModelData data = new ModelData();
            ModelTransformProperties props = GetDocumentProperties(stream, type);
            data.TransformProperties = props;
            data.VertexCount = CountVerts(stream, type);
            data.ReferencedTextures = GetTextureReferences(stream, type);
            data.TextureMappings = GetObjectTextureMapping(stream, type);
            GC.Collect();
            return data;
        }
    }
}
