using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHWarehouseCSharp
{
    public enum HazmatFlags
    {
        NONE = 0x0,
        CHEMICAL = 0x1,
        NUCLEAR = 0x2
    }

    public class Room
    {
        public int volumeInSqareMeters { get; private set; }
        public HazmatFlags hazmatFlags { get; private set; }
        public bool hasStairs { get; private set; }
        public List<Box> boxes;

        public Room(int volume) : this(volume, false) {}

        public Room(int volume, bool hasStairs) : this(volume, hasStairs, 0) {}

        public Room(int volume, bool stairs, HazmatFlags flags)
        {
            volumeInSqareMeters = volume;
            hazmatFlags = flags;
            hasStairs = stairs;
            boxes = new List<Box>();
        }
    }
}
