using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateBox : CubeSpriteBox
    {
        private TemplateVersionDictionary _template;
        private TextBoxMenuItem _templateHoverBox;

        public TemplateBox(IHasDrawLayer parentDrawLayer,Action<TemplateVersionDictionary> selectTemplate) : base(parentDrawLayer)
        {
            OnMouseReleased += (i) => selectTemplate(_template);
            OnMouseStartHover += (i) => TemplateBox_OnMouseStartHover();
            OnMouseEndHover += (i) => TemplateBox_OnMouseEndHover();

            HighlightedSpriteName = "TemplateItemContainerHighlight";

            _templateHoverBox = new TextBoxMenuItem(this,"null");
            _templateHoverBox.UpdateDrawLayerCascade(DrawLayers.MenuHoverLayer);
            _templateHoverBox.Visible = false;

            AddChild(_templateHoverBox);
        }

        public void SetTemplate(TemplateVersionDictionary template)
        {
            _template = template;
            _templateHoverBox.SetText(_template.Name);
            _sprite.SpriteName = template.Main.Sprite;
        }
        private void TemplateBox_OnMouseStartHover()
        {
            if(_template!=null)
            {
                _templateHoverBox.Visible = true;
            }
        }
        private void TemplateBox_OnMouseEndHover() => _templateHoverBox.Visible = false;


        public override void Update(UserInput input)
        {
            base.Update(input);

            if (MouseHovering)
            {
                _templateHoverBox.SetLocationConfig(new IntPoint(input.MouseX, input.MouseY), CoordinateMode.Absolute, false);
                _templateHoverBox.UpdateDimensionsCascade(IntPoint.Zero, IntPoint.Zero);
            }
        }
    }
}
