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
            return null;  //for now, ignore return value
        }
    }
}
