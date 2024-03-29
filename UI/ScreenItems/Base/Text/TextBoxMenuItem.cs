﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TextBoxMenuItem : SpriteScreenItem
    {
        public event Action<string> OnTextTyped;

        public int MaxTextLength { get; set; } = 9;
        public virtual bool Editable { get; set; }
        public virtual bool Focused { get; set; }

        protected TextScreenItem _textItem;

        public TextBoxMenuItem(IHasDrawLayer parentDrawLayer,Func<string> getText) : base(parentDrawLayer, MenuSprites.BasicTextBox)
        {
            _textItem = new TextScreenItem(this, getText);
            _textItem.OnTextTyped += (s) => OnTextTyped?.Invoke(s);
            _textItem.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, centered: true);
            AddChild(_textItem);
        }
        public TextBoxMenuItem(IHasDrawLayer parentDrawLayer, Func<string> getText, Action<string> setText) : this(parentDrawLayer, getText)
        {
            OnTextTyped += setText;
        } 
        public TextBoxMenuItem(IHasDrawLayer parentDrawLayer, string staticText) : this(parentDrawLayer, () => staticText) { }


        public override void Update(UserInput input)
        {
            base.Update(input);

            if(input.MouseLeftPressed)
            {
                Focused = MouseHovering;
            }

            if(Editable & Focused)
            {
                var edited = _textItem.KeyboardEdit(input,MaxTextLength);
                if(edited)
                {
                    _textItem.UpdateLocation(ActualLocation, this.GetCurrentSize());
                }
            }
        }
    }
}
