using Borealis.Graphics.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borealis.Graphics.EventArguments
{
    public class ClickEventArgs
    {
        public InputManager Input { get; set; }
        public Buttons Button { get; set; }
    }
}
