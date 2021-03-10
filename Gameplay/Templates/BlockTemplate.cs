using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class BlockTemplate
    {
        public string Name;
        public string Sprite;
        public bool Active;
        public int Speed;

        public int InitialEnergy;

        public ChipBlock Chips;

        public BlockTemplate(string name)
        {
            Name = name;
        }

        public Block Generate(BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.Surface:
                    return GenerateSurface();
                case BlockType.Ground:
                    return GenerateGround();
                case BlockType.Ephemeral:
                    return GenerateEphemeral();
            }

            throw new Exception("Tried to generate an unhandled block type");
        }
        public SurfaceBlock GenerateSurface()
        {
            var block = new SurfaceBlock(this);
            //todo set any dynamic data here
            return block;
        }
        public GroundBlock GenerateGround()
        {
            var block = new GroundBlock(this);
            //todo set any dynamic data here
            return block;
        }
        public EphemeralBlock GenerateEphemeral()
        {
            var block = new EphemeralBlock(this);
            //todo set any dynamic data here
            return block;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
