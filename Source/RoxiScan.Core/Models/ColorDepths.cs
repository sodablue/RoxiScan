using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoxiScan.Models
{
    public static class ColorDepths
    {
        public static ColorDepth BlackAndWhite = new ColorDepth("Black and White", 4, 1);
        public static ColorDepth Greyscale = new ColorDepth("Greyscale", 2, 8);
        public static ColorDepth Color = new ColorDepth("Color", 1, 24);

        public static List<ColorDepth> List
        {
            get { return new List<ColorDepth> { BlackAndWhite, Greyscale, Color }; }
        }
    }

    [DataContract]
    public class ColorDepth
    {
        public ColorDepth(string name, int value, int bpp)
        {
            Name = name;
            Value = value;
            BitsPerPixel = bpp;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Value { get; set; }
        [DataMember]
        public int BitsPerPixel { get; set; }
    }
}
