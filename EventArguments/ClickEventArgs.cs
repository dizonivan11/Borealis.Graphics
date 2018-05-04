using Borealis.Graphics.Input;

namespace Borealis.Graphics.EventArguments
{
    public class ClickEventArgs
    {
        public InputManager Input { get; set; }
        public MouseButtons Button { get; set; }
    }
}
