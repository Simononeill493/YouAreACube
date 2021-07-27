﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract partial class Cube : ICube
    {
        public CubeTemplate Template { get; private set; }
        public string Sprite => Template.Sprite;
        public bool Active => Template.Active;
        public int Speed => Template.Speed;


        public CubeMode BlockType { get; }
        public Orientation Orientation { get; private set; }
        public Tile Location { get; protected set; } = Tile.Dummy;


        public int _id { get; }
        public int SpeedOffset { get; private set; }

        public (int, int, int, int) ColorMask = (255, 255, 255, 255);


        public Cube(CubeTemplate template, CubeMode blockType)
        {
            _id = IDUtils.GenerateBlockID();

            Template = template;
            BlockType = blockType;

            Energy = 0;
            SpeedOffset = _id % Config.TickCycleLength;
        }


        public virtual void Update(UserInput input, ActionsList actions) => Template.Chipset.Execute(this, input, actions);
        public virtual void BeCreatedBy(Cube creator) => SpeedOffset = creator.SpeedOffset + 1;
        public virtual bool CanUpdate => true;
        public abstract bool ToBeDeleted();
        public virtual void DealDamage(int amount) { }


        public void Rotate(int rotation) => Orientation = Orientation.Rotate(rotation);
        public void SetOrientation(Orientation orientation) => Orientation = orientation;
        public bool InSector(Sector sector) => Location.InSector(sector);
        public void ClearLocation() => Location = null;


        public void SetTemplateToMain() => Template = Template.Versions.Main;
        public void SetTemplateToRuntime() => Template = Templates.GetRuntimeTemplate(Template);
    }

    public interface ICube{}
}