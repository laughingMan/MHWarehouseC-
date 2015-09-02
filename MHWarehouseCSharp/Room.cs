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
        private int currentAvailableSpace;

        public Room(int volume) : this(volume, false) {}

        public Room(int volume, bool hasStairs) : this(volume, hasStairs, 0) {}

        public Room(int volume, bool stairs, HazmatFlags flags)
        {
            volumeInSqareMeters = volume;
            hazmatFlags = flags;
            hasStairs = stairs;
            boxes = new List<Box>();
            currentAvailableSpace = volume;
        }

        public void addBox(Box box)
        {
            currentAvailableSpace -= box.volume;
            this.boxes.Add(box);
        }

        public bool acceptsBox(Box box)
        {
            bool hasCorrectFlags = box.hazmatFlags != HazmatFlags.NONE && (box.hazmatFlags & this.hazmatFlags) != box.hazmatFlags;
            bool canUseStairs = !this.hasStairs || box.volume <= 50;
            bool hasSpace = currentAvailableSpace >= box.volume;
            HazmatFlags boxHazmat = box.hazmatFlags;
            HazmatFlags roomHazmat = this.hazmatFlags;

            return canUseStairs && !hasCorrectFlags && hasSpace;
        }
    }
}
