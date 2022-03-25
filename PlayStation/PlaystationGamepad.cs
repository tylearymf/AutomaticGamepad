using System.Collections.Generic;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.DualShock4;

namespace AutomaticGamepad
{
    public class PlaystationGamepad : Gamepad
    {
        public string Button_Triangle = "b1";
        public string Button_Circle = "b2";
        public string Button_Cross = "b3";
        public string Button_Square = "b4";

        public string Button_L1 = "l1";
        public string Button_R1 = "r1";

        public string LeftStickButton = "l3";
        public string RightStickButton = "r3";

        public string Button_Option = "option";
        public string Button_Share = "share";
        public string Button_Touchpad = "touchpad";
        public string Button_Home = "home";

        public string DPad_UP = "up";
        public string DPad_DOWN = "down";
        public string DPad_LEFT = "left";
        public string DPad_RIGHT = "right";

        public string Trigger_L2 = "l2";
        public string Trigger_R2 = "r2";

        public string LeftStickX = "lsx";
        public string LeftStickY = "lsy";
        public string RightStickX = "rsx";
        public string RightStickY = "rsy";


        public override GamepadType GamepadType => GamepadType.PlayStation;
        public override string ApplicationName => "PS Remote Play";
        public override string PictureName => "ps5";
        protected override IVirtualGamepad Internal_Gamepad
        {
            get { return m_Controller; }
            set { m_Controller = (IDualShock4Controller)value; }
        }

        protected override Dictionary<string, GamepadProperty> GamepadPropertyDic => m_PropertyDic;

        IDualShock4Controller m_Controller;
        Dictionary<string, GamepadProperty> m_PropertyDic;


        public PlaystationGamepad()
        {
            m_Controller = Client.CreateDualShock4Controller();
            m_PropertyDic = new Dictionary<string, GamepadProperty>()
            {
                { Button_Triangle, (GamepadButton)DualShock4Button.Triangle },
                { Button_Circle, (GamepadButton)DualShock4Button.Circle },
                { Button_Cross, (GamepadButton)DualShock4Button.Cross },
                { Button_Square, (GamepadButton)DualShock4Button.Square },

                { Button_L1, (GamepadButton)DualShock4Button.ShoulderLeft },
                { Button_R1, (GamepadButton)DualShock4Button.ShoulderRight },

                // 按下左摇杆
                { LeftStickButton, (GamepadButton)DualShock4Button.ThumbLeft },
                // 按下右摇杆
                { RightStickButton, (GamepadButton)DualShock4Button.ThumbRight },

                { Button_Option, (GamepadButton)DualShock4Button.Options },
                { Button_Share, (GamepadButton)DualShock4Button.Share },
                { Button_Touchpad, (GamepadButton)SpecialButton.Touchpad },
                { Button_Home, (GamepadButton)SpecialButton.Ps },

                { DPad_UP, (GamepadDPad)DualShock4DPadDirection.North },
                { DPad_DOWN, (GamepadDPad)DualShock4DPadDirection.South },
                { DPad_LEFT, (GamepadDPad)DualShock4DPadDirection.West },
                { DPad_RIGHT, (GamepadDPad)DualShock4DPadDirection.East },

                { Trigger_L2, (GamepadTrigger)DualShock4Slider.LeftTrigger },
                { Trigger_R2, (GamepadTrigger)DualShock4Slider.RightTrigger },

                // Left Stick
                { LeftStickX, (GamepadAxis)DualShock4Axis.LeftThumbX },
                { LeftStickY, (GamepadAxis)DualShock4Axis.LeftThumbY },
                // Right Stick
                { RightStickX, (GamepadAxis)DualShock4Axis.RightThumbX },
                { RightStickY, (GamepadAxis)DualShock4Axis.RightThumbY },
            };
        }
    }

    public class SpecialButton : DualShock4SpecialButton
    {
        public new static SpecialButton Ps = new SpecialButton(12, "PS", 1);
        public new static SpecialButton Touchpad = new SpecialButton(13, "PS", 2);

        public SpecialButton(int id, string name, ushort value) : base(id, name, value) { }
    }
}
