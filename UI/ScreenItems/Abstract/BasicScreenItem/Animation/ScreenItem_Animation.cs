using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class ScreenItem
    {
        private List<Animation> _animations = new List<Animation>();

        public void AddAnimation(Animation animation)
        {
            _animations.Add(animation);
        }

        protected void _updateAnimations()
        {
            _animations.ForEach(a => a.Do(this));
        }
    }
}
