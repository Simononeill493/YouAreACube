using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class AnimatedScreenItem<TAnimationType> : ScreenItem where TAnimationType : AnimatedScreenItem<TAnimationType>
    {
        private List<Animation<TAnimationType>> _animations;

        public AnimatedScreenItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer)
        {
            _animations = new List<Animation<TAnimationType>>();
        }

        public void AddAnimation(Animation<TAnimationType> animation)
        {
            _animations.Add(animation);
        }

        protected void _updateAnimations()
        {
            _animations.ForEach(a => a.Do((TAnimationType)this));
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            _updateAnimations();
            base._drawSelf(drawingInterface);
        }
    }

    public abstract class Animation<T> where T : ScreenItem
    {
        protected AnimationTrigger _trigger;

        public Animation(AnimationTrigger trigger)
        {
            _trigger = trigger;
        }

        public void Do(T item)
        {
            if(_trigger.Tick())
            {
                _do(item);
            }
        }
        protected abstract void _do(T item);
    }

    public interface AnimationTrigger
    {
        bool Tick();
    }

    public static class Triggers
    {
        public static RandomAnimationTrigger Random(int odds) => new RandomAnimationTrigger(odds);
        public static CyclicAnimationTrigger Cyclic(int cycleLength) => new CyclicAnimationTrigger(cycleLength);
        public static VaryingCyclicAnimationTrigger CyclicVaried(int lowerbound,int upperBound) => new VaryingCyclicAnimationTrigger(lowerbound,upperBound);
        public static ConstantAnimationTrigger Constant = new ConstantAnimationTrigger();
    }


    public class ConstantAnimationTrigger : AnimationTrigger
    {
        public bool Tick() => true;
    }

    public class RandomAnimationTrigger : AnimationTrigger
    {
        private int _odds;
        public RandomAnimationTrigger(int odds)
        {
            _odds = odds;
        }

        public bool Tick()
        {
            return RandomUtils.RandomNumber(_odds) == 0;
        }
    }

    public class CyclicAnimationTrigger : AnimationTrigger
    {
        private int _cycleLength;
        private int _cyclePosition;

        public CyclicAnimationTrigger(int cycleLength)
        {
            _cycleLength = cycleLength;
        }

        public bool Tick()
        { 
            if(_cyclePosition++>=_cycleLength)
            {
                _cyclePosition = 0;
                return true;
            }

            return false;
        }
    }

    public class VaryingCyclicAnimationTrigger : AnimationTrigger
    {
        private int _cycleLengthLower;
        private int _cycleLengthUpper;
        private int _cycleLengthCurent;

        private int _cyclePosition;

        public VaryingCyclicAnimationTrigger(int cycleLengthLower,int cycleLengthUpper)
        {
            _cycleLengthLower = cycleLengthLower;
            _cycleLengthUpper = cycleLengthUpper;
            _cycleLengthCurent = _getNewCycle();
        }

        public bool Tick()
        {
            if (_cyclePosition++ >= _cycleLengthCurent)
            {
                _cyclePosition = 0;
                _cycleLengthCurent = _getNewCycle();
                return true;
            }

            return false;
        }

        private int _getNewCycle()
        {
            return RandomUtils.RandomNumber(_cycleLengthLower, _cycleLengthUpper);
        }
    }



    public class MovementAnimation<T> : Animation<T> where T : ScreenItem
    {
        private IntPoint _offset;

        public MovementAnimation(AnimationTrigger trigger,IntPoint offset) : base(trigger)
        {
            _offset = offset;
        }

        protected override void _do(T item)
        {
            item.OffsetLocationConfig(_offset);
        }
    }

    public class FlipHorizAnimation : Animation<SpriteScreenItem>
    {

        public FlipHorizAnimation(AnimationTrigger trigger) : base(trigger)
        {
        }

        protected override void _do(SpriteScreenItem item)
        {
            item.FlipHorizontal = !item.FlipHorizontal;
        }
    }

    public class SpriteCycleAnimation : Animation<SpriteScreenItem>
    {
        private List<string> _sprites;
        private int _currentIndex;

        public SpriteCycleAnimation(AnimationTrigger trigger, List<string> sprites): base(trigger)
        {
            _sprites = sprites;
        }

        protected override void _do(SpriteScreenItem item)
        {
            var nextIndex = (_currentIndex + 1) >= _sprites.Count ? 0 : _currentIndex + 1;
            item.SpriteName = _sprites[nextIndex];

            _currentIndex = nextIndex;
        }
    }

}
