using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class KeyEntryDialog : DialogBoxMenuItem
    {
        BlockInputModel _model;

        public KeyEntryDialog(IHasDrawLayer parentDrawLayer, ScreenItem container, BlockInputModel model) : base(parentDrawLayer, container, MenuSprites.SmallMenuRectangle)
        {
            _model = model;

            var text = _addTextItem(() => "Type the key to record.", 50, 50, CoordinateMode.ParentPercentage, true);
            text.MultiplyScale(0.5f);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if(input.KeysJustPressed.Any())
            {
                CompleteSelection(input.KeysJustPressed.First());
            }
        }

        public void CompleteSelection(Keys key)
        {
            var option = BlockInputOption.CreateValue(key);
            _model.InputOption = option;
            Close();
        }
    }
}
