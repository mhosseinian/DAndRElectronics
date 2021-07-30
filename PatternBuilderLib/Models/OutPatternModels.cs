using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PatternBuilderLib.Models
{
    public class OutPatternModels
    {
        [JsonIgnore] private const byte MaxOuts = 22;

        private const byte VersionNr = 1;
        public List<OutPatternModel> Models { get; set; } = new List<OutPatternModel>();

        #region Contructors

        public OutPatternModels()
        {
            
        }

        #endregion


        public OutPatternModel NewOutPatternModel()
        {
            var outs =  Enumerable.Repeat((byte)127, MaxOuts).ToArray();
            var model = new OutPatternModel(outs);
            Models.Add(model);
            return model;
        }


        public void Write(BinaryWriter writer)
        {
            //Write the version
            writer.Write(VersionNr);
            //Write number of out lines
            writer.Write((byte)Models.Count);

            foreach (var outPatternModel in Models)
            {
                outPatternModel.Write(writer);
            }
        }


        public void Read(BinaryReader reader)
        {
            //First byte is reserved for version number
           
            var v = reader.ReadByte();
            if (v != 1)
            {
                Debug.Assert(false, $"Wrong version in OutPatternModels. Version:{v}");
                return;
            }

            Models = new List<OutPatternModel>();
            var nOuts = reader.ReadByte();
            for (var i = 0; i < nOuts; i++)
            {
                Models.Add(new OutPatternModel(reader, MaxOuts));
            }
        }

        public static OutPatternModels Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<OutPatternModels>(content);
        }
    }
}