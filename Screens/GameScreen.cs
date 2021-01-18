using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace IAmACube
{
    class GameScreen : Screen
    {
        public Save LoadedSave;
        public Camera Camera;

        public GameScreen(Save save)
        {
            LoadedSave = save;
            Camera = new Camera();
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            Camera.Draw(drawingInterface,LoadedSave.World.Centre);
        }

        public override void Update(UserInput input)
        {
            Camera.Update(input);
        }
    }
}
