# Quick Start

## Important Components

Within Logos.Input, there are three main components.

`IInputProvider` provides connection to the underlying input system, holds a set of listeners, and calls `DispatchEvents()`. You call `DispatchEvents()` once per frame.

`IInputListener` represents a specific category of an input device such as a keyboard or mouse. You get one from the provider by calling `GetListener<T>()`. Listeners allow you to interact with device specific events for the respective device category.

`IInputDevice` represents a single physical device.


The pattern for integrating our library is largely the same regardless of which input backend you are using. It has a general pattern of:

```
1. Create a provider
2. Get a listener from the provider
3. Subscribe to events on the listener
4. Call DispatchEvents() every frame
```
Below is an example of how to integrate our library with an SDL backend:
```csharp
using Logos.Input;
using Logos.Input.Sdl3;

// 1. Create a provider
using SdlInputProvider provider = new SdlInputProvider();

// 2. Get listeners from the provider
IKeyboardListener keyboard = provider.GetListener<IKeyboardListener>();
IMouseListener mouse = provider.GetListener<IMouseListener>();

// 3. Subscribe to events on the listener
keyboard.KeyPressed += (_, e) =>
{
    Console.WriteLine($"Key pressed: {e.Key}");
};

mouse.ButtonPressed += (_, e) =>
{
    Console.WriteLine($"Button pressed: {e.Button} at {e.Device.Position}");
};

// 4. Call DispatchEvents every frame
while (true)
{
    provider.DispatchEvents();
}
```

## Device Events

### Keyboard

You can interact with keyboard events. To do so, get a keyboard listener from the provider:

```csharp
IKeyboardListener keyboard = provider.GetListener<IKeyboardListener>();
```

**Available events:**

| Event Name | Action |
|---|---|
| `KeyPressed` | A key was pressed |
| `KeyRepeated` | A key is held |
| `KeyReleased` | A key was released |
| `DeviceConnected` | A keyboard was plugged in |
| `DeviceDisconnected` | A keyboard was unplugged |

```csharp
keyboard.KeyPressed += (_, e) =>
{
    Console.WriteLine($"Pressed: {e.Key}");
    Console.WriteLine($"Device: {e.Device}");
};

keyboard.KeyRepeated += (_, e) =>
{
    Console.WriteLine($"Repeated: {e.Key}");
};

keyboard.KeyReleased += (_, e) =>
{
    Console.WriteLine($"Released: {e.Key}");
};

keyboard.DeviceConnected += (_, e) =>
{
    Console.WriteLine("Keyboard connected.");
};

keyboard.DeviceDisconnected += (_, e) =>
{
    Console.WriteLine("Keyboard disconnected.");
};
```
More information on what `e` contains is documented in our `KeyEventArgs.cs` file 

---

### Mouse

You may also want to interact with mouse events as well. You can get a mouse listener from the provider:

```csharp
IMouseListener mouse = provider.GetListener<IMouseListener>();
```

**Available events:**

| Event | When it fires |
|---|---|
| `ButtonPressed` | A mouse button was pressed |
| `ButtonReleased` | A mouse button was released |
| `MouseMoved` | The cursor position changed |
| `WheelMoved` | The scroll wheel moved |
| `DeviceConnected` | A mouse was plugged in |
| `DeviceDisconnected` | A mouse was unplugged |

```csharp
mouse.ButtonPressed += (_, e) =>
{
    Console.WriteLine($"Button: {e.Button}");
    Console.WriteLine($"Position: {e.Device.Position}");
};

mouse.ButtonReleased += (_, e) =>
{
    Console.WriteLine($"Released: {e.Button}");
};

mouse.WheelMoved += (_, e) =>
{
    Console.WriteLine($"Wheel: {e.Device.ScrollWheel}");
};
```
More information on what `e` contains is documented in our `MouseButtonEventArgs.cs` file 

---

## Querying Device State

In addition to events, you can also query device state directly. 

```csharp
// Get the listeners first
IKeyboardListener keyboard = provider.GetListener<IKeyboardListener>();
IMouseListener mouse = provider.GetListener<IMouseListener>();

while (true)
{
    provider.DispatchEvents();

    // Queries state for all connected keyboards
    foreach (IKeyboardDevice device in keyboard.ConnectedDevices)
    {
        if (device.IsKeyPressed(KeyCode.W))
            Console.WriteLine("Pressed W");

        if (device.IsKeyPressed(KeyCode.LShift))
            Console.WriteLine("Pressed LShift");
    }

    // Queries state for all connected mice
    foreach (IMouseDevice device in mouse.ConnectedDevices)
    {
        Console.WriteLine($"Cursor: {device.Position}");
        Console.WriteLine($"Scroll: {device.ScrollWheel}");

        if (device.IsButtonPressed(MouseButton.Left))
            Console.WriteLine("Firing");
    }
}
```
More information on what exactly can be queried is documented in our `IKeyboardDevice.cs` and `IMouseDevice.cs` files.

---

## Mappers
Instead of subscribing to raw events like we've been doing, you can bind specific **gestures** to **observers**. The mapper routes the right events to the right place automatically. Mappers take an argument of a listener which can be retrieved by calling the `GetListener<T>` method from a class that implements the `IInputProvider` interface.
Both `KeyboardMapper` and `MouseMapper` implement `IInputMapper`, which exposes `RouteEvents` and `BlockEvents`. You call `RouteEvents` to start receiving input and `BlockEvents` to stop.

### KeyboardMapper
`KeyboardMapper` maps `KeyGesture` values (a key + an action) to `IKeyObserver` implementations.
```csharp
using Logos.Input;
using Logos.Input.Sdl3;

using SdlInputProvider provider = new SdlInputProvider();

IKeyboardListener keyListener = provider.GetListener<IKeyboardListener>();

KeyboardMapper mapper = new KeyboardMapper(IKeyboardListener);

mapper.Bind(new KeyGesture(KeyCode.Space, KeyAction.Press), jumpObserver);
mapper.Bind(new KeyGesture(KeyCode.W, KeyAction.Repeat), moveForwardObserver);

mapper.IsEnabled = true;

while (true) 
{
    provider.DispatchEvents();
}
```
And then, to stop receiving events:
```csharp
mapper.IsEnabled = false;
```

If you want to remove a speciifc binding, use the `Bind()` method:
```csharp
mapper.Unbind(new KeyGesture(KeyCode.W, KeyAction.Repeat));
```

Below are `KeyAction` values that can be used
| Value | When it triggers |
|---|---|
| `Press` | The key was pressed |
| `Repeat` | The key is held |
| `Release` | The key was released |

---

### MouseMapper
`MouseMapper` maps mouse button gestures, motion directions, **and** wheel directions to their respective observers.
```csharp
MouseMapper mapper = new MouseMapper();

// Button gestures
mapper.Bind(new MouseButtonGesture(MouseButton.Left, MouseButtonAction.Press), fireObserver);
mapper.Bind(new MouseButtonGesture(MouseButton.Right, MouseButtonAction.Press), aimObserver);

// Motion direction
mapper.Bind(MouseMotionDirection.Any, cursorObserver);

// Wheel direction
mapper.Bind(MouseWheelDirection.Up, zoomInObserver);
mapper.Bind(MouseWheelDirection.Down, zoomOutObserver);

mapper.IsEnabled = true;
```

And then of course, to remove bindings:
```csharp
mapper.Unbind(new MouseButtonGesture(MouseButton.Left, MouseButtonAction.Press));
mapper.Unbind(MouseWheelDirection.Down);
```

---
## Observers and Controls
An observer is the target that a mapper routes events to. You implement an observer interface and pass it to `Bind`. The library provides three observer interfaces.

| Interface | Methods |
|---|---|
| `IKeyObserver` | `OnKeyPressed`, `OnKeyRepeated`, `OnKeyReleased` |
| `IMouseButtonObserver` | `OnButtonPressed`, `OnButtonReleased` |
| `IMouseMotionObserver` | `OnMouseMoved` |
| `IMouseWheelObserver` | `OnWheelMoved` |

A control is a special way to assign a certain type to the state of an observer. `InputControl<T>` is a base class that holds a typed state value and fires a `StateChanged` event (which the developer may implement themselves) whenever that state changes. Abstract classes such as `KeyControl<T>`, `MouseButtonControl<T>`, `MouseMotionControl<T>`, and `MouseWheelControl<T>` extend this by implementing the respective observer interfaces, `IKeyObserver`, `IMouseButtonObserver`, `IMouseMotionObserver`, and `IMouseWheelObserver` respectively, leaving only the state update logic for the developer to fill in.

### Implementing an Observer
Lets take the simplest case. You implement the observer interface directly when you just need to react to an event
```csharp
public sealed class JumpObserver : IKeyObserver
{
    private readonly Player _player;

    public JumpObserver(Player player)
    {
        _player = player;
    }

    public void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        _player.Jump();
    }

    public void OnKeyRepeated(object? sender, KeyEventArgs e)
    {
    }

    public void OnKeyReleased(object? sender, KeyEventArgs e)
    {
    }
}

mapper.Bind(new KeyGesture(KeyCode.Space, KeyAction.Press), new JumpObserver(player));
```
### Implementing a Control
Use `KeyControl<T>` when you want the observer to expose its current state, notify subscribers when it changes, and also assign a type to the state. In this case, the state is a boolean but if the developer would like, they can have the state of a key represented as a float.

```csharp
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
```

## Putting it Together
Here is a complete example wiring up a mapper, several controls, and reacting to state changes

```csharp
using System.Numerics;
using Logos.Input;
using Logos.Input.Sdl3;

using SdlWindow window = new SdlWindow("My Game", 1280, 720);
using SdlInputProvider provider = new SdlInputProvider();
provider.RegisterWindow(window);

// Get listeners
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
```

Notice that `fireControl` is bound to both `Press` and `Release` gestures. The same control instance handles both because `KeyControl<T>` implements all three `IKeyObserver` methods. The mapper simply calls the right one based on which gesture matched.