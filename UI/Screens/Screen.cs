using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class Screen
    {
        public readonly ScreenType ScreenType;
        protected Action<ScreenType> SwitchScreen;

        public List<(Keys, Action<UserInput>)> _keysJustReleasedEvents;
        public List<(Keys, Action<UserInput>)> _keysJustPressedEvents;

        protected int _drawTimer;

        public Screen(ScreenType screenType,Action<ScreenType> switchScreen)
        {
            ScreenType = screenType;
            SwitchScreen = switchScreen;
            _keysJustReleasedEvents = new List<(Keys, Action<UserInput>)>();
            _keysJustPressedEvents = new List<(Keys, Action<UserInput>)>();
        }

        public void Update(UserInput input)
        {
            foreach(var keyEvent in _keysJustReleasedEvents)
            {
                if(input.IsKeyJustReleased(keyEvent.Item1))
                {
                    keyEvent.Item2(input);
                }
            }

            foreach (var keyEvent in _keysJustPressedEvents)
            {
                if (input.IsKeyJustPressed(keyEvent.Item1))
                {
                    keyEvent.Item2(input);
                }
            }

            _update(input);
        }
        public virtual void _update(UserInput input) { }

        public virtual void Draw(DrawingInterface drawingInterface)
        {
            _drawTimer++;
        }

        public void AddKeyJustReleasedEvent(Keys key, Action<UserInput> action)
        {
            _keysJustReleasedEvents.Add((key, action));
        }

        public void AddKeyJustPressedEvent(Keys key, Action<UserInput> action)
        {
            _keysJustPressedEvents.Add((key, action));
        }
    }
}
