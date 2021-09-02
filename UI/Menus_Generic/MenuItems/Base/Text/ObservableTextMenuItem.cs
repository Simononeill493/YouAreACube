using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ObservableTextMenuItem : TextMenuItem
    {
        public override string Text { get { return TextProvider(); } set { ObservableTextChanged?.Invoke(value); } }
        public Func<string> TextProvider;

        public event Action<string> ObservableTextChanged;

        public ObservableTextMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) 
        {

        }
    }
}
