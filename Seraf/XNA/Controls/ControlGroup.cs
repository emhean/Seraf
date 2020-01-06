using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Seraf.XNA.Controls
{
    public class ControlGroup<T> : Control where T : Control
    {
        public List<T> Controls { get; set; }

        public ControlGroup()
        {
            Controls = new List<T>();
        }

        public override void Update(float delta, MouseState mouseState)
        {
            base.Update(delta, mouseState);

            for (int i = 0; i < Controls.Count; ++i)
                Controls[i].Update(delta, mouseState);
        }
    }
}
