namespace Logos.Input
{
    public readonly record struct KeyboardEventArgs
    {
        public KeyboardEventArgs(KeyCode key, bool isRepeat, long timestamp)
        {
            Key = key;
            IsRepeat = isRepeat;
            Timestamp = timestamp;
        }

        public KeyCode Key { get; }

        public bool IsRepeat { get; }

        public long Timestamp { get; }
    }
}
