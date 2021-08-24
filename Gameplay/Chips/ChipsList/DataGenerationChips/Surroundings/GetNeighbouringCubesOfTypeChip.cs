using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    internal class GetNeighbouringCubesOfTypeChip: InputPin1<CubeMode>, OutputPin<List<Cube>>
    {
        public List<Cube> Value { get; set; }

        public override void Run(Cube actor, UserInput input, ActionsList actions)
        {
            var neighbours = actor.Location.Adjacent.Where(l => l.Value.HasCube(ChipInput1(actor))).Select(l => l.Value.GetBlock(ChipInput1(actor))).ToList();
            Value = (neighbours);
        }
    }
}

