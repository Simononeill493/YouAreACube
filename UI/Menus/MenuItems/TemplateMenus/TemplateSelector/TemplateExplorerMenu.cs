﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateExplorerMenu : SpriteMenuItem
    {
        private const int ItemsWidth = 6;
        private const int ItemsHeight = 6;

        private Kernel _kernel;
        private List<TemplateBox> _boxes;
        private TemplateSelectedMenu templateSelectedMenu;

        private Action<BlockTemplate> _goToTemplateEditMenu;

        public TemplateExplorerMenu(IHasDrawLayer parentDrawLayer,Kernel kernel,Action<BlockTemplate> goToTemplateEditMenu) : base(parentDrawLayer,"EmptyMenuRectangleFull") 
        {
            _kernel = kernel;
            _goToTemplateEditMenu = goToTemplateEditMenu;

            _boxes = _generateTemplateBoxes();
            _setTemplateItemsLocations();

            templateSelectedMenu = new TemplateSelectedMenu(this, _templateSelectedAction);
            templateSelectedMenu.SetLocationConfig(65, 0, CoordinateMode.ParentPercentageOffset);
            AddChild(templateSelectedMenu);
        }

        private void _templateSelectedAction(BlockTemplate template, TemplateSelectedAction selectedActionType)
        {
            if(template!=null)
            {
                switch (selectedActionType)
                {
                    case TemplateSelectedAction.Edit:
                        _goToTemplateEditMenu(template);
                        break;
                }
            }
        }

        private void _templateBoxClicked(BlockTemplate template)
        {
            if(template!=null)
            {
                templateSelectedMenu.SetTemplate(template);
            }
        }

        private List<TemplateBox> _generateTemplateBoxes()
        {
            var items = new List<TemplateBox>();
            var numTemplates = _kernel.KnownTemplates.Count();
            for (int i = 0; i < ItemsWidth * ItemsHeight; i++)
            {
                var box = new TemplateBox(this, _templateBoxClicked);
                if(i<numTemplates)
                {
                    box.SetTemplate(_kernel.KnownTemplates[i]);
                }
                items.Add(box);
                AddChild(box);
            }

            return items;
        }

        private void _setTemplateItemsLocations()
        {
            var xPadding = 8;

            var xOffset = xPadding;
            var yOffset = 8;
            var xIncrement = 25;
            var yIncrement = 25;

            for (int i=0;i<_boxes.Count;i++)
            {
                _boxes[i].SetLocationConfig(new Point(xOffset, yOffset), CoordinateMode.ParentPixelOffset, centered: false);
                xOffset += xIncrement;

                if ((i + 1) % ItemsWidth == 0)
                {
                    xOffset = xPadding;
                    yOffset += yIncrement;
                }
            }
        }
    }
}