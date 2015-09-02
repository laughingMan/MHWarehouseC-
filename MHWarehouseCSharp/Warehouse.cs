using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHWarehouseCSharp
{
    public class Warehouse
    {
        public Room[] rooms { get; private set; }

        public Warehouse(Room[] rooms) 
        {
            this.rooms = rooms;
        }

        public Box[] addBoxes(Box[] boxes)
        {
            List<Box> rejectedBoxes = new List<Box>();
            List<Box> sortedBoxes = new List<Box>(boxes);
            sortedBoxes = sortedBoxes.OrderByDescending(box => box.hazmatFlags).ToList();
            foreach (Box box in sortedBoxes)
            {
                bool addedToRoom = false;
                foreach (Room room in this.rooms)
                {
                    if (room.acceptsBox(box))
                    {
                        addedToRoom = true;
                        room.addBox(box);
                        break;
                    }
                }

                if (!addedToRoom)
                {
                    rejectedBoxes.Add(box);
                }
            }
            
            return rejectedBoxes.ToArray();
        }
    }
}
