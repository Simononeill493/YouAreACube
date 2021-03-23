﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateBox : SpriteMenuItem
    {
        private BlockTemplate _template;
        private TextBoxMenuItem _templateHoverBox;
        private SpriteMenuItem _sprite;

        public TemplateBox(IHasDrawLayer parentDrawLayer,Action<BlockTemplate> selectTemplate) : base(parentDrawLayer,"TemplateItemContainer")
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

        public void SetTemplate(BlockTemplate template)
        {
            _template = template;
            _templateHoverBox.SetText(_template.Name);

            if (_sprite != null)
            {
                RemoveChild(_sprite);
            }

            _sprite = new SpriteMenuItem(this, template.Sprite);
            _sprite.SetLocationConfig(14, 14, CoordinateMode.ParentPercentageOffset);

            AddChild(_sprite);
        }
        private void TemplateBox_OnMouseStartHover()
        {
            if(_template!=null)
            {
                _templateHoverBox.Visible = true;
            }
        }
        private void TemplateBox_OnMouseEndHover()
        {
            _templateHoverBox.Visible = false;
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if (MouseHovering)
            {
                _templateHoverBox.SetLocationConfig(new Point(input.MouseX, input.MouseY), CoordinateMode.Absolute, false);
                _templateHoverBox.UpdateDimensionsCascade(Point.Zero, Point.Zero);
            }
        }
    }
}