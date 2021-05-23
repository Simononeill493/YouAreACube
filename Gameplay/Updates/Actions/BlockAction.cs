using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class BlockAction
    {
        public Block Actor;
        public Block TargetBlock;
        public Tile TargetTile;

        public ActionType ActionType;

        public CardinalDirection CardinalDir;
        public RelativeDirection RelativeDir;
        public BlockMode BlockType;

        public int EnergyAmount;
        public int Rotation;
        public int MoveSpeed;

        public BlockTemplate Template;

        public BlockAction(Block actor,ActionType actionType)
        {
            ActionType = actionType;
            Actor = actor;
        }
    }
}
