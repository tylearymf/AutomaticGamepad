using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AutomaticGamepad
{
    public class ControllerForm : Form
    {
        public MainForm MainForm { set; get; }

        protected virtual PictureBox PictureBox { get; }
        protected virtual Image SourceImage { get; }

        protected Image m_GrayImage;


        public ControllerForm()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            m_GrayImage = SourceImage.ToGrayScale();
            UpdateImage();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (m_GrayImage != null)
                m_GrayImage.Dispose();
            m_GrayImage = null;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            UpdateImage();
        }

        protected void Control_MouseLeave(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.Transparent;
        }

        protected void Control_MouseHover(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.FromArgb(100, Color.Gray);
        }

        protected void Control_MouseDown(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.FromArgb(100, Color.White);
        }

        protected void Control_MouseUp(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.FromArgb(100, Color.Gray);
        }

        protected void Control_Click(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;
            var tag = pic.Tag?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(tag))
            {
                MessageBox.Show($"控件 {pic.Name} 的 Tag 未设置.");
                return;
            }

            var args = tag.Split(',');
            var gamepad = MainForm.Gamepad;
            var gamepadType = gamepad.GetType();
            var fieldName = args[0];
            var field = gamepadType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
            if (field == null)
            {
                MessageBox.Show($"{gamepadType.FullName} 不包含字段 {fieldName}");
                return;
            }

            args[0] = field.GetValue(gamepad).ToString();
            gamepad.CallMethod(args);
        }

        protected void BindTransparent(PictureBox pictureBox, Control parent)
        {
            pictureBox.Image = Properties.Resources.point;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Parent = parent;

            BindEvent(pictureBox);
        }

        protected void BindArrow(PictureBox up, PictureBox down, PictureBox left, PictureBox right, bool isWhite)
        {
            var sourceImage = isWhite ? Properties.Resources.arrow_white : Properties.Resources.arrow_black;
            down.Image = new Bitmap(sourceImage);
            down.SizeMode = PictureBoxSizeMode.Zoom;

            left.Image = new Bitmap(sourceImage);
            left.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            left.SizeMode = PictureBoxSizeMode.Zoom;

            up.Image = new Bitmap(sourceImage);
            up.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            up.SizeMode = PictureBoxSizeMode.Zoom;

            right.Image = new Bitmap(sourceImage);
            right.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            right.SizeMode = PictureBoxSizeMode.Zoom;

            BindEvent(up);
            BindEvent(down);
            BindEvent(left);
            BindEvent(right);
        }

        private void BindEvent(PictureBox pictureBox)
        {
            pictureBox.MouseHover -= Control_MouseHover;
            pictureBox.MouseHover += Control_MouseHover;

            pictureBox.MouseLeave -= Control_MouseLeave;
            pictureBox.MouseLeave += Control_MouseLeave;

            pictureBox.MouseDown -= Control_MouseDown;
            pictureBox.MouseDown += Control_MouseDown;

            pictureBox.MouseUp -= Control_MouseUp;
            pictureBox.MouseUp += Control_MouseUp;

            pictureBox.Click -= Control_Click;
            pictureBox.Click += Control_Click;
        }

        private void UpdateImage()
        {
            var pic = PictureBox;
            if (pic != null)
                pic.Image = Enabled ? SourceImage : m_GrayImage;
        }
    }
}
