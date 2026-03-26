namespace Logos.Input
{
    public interface IInputMapper<in TInputDevice> where TInputDevice : IInputDevice
    {
        void Connect(TInputDevice device);

        void Disconnect(TInputDevice device);
    }
}
