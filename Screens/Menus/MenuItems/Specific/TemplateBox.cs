using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateBox : SpriteMenuItem
    {
        public BlockTemplate Template;

        private int _mouseX;
        private int _mouseY;

        public TemplateBox(Action<BlockTemplate> templateClick) : base("TemplateItemContainer")
        {
            OnClick += () => templateClick(Template);

            DrawLayer = DrawLayers.MenuContentsBackLayer;
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

            if(Template != null)
            {
                _drawTemplateData(drawingInterface);
            }
        }

        private void _drawTemplateData(DrawingInterface drawingInterface)
        {
            drawingInterface.DrawSprite(Template.Sprite, Location.X + (3 * Scale), Location.Y + (3 * Scale), Scale, DrawLayers.MenuContentsFrontLayer);

            if (_mouseHovering)
            {
                drawingInterface.DrawSprite("EmptyMenuRectangleMedium", _mouseX, _mouseY, Scale, DrawLayers.MenuHoverLayer1);
                drawingInterface.DrawText(Template.Name, _mouseX + (5 * Scale), _mouseY + (3 * Scale), Scale - 3, DrawLayers.MenuHoverLayer2);
            }
        }
    }
}
