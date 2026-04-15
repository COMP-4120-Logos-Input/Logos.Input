using System.Numerics;
using Logos.Input;
using Logos.Input.Sdl3;

// A control that tracks whether the fire button is held using a bool as the state
public sealed class FireControl : KeyControl<bool>
{
    public FireControl() : base() { }

    public override void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        State = true;  // fires StateChanged with true
    }

    public override void OnKeyRepeated(object? sender, KeyEventArgs e)
    {
    }

    public override void OnKeyReleased(object? sender, KeyEventArgs e)
    {
        State = false;  // fires StateChanged with false
    }
}

// A control that tracks cursor position as a Vector2
public sealed class CursorControl : MouseMotionControl<Vector2>
{
    public CursorControl() : base() { }

    public override void OnMouseMoved(object? sender, MouseMotionEventArgs e)
    {
        State = e.Device.Position;  // fires StateChanged with new position
    }
}

public class Program
{
    public static void Main()
    {
        using SdlWindow window = new SdlWindow("My Game", 1280, 720);
        using SdlInputProvider provider = new SdlInputProvider();
        provider.RegisterWindow(window);

        IKeyboardListener keyListener = provider.GetListener<IKeyboardListener>();
        IMouseListener mouseListener = provider.GetListener<IMouseListener>();
        
        // Create mappers
        KeyboardMapper keyMapper = new KeyboardMapper(keyListener);
        MouseMapper mouseMapper = new MouseMapper(mouseListener);

        // Create controls (from the previous examples)
        FireControl fireControl = new FireControl();
        CursorControl cursorControl = new CursorControl();

        // React to state changes
        fireControl.StateChanged += (_, isFiring) =>
        {
            if (isFiring)
                Console.WriteLine("Started firing");
            else
                Console.WriteLine("Stopped firing");
        };

        cursorControl.StateChanged += (_, position) =>
        {
            Console.WriteLine($"Cursor at {position}");
        };

        // Bind controls to gestures
        keyMapper.Bind(new KeyGesture(KeyCode.Space, KeyAction.Press), fireControl);
        keyMapper.Bind(new KeyGesture(KeyCode.Space, KeyAction.Release), fireControl);
        mouseMapper.Bind(MouseMotionDirection.Any, cursorControl);

        // Route events from the provider through the mappers
        keyMapper.IsEnabled = true;
        mouseMapper.IsEnabled = true;

        while (!window.IsCloseRequested)
        {
            provider.DispatchEvents();
        }

        provider.UnregisterWindow(window);
    }
}