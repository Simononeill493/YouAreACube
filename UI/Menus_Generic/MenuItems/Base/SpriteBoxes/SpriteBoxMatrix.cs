using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class SpriteBoxMatrix<TBox> : ContainerMenuItem where TBox : CubeSpriteBox
    {
        protected const int DefaultItemsIncrement = 3;
        //protected const int DefaultItemsIncrement = 25;

        public int Capacity => _width * _height;
        public int XPadding;
        public int YPadding;

        private List<TBox> _boxes;

        private int _width;
        private int _height;
        private int _increment;

        public SpriteBoxMatrix(IHasDrawLayer parent,int width,int height, int increment = DefaultItemsIncrement) : base(parent)
        {
            _boxes = new List<TBox>();
            _width = width;
            _height = height;
            _increment = increment + SpriteManager.GetSpriteSize(BuiltInMenuSprites.SpriteBox).X;
        }

        public void AddBoxes(List<TBox> boxes)
        {
            AddChildren(boxes);
            _boxes.AddRange(boxes);
        }

        public void UpdateBoxLocations()
        {
            var xOffset = XPadding;
            var yOffset = YPadding;

            for (int i = 0; i < _boxes.Count; i++)
            {
                _boxes[i].SetLocationConfig(new IntPoint(xOffset, yOffset), CoordinateMode.ParentPixelOffset, centered: false);
                xOffset += _increment;

                if ((i + 1) % _width == 0)
                {
                    xOffset = XPadding;
                    yOffset += _increment;
                }
            }
        }

        public void PadToCapacity(Func<TBox> generator)
        {
            var amountToAdd = Capacity - _boxes.Count();
            var toAdd = new List<TBox>();
            for(int i=0;i< amountToAdd; i++)
            {
                toAdd.Add(generator());
            }

            AddBoxes(toAdd);
            UpdateBoxLocations();
        }
    }
}
