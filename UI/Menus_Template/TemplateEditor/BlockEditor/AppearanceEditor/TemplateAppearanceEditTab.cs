using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateAppearanceEditTab : SpriteMenuItem
    {
        private AppearanceEditSpriteBox _spriteBox;

        public TemplateAppearanceEditTab(IHasDrawLayer parent, CubeTemplate baseTemplate) : base(parent, "EmptyMenuRectangleFull")
        {
            var eyeSelectorTab = new TemplateSpriteSelectorTab(this);
            eyeSelectorTab.SetLocationConfig(20, 5, CoordinateMode.ParentPixelOffset,false);
            AddChild(eyeSelectorTab);

            var appearanceEditTabs = new TabArrayMenuItem(this, MenuOrientation.Vertical, -1, "AppearanceEditOptionButton");
            appearanceEditTabs.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            appearanceEditTabs.AddTabButton("E", eyeSelectorTab);
            appearanceEditTabs.AddTabButton("B", new ContainerMenuItem(this));
            appearanceEditTabs.AddTabButton("C", new ContainerMenuItem(this));
            appearanceEditTabs.AddTabButton("D", new ContainerMenuItem(this));
            appearanceEditTabs.AddTabButton("L", new ContainerMenuItem(this));
            AddChild(appearanceEditTabs);

            _spriteBox = new AppearanceEditSpriteBox(this);
            _spriteBox.SetLocationConfig(85, 20, CoordinateMode.ParentPercentageOffset, true);
            _spriteBox.MultiplyScaleCascade(2.0f);
            AddChild(_spriteBox);

            LoadAppearanceForEditing(baseTemplate);
        }

        public void LoadAppearanceForEditing(CubeTemplate template)
        {
            _spriteBox.SetSpriteToSingleImage(template.Sprite);
        }

        public void AddEditedAppearanceToTemplate(CubeTemplate template)
        {
        }
    }
}
