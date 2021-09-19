using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateAppearanceEditTab : SpriteScreenItem
    {
        private AppearanceEditSpriteBox _spriteBox;

        public TemplateAppearanceEditTab(IHasDrawLayer parent, CubeTemplate baseTemplate) : base(parent, MenuSprites.LargeMenuRectangle)
        {
            var singleSpriteSelectorTab = new TemplateSpriteSelectorTab(this, Tilesets.CanBeFullBodySprites, SpriteSelected);
            singleSpriteSelectorTab.SetLocationConfig(20, 5, CoordinateMode.ParentPixel, false);
            AddChild(singleSpriteSelectorTab);

            var appearanceEditTabs = new TabArrayMenuItem(this, MenuOrientation.Vertical, -1, MenuSprites.AppearanceEditTab);
            appearanceEditTabs.SetLocationConfig(0, 0, CoordinateMode.ParentPixel, false);
            appearanceEditTabs.AddTabButton("I", singleSpriteSelectorTab);//Individual
            appearanceEditTabs.AddTabButton("E", new ContainerMenuItem(this));
            appearanceEditTabs.AddTabButton("B", new ContainerMenuItem(this));
            appearanceEditTabs.AddTabButton("C", new ContainerMenuItem(this));//Color
            appearanceEditTabs.AddTabButton("L", new ContainerMenuItem(this));//Load
            appearanceEditTabs.SwitchToFirstTab();
            AddChild(appearanceEditTabs);

            _spriteBox = new AppearanceEditSpriteBox(this);
            _spriteBox.SetLocationConfig(85, 20, CoordinateMode.ParentPercentage, true);
            _spriteBox.MultiplyScale(2.0f);
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
            var (spriteName, spriteType) = GenerateSpriteData();

            template.Sprite = spriteName;
            template.SpriteType = spriteType;
        }

        public (string,CubeSpriteDataType) GenerateSpriteData() => _spriteBox.GenerateSpriteDataForTemplate();

    }
}
