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

        public TemplateAppearanceEditTab(IHasDrawLayer parent, CubeTemplate baseTemplate) : base(parent, BuiltInMenuSprites.LargeMenuRectangle)
        {
            var singleSpriteSelectorTab = new TemplateSpriteSelectorTab(this, BuiltInTileSprites.EyeSprites);
            singleSpriteSelectorTab.SetLocationConfig(20, 5, CoordinateMode.ParentPixelOffset, false);
            AddChild(singleSpriteSelectorTab);

            var eyeSelectorTab = new TemplateSpriteSelectorTab(this,BuiltInTileSprites.EyeSprites);
            eyeSelectorTab.SetLocationConfig(20, 5, CoordinateMode.ParentPixelOffset,false);
            AddChild(eyeSelectorTab);

            var bodySelectorTab = new TemplateSpriteSelectorTab(this, BuiltInTileSprites.BodySprites);
            bodySelectorTab.SetLocationConfig(20, 5, CoordinateMode.ParentPixelOffset, false);
            AddChild(bodySelectorTab);

            var appearanceEditTabs = new TabArrayMenuItem(this, MenuOrientation.Vertical, -1, BuiltInMenuSprites.AppearanceEditTab);
            appearanceEditTabs.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            appearanceEditTabs.AddTabButton("I", new ContainerMenuItem(this));//Individual
            appearanceEditTabs.AddTabButton("E", eyeSelectorTab);
            appearanceEditTabs.AddTabButton("B", bodySelectorTab);
            appearanceEditTabs.AddTabButton("C", new ContainerMenuItem(this));//Color
            appearanceEditTabs.AddTabButton("L", new ContainerMenuItem(this));//Load
            appearanceEditTabs.SwitchToFirstTab();
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
