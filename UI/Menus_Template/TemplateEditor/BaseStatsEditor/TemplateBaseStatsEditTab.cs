using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateBaseStatsEditTab :SpriteScreenItem
    {
        public string CurrentName => name;

        private string name;
        private string health;
        private string energy;
        private string speed;
        private bool active;

        public TemplateBaseStatsEditTab(IHasDrawLayer parent,CubeTemplate baseTemplate) : base(parent, MenuSprites.LargeMenuRectangle)
        {
            _addStaticTextItem("Name:", 20, 10, CoordinateMode.ParentPercentage, true);
            _addStaticTextItem("Health:", 20, 25, CoordinateMode.ParentPercentage, true);
            _addStaticTextItem("Energy:", 20, 40, CoordinateMode.ParentPercentage, true);
            _addStaticTextItem("Speed:", 20, 55, CoordinateMode.ParentPercentage, true);
            _addStaticTextItem("Active:", 20, 80, CoordinateMode.ParentPercentage, true);

            _addTextBox(() => name, (s) => { name = s; }, 60, 10, CoordinateMode.ParentPercentage, true, editable: true, maxTextLength: 15); ;
            _addTextBox(() => health, (s) => { health = s; }, 60, 25, CoordinateMode.ParentPercentage, true, editable: true, maxTextLength: 4);
            _addTextBox(() => energy, (s) => { energy = s; }, 60, 40, CoordinateMode.ParentPercentage, true, editable: true, maxTextLength: 4);
            _addTextBox(() => speed, (s) => { speed = s; }, 60, 55, CoordinateMode.ParentPercentage, true, editable: true, maxTextLength: 6);
            _addItem(new CheckBoxMenuItem(this, () => active, (b) => active = b), 35, 80, CoordinateMode.ParentPercentage, true);

            LoadFieldsForEditing(baseTemplate);
        }

        public void LoadFieldsForEditing(CubeTemplate template)
        {
            name = template.Name;
            health = template.MaxHealth.ToString();
            energy = template.MaxEnergy.ToString();
            speed = template.Speed.ToString();
            active = template.Active;
        }

        public void AddEditedFieldsToTemplate(CubeTemplate template)
        {
            template.Name = name;
            template.MaxHealth = int.Parse(health);
            template.MaxEnergy = int.Parse(energy);
            template.Speed = int.Parse(speed);
            template.Active = active;
        }
    }
}
