using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateItem : SpriteMenuItem
    {
        private BlockTemplate _contents;

        private int _mouseX;
        private int _mouseY;

        public TemplateItem(BlockTemplate contents) : base("TemplateItemContainer")
        {
            _contents = contents;
            _layer = DrawLayers.MenuContentsBackLayer;
            HighlightedSpriteName = "TemplateItemContainerHighlight";
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            _mouseX = input.MouseX;
            _mouseY = input.MouseY;
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);

            drawingInterface.DrawSprite(_contents.Sprite, Location.X + (3 * Scale), Location.Y + (3 * Scale), Scale, DrawLayers.MenuContentsFrontLayer);

            if(_mouseHovering)
            {
                drawingInterface.DrawSprite("EmptyMenuRectangleMedium", _mouseX, _mouseY, Scale, DrawLayers.MenuHoverLayer1);
                drawingInterface.DrawText(_contents.Name, _mouseX+(5*Scale), _mouseY+(3 * Scale), Scale-3, DrawLayers.MenuHoverLayer2);

            }
        }
    }
}
