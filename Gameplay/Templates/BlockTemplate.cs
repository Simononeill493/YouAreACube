using Newtonsoft.Json;
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
        public int Version;

        public string Name;
        public string Sprite;
        public bool Active;
        public int Speed;

        public int InitialEnergy;

        [JsonIgnore]
        public ChipBlock Chips;

        [JsonIgnore]
        public TemplateVersionList Versions;

        public BlockTemplate(string name)
        {
            Name = name;
        }

        public Block Generate(BlockMode blockType)
        {
            switch (blockType)
            {
                case BlockMode.Surface:
                    return GenerateSurface();
                case BlockMode.Ground:
                    return GenerateGround();
                case BlockMode.Ephemeral:
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
            return Version + ": " + Name;
        }

        public BlockTemplate Clone()
        {
            var cloneJson = JsonConvert.SerializeObject(this);
            var clone = JsonConvert.DeserializeObject<BlockTemplate>(cloneJson);

            var cloneChipsJson = ChipBlockParser.ParseBlockToJson(Chips);
            var cloneChips = ChipBlockParser.ParseJsonToBlock(cloneChipsJson);
            clone.Chips = cloneChips;

            return clone;
        }

    }
}
