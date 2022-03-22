
using System;
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

        public override void Button(string name, double duration = 200)
        {
            if (XboxControllerNames.s_ButtonDic.TryGetValue(name, out var button))
                SetButton(button, duration);
        }

        public override void Trigger(string name, double value, double duration = 200)
        {
            if (XboxControllerNames.s_TriggerDic.TryGetValue(name, out var trigger))
                SetTrigger(trigger, (byte)(value * byte.MaxValue), duration);
        }

        public override void Axis(string name, double value, double duration = 200)
        {
            if (XboxControllerNames.s_AxisDic.TryGetValue(name, out var axis))
                SetAxis(axis, (short)(value * short.MaxValue), duration);
        }

        public override void Axis2(string name1, string name2, double value1, double value2, double duration = 200)
        {
            var result1 = XboxControllerNames.s_AxisDic.TryGetValue(name1, out var axis1);
            var result2 = XboxControllerNames.s_AxisDic.TryGetValue(name2, out var axis2);
            if (result1 && result2)
            {
                var len = Math.Pow(value1 + value2, 0.5F);
                value1 = value1 / len;
                value2 = value2 / len;

                SetAxis2(axis1, axis2, (short)(value1 * short.MaxValue), (short)(value2 * short.MaxValue), duration);
            }
        }


        void SetButton(Xbox360Button button, double duration = 200)
        {
            m_Controller.SetButtonState(button, true);
            Sleep(duration);
            m_Controller.SetButtonState(button, false);
            Sleep(200);
        }

        void SetTrigger(Xbox360Slider slider, byte value, double duration = 200)
        {
            m_Controller.SetSliderValue(slider, value);
            Sleep(duration);
            m_Controller.SetSliderValue(slider, 0);
            Sleep(200);
        }

        void SetAxis(Xbox360Axis axis, short value, double duration = 200)
        {
            m_Controller.SetAxisValue(axis, value);
            Sleep(duration);
            m_Controller.SetAxisValue(axis, 0);
            Sleep(200);
        }

        void SetAxis2(Xbox360Axis axis1, Xbox360Axis axis2, short value1, short value2, double duration = 200)
        {
            m_Controller.SetAxisValue(axis1, value1);
            m_Controller.SetAxisValue(axis2, value2);
            Sleep(duration);
            m_Controller.SetAxisValue(axis1, 0);
            m_Controller.SetAxisValue(axis2, 0);
            Sleep(200);
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
