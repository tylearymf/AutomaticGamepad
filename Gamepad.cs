using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.DualShock4;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace AutomaticGamepad
{
    public abstract class Gamepad : IDisposable
    {
        public const string Name = "gamepad";

        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);

        public static Gamepad Instance { private set; get; }


        public IntPtr Handle { set; get; }
        public bool AbortThread { set; get; }

        public abstract GamepadType GamepadType { get; }
        public abstract string BindApplicationName { get; }
        public abstract string PictureName { get; }
        protected abstract IVirtualGamepad Internal_Gamepad { set; get; }
        protected abstract Dictionary<string, GamepadProperty> GamepadPropertyDic { get; }

        protected ViGEmClient Client { set; get; }


        public Gamepad()
        {
            Instance = this;
            Client = new ViGEmClient();
            Console.WriteLine("Gamepad New");
        }

        public void Dispose()
        {
            OnDispose();

            // 不要调Disconnect，会导致驱动释放异常
            // 正确的释放会在下面 Client.Dipose 中处理
            //Internal_Gamepad?.Disconnect();
            Internal_Gamepad = null;

            Client?.Dispose();
            Client = null;
            Instance = null;

            Console.WriteLine("Gamepad Dispose");
        }

        public void Connect()
        {
            try
            {
                Internal_Gamepad.Connect();

                Console.WriteLine("Connect Gamepad");

                Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                throw new Exception("虚拟手柄连接错误！\n" + ex);
            }
        }

        public virtual void OnDispose()
        {
        }

        public virtual void SetForeground(double milliseconds)
        {
            if (Handle != IntPtr.Zero)
            {
                SetForegroundWindow(Handle);
                Thread.Sleep((int)milliseconds);
            }
        }

        #region JS Interface

        public void sleep(double milliseconds) { SetSleep(milliseconds); }

        public void button(string name, double duration = 200) { SetButton(name, duration); }
        public void dpad(string name, double duration = 200) { SetDPad(name, duration); }
        public void trigger(string name, double value, double duration = 200) { SetTrigger(name, value, duration); }
        public void axis(string name, double value, double duration = 200) { SetAxis(name, value, duration); }
        public void axis2(string name1, string name2, double value1, double value2, double duration = 200) { SetAxis2(name1, name2, value1, value2, duration); }

        public void buttonstate(string name, bool state) { SetButtonState(name, state); }
        public void dpadstate(string name, bool state) { SetDPadState(name, state); }
        public void triggerstate(string name, double value) { SetTriggerState(name, value); }
        public void axisstate(string name, double value) { SetAxisState(name, value); }
        public void axis2state(string name1, string name2, double value1, double value2) { SetAxis2State(name1, name2, value1, value2); }

        #endregion


        public virtual void SetSleep(double milliseconds)
        {
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (AbortThread || stopwatch.ElapsedMilliseconds >= milliseconds)
                    break;

                Thread.Sleep(1);
            }
        }

        public virtual void CallMethod(params string[] args)
        {
            SetForeground(200);

            var argLength = args?.Length ?? 0;
            if (argLength == 0)
                return;

            var arg1 = args[0];
            var arg2 = 0D;
            var arg3 = 0D;

            if (!GamepadPropertyDic.TryGetValue(arg1, out var property))
                return;

            if (argLength >= 2)
                double.TryParse(args[1], out arg2);
            if (argLength >= 3)
                double.TryParse(args[2], out arg3);

            var enabledDelay = false;
            switch (argLength)
            {
                case 1:
                    if (property is GamepadButton button)
                        SetButton(button, enabledDelay: enabledDelay);
                    else if (property is GamepadDPad dPad)
                        SetDPad(dPad, enabledDelay: enabledDelay);
                    break;
                case 3:
                    if (property is GamepadTrigger slider)
                        SetTrigger(slider, arg2, arg3, enabledDelay);
                    else if (property is GamepadAxis axis)
                        SetAxis(axis, arg2, arg3, enabledDelay);
                    break;
                default:
                    throw new Exception("未实现 CallMethod 接口");
            }
        }

        public T GetProperty<T>(string name) where T : GamepadProperty
        {
            GamepadPropertyDic.TryGetValue(name, out var property);
            return property as T;
        }

        protected void SetButton(GamepadButton button, double duration = 200, bool enabledDelay = true)
        {
            if (button == null)
                return;

            SetButtonState(button, true);
            SetSleep(duration);
            SetButtonState(button, false);

            if (enabledDelay)
                SetSleep(200);
        }

        protected void SetDPad(GamepadDPad dPad, double duration = 200, bool enabledDelay = true)
        {
            if (dPad == null)
                return;

            SetDPadState(dPad, true);
            SetSleep(duration);
            SetDPadState(dPad, false);

            if (enabledDelay)
                SetSleep(200);
        }

        protected void SetTrigger(GamepadTrigger trigger, double value, double duration = 200, bool enabledDelay = true)
        {
            if (trigger == null)
                return;

            SetTriggerState(trigger, value);
            SetSleep(duration);
            SetTriggerState(trigger, 0);

            if (enabledDelay)
                SetSleep(200);
        }

        protected void SetAxis(GamepadAxis axis, double value, double duration = 200, bool enabledDelay = true)
        {
            if (axis == null)
                return;

            SetAxisState(axis, value);
            SetSleep(duration);
            SetAxisState(axis, 0);

            if (enabledDelay)
                SetSleep(200);
        }

        protected void SetAxis2(GamepadAxis axis1, GamepadAxis axis2, double value1, double value2, double duration = 200, bool enabledDelay = true)
        {
            if (axis1 == null || axis2 == null)
                return;

            SetAxis2State(axis1, axis2, value1, value2);
            SetSleep(duration);
            SetAxis2State(axis1, axis2, 0, 0);

            if (enabledDelay)
                SetSleep(200);
        }

        protected void SetButtonState(GamepadButton button, bool state)
        {
            var gamepad = Internal_Gamepad;
            gamepad.AutoSubmitReport = true;
            gamepad.SetButtonState(button, state);
        }

        protected void SetDPadState(GamepadDPad dPad, bool state)
        {
            var gamepad = Internal_Gamepad;
            gamepad.AutoSubmitReport = true;

            if (gamepad is IDualShock4Controller controller)
            {
                if (state)
                    controller.SetDPadDirection((DualShock4DPadDirection)dPad.DualShock4);
                else
                    controller.SetDPadDirection(DualShock4DPadDirection.None);
            }
            else
            {
                gamepad.SetButtonState(dPad, state);
            }
        }

        protected void SetTriggerState(GamepadTrigger trigger, double value)
        {
            var gamepad = Internal_Gamepad;
            gamepad.AutoSubmitReport = true;
            gamepad.SetSliderValue(trigger, ToByte(value));
        }

        protected void SetAxisState(GamepadAxis axis, double value)
        {
            var gamepad = Internal_Gamepad;
            gamepad.AutoSubmitReport = true;

            value = FixedAxisValue(axis, value);
            gamepad.SetAxisValue(axis, ToShort(value));
        }

        protected void SetAxis2State(GamepadAxis axis1, GamepadAxis axis2, double value1, double value2)
        {
            var gamepad = Internal_Gamepad;
            gamepad.AutoSubmitReport = false;

            value1 = FixedAxisValue(axis1, value1);
            value2 = FixedAxisValue(axis2, value2);
            gamepad.SetAxisValue(axis1, ToShort(value1));
            gamepad.SetAxisValue(axis2, ToShort(value2));

            gamepad.SubmitReport();
        }

        protected byte ToByte(double value)
        {
            return (byte)(value * byte.MaxValue);
        }

        protected short ToShort(double value)
        {
            return (short)(value * short.MaxValue);
        }

        protected double FixedAxisValue(GamepadAxis axis, double value)
        {
            if (axis?.DualShock4 == DualShock4Axis.LeftThumbY ||
                axis?.DualShock4 == DualShock4Axis.RightThumbY)
                return value * -1;

            return value;
        }
    }

    public enum GamepadType
    {
        Xbox = 0,
        PlayStation = 1,
    }

    public class GamepadProperty
    {
        public Xbox360Property Xbox360 { protected set; get; }
        public DualShock4Property DualShock4 { protected set; get; }

        public int Index
        {
            get
            {
                if (Xbox360 != null)
                    return Xbox360.Id;
                if (DualShock4 != null)
                    return DualShock4.Id;

                throw new Exception("Gamepad Button 无效");
            }
        }

        public static implicit operator int(GamepadProperty property)
        {
            return property.Index;
        }
    }

    public class GamepadButton : GamepadProperty
    {
        public static implicit operator GamepadButton(Xbox360Button button)
        {
            return new GamepadButton() { Xbox360 = button };
        }

        public static implicit operator GamepadButton(DualShock4Button button)
        {
            return new GamepadButton() { DualShock4 = button };
        }

        public static implicit operator GamepadButton(string name)
        {
            return Gamepad.Instance.GetProperty<GamepadButton>(name);
        }
    }

    public class GamepadDPad : GamepadProperty
    {
        public static implicit operator GamepadDPad(Xbox360Button button)
        {
            return new GamepadDPad() { Xbox360 = button };
        }

        public static implicit operator GamepadDPad(DualShock4DPadDirection button)
        {
            return new GamepadDPad() { DualShock4 = button };
        }

        public static implicit operator GamepadDPad(string name)
        {
            return Gamepad.Instance.GetProperty<GamepadDPad>(name);
        }
    }

    public class GamepadTrigger : GamepadProperty
    {
        public static implicit operator GamepadTrigger(Xbox360Slider slider)
        {
            return new GamepadTrigger() { Xbox360 = slider };
        }

        public static implicit operator GamepadTrigger(DualShock4Slider slider)
        {
            return new GamepadTrigger() { DualShock4 = slider };
        }

        public static implicit operator GamepadTrigger(string name)
        {
            return Gamepad.Instance.GetProperty<GamepadTrigger>(name);
        }
    }

    public class GamepadAxis : GamepadProperty
    {
        public static implicit operator GamepadAxis(Xbox360Axis button)
        {
            return new GamepadAxis() { Xbox360 = button };
        }

        public static implicit operator GamepadAxis(DualShock4Axis button)
        {
            return new GamepadAxis() { DualShock4 = button };
        }

        public static implicit operator GamepadAxis(string name)
        {
            return Gamepad.Instance.GetProperty<GamepadAxis>(name);
        }
    }
}
