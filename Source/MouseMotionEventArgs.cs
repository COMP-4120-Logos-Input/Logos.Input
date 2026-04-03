using System.Numerics;

namespace Logos.Input
{
    public readonly record struct MouseMotionEventArgs
    {
        public MouseMotionEventArgs(Vector2 position, long timestamp)
        {
            Position = position;
            Timestamp = timestamp;
        }

        public Vector2 Position { get; }

        public long Timestamp { get; }
    }
}
