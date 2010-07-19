using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterWrapper
{
    public class ConvertedModel
    {

        public string type;
        public byte[] data;
        public List<string> textureFiles;
        public List<string> missingTextures;
        public Parser.ModelData _ModelData;
        public ConvertedModel()
        {
            textureFiles = new List<string>();
            missingTextures = new List<string>();

        }
    }
}
