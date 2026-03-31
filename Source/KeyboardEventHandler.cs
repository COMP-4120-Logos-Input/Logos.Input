using System;

namespace Logos.Input
{
    public static class KeyboardEventHandler
    {
        public static EventHandler<KeyboardEventArgs> Create<TState>(Action<TState> action, TState state)
        {
            ArgumentNullException.ThrowIfNull(action);
            return (_, _) => action(state);
        }

        public static EventHandler<KeyboardEventArgs> Create<TState>(Action<TState> action, Func<object?, KeyboardEventArgs, TState> converter)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(converter);
            return (sender, args) => action(converter(sender, args));
        }
    }
}
