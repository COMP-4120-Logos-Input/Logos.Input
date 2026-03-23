namespace Logos.Input
{
    public readonly record struct MouseButtonEventArgs
    {
        public MouseButtonEventArgs(MouseButton button, long timestamp)
        {
            Button = button;
            Timestamp = timestamp;
        }

        public MouseButton Button { get; }

        public long Timestamp { get; }
    }
}
