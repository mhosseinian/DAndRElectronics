using System;
using System.IO;

namespace PatternBuilderLib.Models
{
    public class OutPatternModel
    {

        public byte[] Outs { get; set; }

        #region Contructors

        //Constructor from BinaryReader
        public OutPatternModel(BinaryReader reader, byte n)
        {
            Outs = new byte[n];
            reader.Read(Outs, 0, n);
        }

        //Copy constructor
        public OutPatternModel(OutPatternModel model)
        {
            Outs = new byte[model.Outs.Length];
            Array.Copy(model.Outs, Outs, model.Outs.Length);
        }

        public OutPatternModel(byte[] outs)
        {
            Outs = new byte[outs.Length];
            Array.Copy(outs, Outs, Outs.Length);
        }

        //Default constructor
        public OutPatternModel()
        {
        }

        #endregion

        public void Write(BinaryWriter writer)
        {
            writer.Write(Outs);
        }
    }
}