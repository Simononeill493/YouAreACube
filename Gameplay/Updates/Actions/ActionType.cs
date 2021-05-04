﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public enum ActionType
    {
        CardinalMovement,
        RelativeMovement,
        Rotation,
        CardinalCreation,
        RelativeCreation
    }
}
