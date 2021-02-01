using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Action
    {
        public ActionType ActionType;
        public Block Actor;

        public CardinalDirection CardinalDir;
        public RelativeDirection RelativeDir;
        public int Rotation;
        public int MoveSpeed;

        public BlockType BlockType;
        public BlockTemplate BlockTemplate;

        public Action(Block actor,ActionType actionType)
        {
            ActionType = actionType;
            Actor = actor;
        }
    }
}
