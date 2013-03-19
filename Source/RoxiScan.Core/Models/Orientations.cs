using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoxiScan.Models
{
    public static class Orientations
    {
        public static Orientation Portrait = new Orientation("Portrait", 0);
        public static Orientation Landscape = new Orientation("Landscape", 1);

        public static List<Orientation> List
        {
            get { return new List<Orientation> { Portrait, Landscape }; }
        }
    }

    [DataContract]
    public class Orientation
    {
        public Orientation(string name, int direction)
        {
            Name = name;
            Direction = direction;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Direction { get; set; }
    }
}
