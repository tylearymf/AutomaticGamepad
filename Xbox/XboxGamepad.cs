using System.Collections.Generic;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace AutomaticGamepad
{
    public class XboxGamepad : Gamepad
    {
        #region Button Alias

        public string Button_A = "a";
        public string Button_B = "b";
        public string Button_X = "x";
        public string Button_Y = "y";

        public string Button_LB = "lb";
        public string Button_RB = "rb";

        public string LeftStickButton = "lsb";
        public string RightStickButton = "rsb";

        public string Button_Menu = "menu";
        public string Button_View = "view";
        public string Button_Home = "home";

        public string DPad_UP = "up";
        public string DPad_DOWN = "down";
        public string DPad_LEFT = "left";
        public string DPad_RIGHT = "right";

        public string Trigger_LT = "lt";
        public string Trigger_RT = "rt";

        public string LeftStickX = "lsx";
        public string LeftStickY = "lsy";
        public string RightStickX = "rsx";
        public string RightStickY = "rsy";

        #endregion


        public override GamepadType GamepadType => GamepadType.Xbox;
        public override string BindApplicationName => "Xbox";
        public override string PictureName => "xbox";
        protected override IVirtualGamepad Internal_Gamepad
        {
            get { return m_Controller; }
            set { m_Controller = (IXbox360Controller)value; }
        }

        protected override Dictionary<string, GamepadProperty> GamepadPropertyDic => m_PropertyDic;

        IXbox360Controller m_Controller;
        Dictionary<string, GamepadProperty> m_PropertyDic;


        public XboxGamepad()
        {
            m_Controller = Client.CreateXbox360Controller();
            m_PropertyDic = new Dictionary<string, GamepadProperty>()
            {
                { Button_A, (GamepadButton)Xbox360Button.A },
                { Button_B, (GamepadButton)Xbox360Button.B },
                { Button_X, (GamepadButton)Xbox360Button.X },
                { Button_Y, (GamepadButton)Xbox360Button.Y },

                { Button_LB, (GamepadButton)Xbox360Button.LeftShoulder },
                { Button_RB, (GamepadButton)Xbox360Button.RightShoulder },

                // 按下左摇杆
                { LeftStickButton, (GamepadButton)Xbox360Button.LeftThumb },
                // 按下右摇杆
                { RightStickButton, (GamepadButton)Xbox360Button.RightThumb },

                { Button_Menu, (GamepadButton)Xbox360Button.Start },
                { Button_View, (GamepadButton)Xbox360Button.Back },
                { Button_Home, (GamepadButton)Xbox360Button.Guide },

                { DPad_UP, (GamepadDPad)Xbox360Button.Up },
                { DPad_DOWN, (GamepadDPad)Xbox360Button.Down },
                { DPad_LEFT, (GamepadDPad)Xbox360Button.Left },
                { DPad_RIGHT, (GamepadDPad)Xbox360Button.Right },

                { Trigger_LT, (GamepadTrigger)Xbox360Slider.LeftTrigger },
                { Trigger_RT, (GamepadTrigger)Xbox360Slider.RightTrigger },

                { LeftStickX, (GamepadAxis)Xbox360Axis.LeftThumbX },
                { LeftStickY, (GamepadAxis)Xbox360Axis.LeftThumbY },
                { RightStickX, (GamepadAxis)Xbox360Axis.RightThumbX },
                { RightStickY, (GamepadAxis)Xbox360Axis.RightThumbY },
            };
        }
    }
}
