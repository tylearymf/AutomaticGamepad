using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AutomaticGamepad
{
    public partial class XboxController : ControllerForm
    {
        protected override PictureBox PictureBox => pictureBox1;
        protected override Image SourceImage => Properties.Resources.xbox_controller;


        public XboxController()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var pictureBoxes = new List<PictureBox>() { button1, button2, button3, button4, button5,
                                                        button6, button7, button8, button9, button10,
                                                        button11, button12, button13, button14, button15,
                                                        button16, button17 };
            foreach (var item in pictureBoxes)
                BindTransparent(item, pictureBox1);

            BindArrow(ls_up, ls_down, ls_left, ls_right, true);
            BindArrow(rs_up, rs_down, rs_left, rs_right, true);
        }
    }
}
