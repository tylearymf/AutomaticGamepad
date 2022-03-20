
using System.Collections.Generic;
using System.Threading;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;


namespace AutomaticGamepad
{
    public class XboxGamepad : Gamepad
    {
        protected override IVirtualGamepad Internal_Gamepad
        {
            get { return m_Controller; }
            set { m_Controller = (IXbox360Controller)value; }
        }

        IXbox360Controller m_Controller;


        public XboxGamepad()
        {
            m_Controller = Client.CreateXbox360Controller();
        }

        public override void Button(string name, int duration = 200)
        {
            if (XboxControllerNames.s_ButtonDic.TryGetValue(name, out var button))
                SetButton(button, duration);
        }

        public override void Trigger(string name, float value, int duration = 200)
        {
            if (XboxControllerNames.s_TriggerDic.TryGetValue(name, out var trigger))
                SetTrigger(trigger, (byte)(value * byte.MaxValue), duration);
        }

        public override void Axis(string name, float value, int duration = 200)
        {
            if (XboxControllerNames.s_AxisDic.TryGetValue(name, out var axis))
                SetAxis(axis, (short)(value * short.MaxValue), duration);
        }

        public override void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }


        void SetButton(Xbox360Button button, int duration = 200)
        {
            m_Controller.SetButtonState(button, true);
            Thread.Sleep(duration);
            m_Controller.SetButtonState(button, false);
            Thread.Sleep(200);
        }

        void SetTrigger(Xbox360Slider slider, byte value, int duration = 200)
        {
            m_Controller.SetSliderValue(slider, value);
            Thread.Sleep(duration);
            m_Controller.SetSliderValue(slider, 0);
            Thread.Sleep(200);
        }

        void SetAxis(Xbox360Axis axis, short value, int duration = 200)
        {
            m_Controller.SetAxisValue(axis, value);
            Thread.Sleep(duration);
            m_Controller.SetAxisValue(axis, 0);
            Thread.Sleep(200);
        }

        void SetAxis2(Xbox360Axis axis1, Xbox360Axis axis2, short value1, short value2, int duration = 200)
        {
            m_Controller.SetAxisValue(axis1, value1);
            m_Controller.SetAxisValue(axis2, value2);
            Thread.Sleep(duration);
            m_Controller.SetAxisValue(axis1, 0);
            m_Controller.SetAxisValue(axis2, 0);
            Thread.Sleep(200);
        }
    }

    public class XboxControllerNames
    {
        // Button
        public const string Button_A = "a";
        public const string Button_B = "b";
        public const string Button_X = "x";
        public const string Button_Y = "y";

        public const string Button_LB = "lb";
        public const string Button_RB = "rb";

        // 按下左摇杆
        public const string LeftStickButton = "lsb";
        // 按下右摇杆
        public const string RightStickButton = "rsb";

        public const string Dpad_UP = "up";
        public const string Dpad_DOWN = "down";
        public const string Dpad_LEFT = "left";
        public const string Dpad_RIGHT = "right";

        public const string Button_Menu = "menu";
        public const string Button_View = "view";
        public const string Button_XBOX = "home";


        // Trigger
        public const string Trigger_LT = "lt";
        public const string Trigger_RT = "rt";

        // Axis
        public const string LeftStickX = "lsx";
        public const string LeftStickY = "lsy";
        public const string RightStickX = "rsx";
        public const string RightStickY = "rsy";



        public static Dictionary<string, Xbox360Button> s_ButtonDic = new Dictionary<string, Xbox360Button>()
        {
            { Button_A, Xbox360Button.A },
            { Button_B, Xbox360Button.B },
            { Button_X, Xbox360Button.X },
            { Button_Y, Xbox360Button.Y },

            { Button_LB, Xbox360Button.LeftShoulder },
            { Button_RB, Xbox360Button.RightShoulder},

            { LeftStickButton, Xbox360Button.LeftThumb},
            { RightStickButton, Xbox360Button.RightThumb},

            { Dpad_UP, Xbox360Button.Up},
            { Dpad_DOWN, Xbox360Button.Down},
            { Dpad_LEFT, Xbox360Button.Left},
            { Dpad_RIGHT, Xbox360Button.Right},

            { Button_Menu, Xbox360Button.Start},
            { Button_View, Xbox360Button.Back},
            { Button_XBOX, Xbox360Button.Guide},
        };

        public static Dictionary<string, Xbox360Slider> s_TriggerDic = new Dictionary<string, Xbox360Slider>()
        {
            { Trigger_LT, Xbox360Slider.LeftTrigger },
            { Trigger_RT, Xbox360Slider.RightTrigger },
        };

        public static Dictionary<string, Xbox360Axis> s_AxisDic = new Dictionary<string, Xbox360Axis>()
        {
            { LeftStickX, Xbox360Axis.LeftThumbX },
            { LeftStickY, Xbox360Axis.LeftThumbY },
            { RightStickX, Xbox360Axis.RightThumbX },
            { RightStickY, Xbox360Axis.RightThumbY },
        };
    }
}
