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
        public Cube Actor;
        public Cube TargetBlock;
        public Tile TargetTile;

        public ActionType ActionType;

        public CardinalDirection CardinalDir;
        public RelativeDirection RelativeDir;
        public CubeMode BlockType;

        public int EnergyAmount;
        public int Rotation;
        public int MoveSpeed;

        public CubeTemplate Template;

        public BlockAction(Cube actor,ActionType actionType)
        {
            ActionType = actionType;
            Actor = actor;
        }
    }
}
