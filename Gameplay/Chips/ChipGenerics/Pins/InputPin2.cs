﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract class InputPin2<TInputType1,TInputType2> : InputPin1<TInputType1>
    {
        public TInputType2 ChipInput2
        {
            get
            {
                switch (InputType2)
                {
                    case InputOptionType.Value:
                        return InputValue2;
                    case InputOptionType.Reference:
                        return InputReference2.Value;
                };
                return default;
            }
        }

        public InputOptionType InputType2 { get; set; }

        public OutputPin<TInputType2> InputReference2
        {
            get
            {
                return _inputReference2;
            }
            set
            {
                _inputReference2 = value;
                InputType2 = InputOptionType.Reference;
            }
        }
        private OutputPin<TInputType2> _inputReference2;

        public TInputType2 InputValue2
        {
            get
            {
                return _inputValue2;
            }
            set
            {
                _inputValue2 = value;
                InputType2 = InputOptionType.Value;
            }
        }
        private TInputType2 _inputValue2;
    }
}
