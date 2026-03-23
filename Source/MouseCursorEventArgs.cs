using System.Numerics;

namespace Logos.Input
{
    public readonly record struct MouseCursorEventArgs
    {
        public MouseCursorEventArgs(Vector2 position, long timestamp)
        {
            Position = position;
            Timestamp = timestamp;
        }

        public Vector2 Position { get; }

        public long Timestamp { get; }
    }
}
