﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class BlocksetEditPane
    {
        private bool _panning;
        private IntPoint _panPos;

        private void _checkForPan(UserInput input)
        {
            if (_panning)
            {
                if (input.MouseMiddleReleased)
                {
                    _endPan();
                }
                else
                {
                    _pan(input);
                }
            }
            if (MouseHovering & input.MouseMiddleJustPressed)
            {
                _startPan(input);
            }
        }

        private void _startPan(UserInput input)
        {
            _panning = true;
            _panPos = input.MousePos;
        }

        private void _endPan()
        {
            _panning = false;
        }

        private void _pan(UserInput input)
        {
            float trueScale = Scale;// * _blockScaleMultiplier;
            var panDiffBase = input.MousePos - _panPos;
            var panDiff = (panDiffBase / trueScale).Ceiling;

            /*if (panDiffBase != IntPoint.Zero & panDiff == IntPoint.Zero)
            {
                panDiff = panDiffBase;
            }*/

            panDiff *= 2;

            foreach(var blockset in Blocksets.Values.Where(b => !b.IsInternal))
            {
                blockset.OffsetLocationConfig(panDiff);
            }

            _updateChildLocations();

            _panPos = input.MousePos;
        }
    }
}
