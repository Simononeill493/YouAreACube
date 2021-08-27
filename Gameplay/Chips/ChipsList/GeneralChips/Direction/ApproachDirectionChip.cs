using System;

namespace IAmACube
{
    [Serializable()]
    internal class ApproachDirectionChip : InputPin1<Tile>, OutputPin<CardinalDirection>
    {
        public CardinalDirection Value { get; set; }

        public override void Run(Cube actor,UserInput userInput, ActionsList actions)
        {
            var targetTile = ChipInput1(actor);
            if(targetTile!=null)
            {
                Value = (actor.Location.AbsoluteLocation.ApproachDirection(targetTile.AbsoluteLocation));
            }
        }
    }
}