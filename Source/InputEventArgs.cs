namespace Logos.Input
{
    public readonly record struct InputEventArgs
    {
        public InputEventArgs(IInputDevice device, long timestamp)
        {
            Device = device;
            Timestamp = timestamp;
        }

        public IInputDevice Device { get; }

        public long Timestamp { get; }
    }
}
