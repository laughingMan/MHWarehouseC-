using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHWarehouseCSharp
{
    public class Box
    {
        public int volume { get; private set; }
        public string name { get; private set; }
        public HazmatFlags hazmatFlags { get; private set; }

        public Box(string name, int volume) : this(name, volume, 0) { }

        public Box(string name, int volume, HazmatFlags hazmatFlags)
        {
            this.name = name;
            this.volume = volume;
            this.hazmatFlags = hazmatFlags;
        }
    }
}
