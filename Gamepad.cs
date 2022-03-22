
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Nefarius.ViGEm.Client;


namespace AutomaticGamepad
{
    public abstract class Gamepad : IDisposable
    {
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);


        public IntPtr Handle { set; get; }
        public bool ConnectState { protected set; get; }

        protected abstract IVirtualGamepad Internal_Gamepad { set; get; }
        protected ViGEmClient Client { set; get; }


        public Gamepad()
        {
            Client = new ViGEmClient();

            Console.WriteLine("Gamepad New");
        }

        public virtual void Dispose()
        {
            if (ConnectState)
                Disconnect();
            Internal_Gamepad = null;

            Client?.Dispose();
            Client = null;

            Console.WriteLine("Gamepad Dispose");
        }

        public virtual void Connect()
        {
            if (ConnectState)
                return;

            ConnectState = true;
            Internal_Gamepad.Connect();

            Console.WriteLine("Connect Gamepad");
        }
        public virtual void Disconnect()
        {
            if (!ConnectState)
                return;

            ConnectState = false;
            Internal_Gamepad.Disconnect();

            Console.WriteLine("Disconnect Gamepad");
        }

        public virtual void SetForeground()
        {
            if (Handle != IntPtr.Zero)
            {
                SetForegroundWindow(Handle);
                Thread.Sleep(1000);
            }
        }

        public virtual void Sleep(double milliseconds)
        {
            Thread.Sleep((int)milliseconds);
        }

        public abstract void Button(string name, double duration = 200);
        public abstract void Trigger(string name, double value, double duration = 200);
        public abstract void Axis(string name, double value, double duration = 200);
        public abstract void Axis2(string name1, string name2, double value1, double value2, double duration = 200);

    }
}
