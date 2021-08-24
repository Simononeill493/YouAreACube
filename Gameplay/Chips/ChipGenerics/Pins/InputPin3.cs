using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract class InputPin3<TInputType1, TInputType2, TInputType3> : InputPin2<TInputType1,TInputType2>
    {
        public TInputType3 ChipInput3(Cube actor)
        {
            switch (InputType3)
            {
                case InputOptionType.Value:
                    return InputValue3;
                case InputOptionType.Reference:
                    return InputReference3.Value;
                case InputOptionType.Variable:
                    return (TInputType3)(actor.Variables[InputVariable3]);
            };
            return default;
        }

        public InputOptionType InputType3 { get; set; }

        public OutputPin<TInputType3> InputReference3
        {
            get
            {
                return _inputReference3;
            }
            set
            {
                _inputReference3 = value;
                InputType3 = InputOptionType.Reference;
            }
        }
        private OutputPin<TInputType3> _inputReference3;

        public TInputType3 InputValue3
        {
            get
            {
                return _inputValue3;
            }
            set
            {
                _inputValue3 = value;
                InputType3 = InputOptionType.Value;
            }
        }
        private TInputType3 _inputValue3;

        public int InputVariable3
        {
            get
            {
                return _inputVariable3;
            }
            set
            {
                _inputVariable3 = value;
                InputType3 = InputOptionType.Variable;
            }
        }
        private int _inputVariable3;
    }
}
