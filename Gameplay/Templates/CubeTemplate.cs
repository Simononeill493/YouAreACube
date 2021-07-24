﻿using Newtonsoft.Json;
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
        public string Sprite;
        public bool Active;
        public int Speed;

        public int MaxEnergy = 30;
        public int MaxHealth = 250;

        [JsonIgnore]
        public Chipset Chipset;

        [JsonIgnore]
        public TemplateVersionDictionary Versions;
        public int Version;

        public CubeTemplate(string name) => Name = name;


        public Cube Generate(CubeMode blockType)
        {
            switch (blockType)
            {
                case CubeMode.Surface:
                    return GenerateSurface();
                case CubeMode.Ground:
                    return GenerateGround();
                case CubeMode.Ephemeral:
                    return GenerateEphemeral();
                default:
                    throw new Exception("Tried to generate an unhandled block type");
            }
        }
        public SurfaceCube GenerateSurface() => new SurfaceCube(this);
        public GroundCube GenerateGround() => new GroundCube(this);
        public EphemeralCube GenerateEphemeral() => new EphemeralCube(this);
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

        public override string ToString() => Version + ": " + Name;
    }
}
