using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract partial class Cube
    {
        public CubeTemplate Template { get; private set; }
        public bool Active => Template.Active;
        public int Speed => Template.Speed;

        public CubeMode CubeMode { get; }
        public Orientation Orientation { get; private set; }
        public Tile Location { get; protected set; } = Tile.Dummy;
        public Kernel Source;

        public int _id { get; }
        public int SpeedOffset { get; private set; }



        public Cube(CubeTemplate template, Kernel source, CubeMode cubeMode)
        {
            Template = template;
            CubeMode = cubeMode;
            Source = source;
            Energy = 0;

            _id = IDUtils.GenerateBlockID();
            SpeedOffset = _id % Config.TickCycleLength;

            InitializeSpriteData();
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



        public void UpdateAsCompanion()
        {
            SetTemplateToMain();
            InitializeSpriteData();
        }

        public void SetTemplateToMain() => Template = Template.Versions.Main;
        public void SetTemplateToRuntime() => Template = Template.GetRuntimeTemplate();
    }
}
