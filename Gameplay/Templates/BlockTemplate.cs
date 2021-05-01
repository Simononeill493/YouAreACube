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
        public string Name;
        public string Sprite;
        public bool Active;
        public int Speed;

        public int MaxEnergy = 30;
        public int MaxHealth = 250;

        [JsonIgnore]
        public ChipBlock ChipBlock;

        [JsonIgnore]
        public TemplateVersionDictionary Versions;
        public int Version;

        public BlockTemplate(string name) => Name = name;


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
                default:
                    throw new Exception("Tried to generate an unhandled block type");
            }
        }
        public SurfaceBlock GenerateSurface() => new SurfaceBlock(this);
        public GroundBlock GenerateGround() => new GroundBlock(this);
        public EphemeralBlock GenerateEphemeral() => new EphemeralBlock(this);
        public BlockTemplate Clone()
        {
            var clone = JsonConvert.DeserializeObject<BlockTemplate>(JsonConvert.SerializeObject(this));
            clone.ChipBlock = ChipBlockParser.ParseJsonToBlock(ChipBlockParser.ParseBlockToJson(ChipBlock));

            clone.Version = -1;
            return clone;
        }

        public override string ToString() => Version + ": " + Name;
    }
}
