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

        public int EnergyCap = 30;
        public int MaxHealth = 250;

        [JsonIgnore]
        public ChipBlock Chips;

        [JsonIgnore]
        public TemplateAllVersions Versions;

        public BlockTemplate(string name) => Name = name;

        public SurfaceBlock GenerateSurface() => new SurfaceBlock(this);
        public GroundBlock GenerateGround() => new GroundBlock(this);
        public EphemeralBlock GenerateEphemeral() => new EphemeralBlock(this);


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

        public BlockTemplate Clone()
        {
            var cloneJson = JsonConvert.SerializeObject(this);
            var clone = JsonConvert.DeserializeObject<BlockTemplate>(cloneJson);

            var cloneChipsJson = ChipBlockParser.ParseBlockToJson(Chips);
            var cloneChips = ChipBlockParser.ParseJsonToBlock(cloneChipsJson);
            clone.Chips = cloneChips;

            return clone;
        }

        public override string ToString() => Version + ": " + Name;
    }
}
