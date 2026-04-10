using Logos.Input;
using Logos.Input.Sdl3;

using var window = new SdlWindow("Input Test", 1280, 720);
using var provider = new SdlInputProvider();

provider.RegisterWindow(window);

var keyboard = provider.GetListener<IKeyboardListener>();
var mouse    = provider.GetListener<IMouseListener>();

foreach (var keyboardDevice in mouse.Devices)
{
    Console.WriteLine("Connected Device: " + keyboardDevice);
}

keyboard.KeyPressed  += (_, e) => Console.WriteLine($"Key down:  {e.Key}");
keyboard.KeyReleased += (_, e) => Console.WriteLine($"Key up:    {e.Key}");
mouse.ButtonPressed  += (_, e) => Console.WriteLine($"Mouse btn: {e.Button} at {e.Device.Position}");

while (!window.IsCloseRequested)
{
    provider.DispatchEvents();
}

provider.UnregisterWindow(window);