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
            var singleSpriteSelectorTab = new TemplateSpriteSelectorTab(this, BuiltInTileSprites.CanBeFullBodySprites, SpriteSelected);
            singleSpriteSelectorTab.SetLocationConfig(20, 5, CoordinateMode.ParentPixelOffset, false);
            AddChild(singleSpriteSelectorTab);

            var appearanceEditTabs = new TabArrayMenuItem(this, MenuOrientation.Vertical, -1, BuiltInMenuSprites.AppearanceEditTab);
            appearanceEditTabs.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset, false);
            appearanceEditTabs.AddTabButton("I", singleSpriteSelectorTab);//Individual
            appearanceEditTabs.AddTabButton("E", new ContainerMenuItem(this));
            appearanceEditTabs.AddTabButton("B", new ContainerMenuItem(this));
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

        public void SpriteSelected(string sprite, CubeSpriteDataType spriteType) => _spriteBox.SetSpriteToSingleSpriteType(sprite, spriteType);

        public void LoadAppearanceForEditing(CubeTemplate template)
        {
            _spriteBox.SetSpriteToSingleSpriteType(template.Sprite,template.SpriteType);
        }

        public void AddEditedAppearanceToTemplate(CubeTemplate template)
        {
            var (spriteName, spriteType) = _spriteBox.GenerateSpriteDataForTemplate();

            template.Sprite = spriteName;
            template.SpriteType = spriteType;
        }
    }
}
