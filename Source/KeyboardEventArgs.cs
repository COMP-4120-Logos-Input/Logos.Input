namespace Logos.Input
{
    public readonly record struct KeyboardEventArgs
    {
        public KeyboardEventArgs(IKeyboardDevice source, long timestamp, KeyCode key, bool isRepeat)
        {
            Source = source;
            Timestamp = timestamp;
            Key = key;
            IsRepeat = isRepeat;
        }

        public IKeyboardDevice Source { get; }

        public long Timestamp { get; }

        public KeyCode Key { get; }

        public bool IsRepeat { get; }
    }
}
