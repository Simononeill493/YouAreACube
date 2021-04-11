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
        public ActionType ActionType;
        public Block Actor;

        public CardinalDirection CardinalDir;
        public RelativeDirection RelativeDir;
        public int Rotation;
        public int MoveTotalTicks;

        public BlockMode BlockType;
        public TemplateVersionList BlockTemplate;
        public int TemplateVersion;

        public BlockAction(Block actor,ActionType actionType)
        {
            ActionType = actionType;
            Actor = actor;
        }
    }
}
