using System.Numerics;

namespace Logos.Input
{
    public readonly record struct MouseWheelEventArgs
    {
        public MouseWheelEventArgs(Vector2 rotation, long timestamp)
        {
            Rotation = rotation;
            Timestamp = timestamp;
        }

        public Vector2 Rotation { get; }

        public long Timestamp { get; }
    }
}
