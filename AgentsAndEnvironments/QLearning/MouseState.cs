using SharedLibrary.States;
using System.Diagnostics;
using Point = Microsoft.Xna.Framework.Point;

namespace QLearning
{
    [DebuggerDisplay("({Position.X},{Position.Y})")]
    public class MouseState(Point position, MouseState.Types type) : IState
    {
        public enum Types
        {
            Unknown,
            Empty,
            Fire,
            Cheese
        }
        public Point Position = position;
        public Types Type = type;
    }
}