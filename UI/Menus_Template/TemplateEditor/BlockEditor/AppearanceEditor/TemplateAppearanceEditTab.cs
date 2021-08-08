using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateAppearanceEditTab : SpriteMenuItem
    {

        public TemplateAppearanceEditTab(IHasDrawLayer parent, CubeTemplate baseTemplate) : base(parent, "EmptyMenuRectangleFull")
        {
            var appearances = new DropdownMenuItem<string>(this);
            appearances.SetLocationConfig(50, 10, CoordinateMode.ParentPercentageOffset, true);

            var validPics = new List<string>();
            foreach(var kvp in SpriteManager.Sprites)
            {
                if(kvp.Value.Width==16 & kvp.Value.Height == 16)
                {
                    validPics.Add(kvp.Key);
                }
            }
            appearances.AddItems(validPics);

            AddChild(appearances);
        }

        public void LoadFieldsForEditing(CubeTemplate template)
        {
        }

        public void AddEditedFieldsToTemplate(CubeTemplate template)
        {
        }
    }
}
