using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class CubeTemplate
    {
        public string Name;
        public bool Active;
        public int Speed;

        public int MaxEnergy = 30;
        public int MaxHealth = 250;

        public bool Invincible;

        public string Sprite;
        public CubeSpriteDataType SpriteType;

        [JsonIgnore]
        public Chipset Chipset;

        [JsonIgnore]
        public TemplateVersionDictionary Versions;
        public int Version;

        public TemplateVariableSet Variables;

        public CubeTemplate(string name) => Name = name;

        public virtual Cube Generate(Kernel source,CubeMode blockType)
        {
            switch (blockType)
            {
                case CubeMode.Surface:
                    return GenerateSurface(source);
                case CubeMode.Ground:
                    return GenerateGround(source);
                case CubeMode.Ephemeral:
                    return GenerateEphemeral(source);
                default:
                    throw new Exception("Tried to generate an unhandled block type");
            }
        }
        public virtual SurfaceCube GenerateSurface(Kernel source) => new SurfaceCube(this,source);
        public virtual GroundCube GenerateGround(Kernel source) => new GroundCube(this, source);
        public virtual EphemeralCube GenerateEphemeral(Kernel source) => new EphemeralCube(this, source);
        public CubeTemplate Clone()
        {
            var clone = JsonConvert.DeserializeObject<CubeTemplate>(JsonConvert.SerializeObject(this));
            if(this.Chipset!=null)
            {
                clone.Chipset = Parser_JSONToChipset.ParseJsonToBlock(Parser_ChipsetToJSON.ParseChipsetToJson(Chipset));
            }

            clone.Version = -1;
            return clone;
        }

        public virtual CubeTemplate GetRuntimeTemplate() => Templates.Database[Versions.Name][Version];

        public virtual Dictionary<int, object> GenerateVariables() => Variables.GenerateVariables();

        public override string ToString() => Version + ": " + Name;
        public virtual string ToJsonRep() => Versions.Name + '|' + Version;
    }
}
