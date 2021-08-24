using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract class InputPin1<TInputType1> : IChip
    {
        public string Name { get; set; }
        public abstract void Run(Cube actor, UserInput userInput, ActionsList actions);

        public TInputType1 ChipInput1(Cube actor)
        { 
            switch (InputType1)
            {
                case InputOptionType.Value:
                    return InputValue1;
                case InputOptionType.Reference:
                    return InputReference1.Value;
                case InputOptionType.Variable:
                    return (TInputType1)(actor.Variables[InputVariable1]);
            };
            return default;
        }


        public InputOptionType InputType1 { get; set; }

        public OutputPin<TInputType1> InputReference1
        {
            get
            {
                return _inputReference1;
            }
            set
            {
                _inputReference1 = value;
                InputType1 = InputOptionType.Reference;
            }
        }
        private OutputPin<TInputType1> _inputReference1;

        public TInputType1 InputValue1
        {
            get
            {
                return _inputValue1;
            }
            set
            {
                _inputValue1 = value;
                InputType1 = InputOptionType.Value;
            }
        }
        private TInputType1 _inputValue1;


        public int InputVariable1
        {
            get
            {
                return _inputVariable1;
            }
            set
            {
                _inputVariable1 = value;
                InputType1 = InputOptionType.Variable;
            }
        }
        private int _inputVariable1;

    }
}
