namespace Logos.Input
{
    public readonly record struct InputEventArgs
    {
        public InputEventArgs(IInputDevice source, long timestamp)
        {
            Source = source;
            Timestamp = timestamp;
        }

        public IInputDevice Source { get; }

        public long Timestamp { get; }
    }
}
