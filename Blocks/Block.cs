using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Block
    {
        public string Sprite;

        public Block Clone()
        {
            var clone = (Block)this.MemberwiseClone();

            //manually clone all subobjects

            return clone;
        }
    }
}
