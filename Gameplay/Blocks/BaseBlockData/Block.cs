using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract class Block
    {
        public Tile Location;
        public BlockType BlockType;
        public int SpeedOffset;

        public bool IsMoving;
        public BlockMovementData MovementData;

        public string Sprite => _template.Sprite;
        public bool Active => _template.Active;
        public int Speed => _template.Speed;
        protected BlockTemplate _template;

        public Block(BlockTemplate template)
        {
            _template = template;
            SpeedOffset = RandomUtils.R.Next(0, Config.TickCycleLength);
            MovementData = new BlockMovementData();
        }

        public void Update(UserInput input,EffectsList effects)
        {
            _template.Chips.Execute(this, input,effects);
        }

        public abstract void Move(Direction direction);
    }
}
