using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConverterWrapper
{
    public class ModelPackager
    {
        //does this file extension represent a 3d model?
        private bool Is_3D_File(string intype)
        {
            if (intype.ToLower() == ".3ds" ||
                intype.ToLower() == ".obj" ||
                intype.ToLower() == ".dae" ||
                intype.ToLower() == ".flt" ||
                intype.ToLower() == ".fbx" ||
                intype.ToLower() == ".wrl" ||
                intype.ToLower() == ".lwo")
                return true;

            else return false;
        }

        //is the extension provided an extension for an iamge type?
        private bool Is_Image_File(string intype)
        {
            if (intype.ToLower() == ".jpg" ||
                intype.ToLower() == ".gif" ||
                intype.ToLower() == ".tga" ||
                intype.ToLower() == ".png" ||
                intype.ToLower() == ".tiff" ||
                intype.ToLower() == ".exr" ||
                intype.ToLower() == ".bmp" ||
                intype.ToLower() == ".dds")
                return true;

            else return false;
        }

        public ConvertedModel Convert(byte[] indata, char[] infilename, string outputType = ".dae")
        {
            //create the structure to return the data;
            ConvertedModel newModel = new ConvertedModel();


            if (Path.GetExtension(new string(infilename)).ToLower() == ".zip")
            {

                //write the input data into an iostream and open as a zipfile
                System.IO.MemoryStream io = new System.IO.MemoryStream();
                io.Write(indata, 0, indata.Length);
                io.Seek(0, SeekOrigin.Begin);
                //is this stream a zipfile?
                bool isZip = Ionic.Zip.ZipFile.IsZipFile(io, false);

                //if the file is a zip file
                if (isZip)
                {
                    //the zipfile object
                    io.Seek(0, SeekOrigin.Begin);
                    using (Ionic.Zip.ZipFile zipFile = Ionic.Zip.ZipFile.Read(io))
                    {
                        //a list to hold the zipped files
                        List<Ionic.Zip.ZipEntry> ZippedFiles = new List<Ionic.Zip.ZipEntry>();

                        System.Collections.Generic.IEnumerator<Ionic.Zip.ZipEntry> i = zipFile.GetEnumerator();

                        //read each file from the zip into the list of files
                        do
                        {
                            ZippedFiles.Add(i.Current);
                        } while (i.MoveNext());

                        //loop over each file
                        for (int j = 0; j < ZippedFiles.Count; j++)
                        {
                            //the file
                            Ionic.Zip.ZipEntry entry = ZippedFiles[j];
                            if (entry != null)
                            {
                                string filename = entry.FileName;

                                //find the 3d file in this zip
                                if (Is_3D_File(Path.GetExtension(filename)))
                                {
                                    //do conversion
                                    using (System.IO.MemoryStream extractedstream = new System.IO.MemoryStream())
                                    {
                                        entry.Extract(extractedstream);

                                        Utility3D _3D = new Utility3D();
                                        //_3D.Initialize(System.IO.Path.Combine(Context.Request.PhysicalApplicationPath, "bin"));
                                        Parser Parser = new Parser();
                                        Parser.ModelData data = Parser.GetModelData(extractedstream, Path.GetExtension(filename));
                                        //Utility_3D.Denitialize();

                                        newModel._ModelData = data;

                                        if (Path.GetExtension(filename).ToLower() != outputType)
                                        {

                                            //convert the file to dae
                                            Converter Converter = new Converter();
                                            System.IO.Stream convertedmodel = Converter.Convert(extractedstream, Path.GetExtension(filename), outputType);
                                            //if the conversion worked, or at least got a conversion
                                            if (convertedmodel != null && convertedmodel.Length > 0)
                                            {
                                                //remove the old file
                                                zipFile.RemoveEntry(entry);
                                                //add the new datastream
                                                Ionic.Zip.ZipEntry newDAEEntry = new Ionic.Zip.ZipEntry();
                                                string newFileName = Path.GetFileNameWithoutExtension(filename) + outputType;
                                                zipFile.AddEntry(newFileName, "", convertedmodel);
                                            }
                                        }
                                    }
                                }
                                //if the file is an image file add it to the list of included images
                                if (Is_Image_File(Path.GetExtension(filename)))
                                {
                                    newModel.textureFiles.Add(filename);
                                }
                            }
                        }
                        //write the modified zipfile back to the return data
                        using (System.IO.MemoryStream outzipstream = new MemoryStream())
                        {
                            zipFile.Save(outzipstream);
                            outzipstream.Seek(0, SeekOrigin.Begin);
                            byte[] outarray = new byte[outzipstream.Length];
                            outzipstream.Read(outarray, 0, (int)outzipstream.Length);
                            newModel.data = outarray;
                        }
                    }
                }
            }
            //if the file is not a zup file, but a 3D file
            if (Is_3D_File(Path.GetExtension(new string(infilename))))
            {
                //write to a stream
                using (System.IO.MemoryStream extractedstream = new System.IO.MemoryStream())
                {
                    extractedstream.Read(indata, 0, indata.Length);

                    //parse it
                    Utility3D _3D = new Utility3D();
                    //_3D.Initialize(System.IO.Path.Combine(Context.Request.PhysicalApplicationPath, "bin"));
                    Parser Parser = new Parser();
                    Parser.ModelData data = Parser.GetModelData(extractedstream, Path.GetExtension(new string(infilename)));


                    newModel._ModelData = data;
                }
            }

            return newModel;
        }
    }
}
