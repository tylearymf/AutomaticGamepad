using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.ClearScript.V8;

namespace AutomaticGamepad
{
    public partial class MainForm : Form
    {
        const string FileExtension = ".ag";
        const int StartIntervalTime = 1;
        const int StopIntervalTime = 1;

        public Gamepad Gamepad { private set; get; }

        DateTime m_TickTime;
        DateTime m_LastStartTime;
        ToolTip m_Tooltip;
        ControllerForm m_ControllerForm;
        bool m_ShowTooltipInItem;

        CheckBoxType m_CheckBoxType;
        int m_MaxExecuteCount;
        int m_ExecuteCount;

        Color m_StartColor = ColorTranslator.FromHtml("#2A5CAA");
        Color m_StopColor = ColorTranslator.FromHtml("#D93A49");

        V8ScriptEngine m_ScriptEngine;
        bool m_IsRunning;


        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var splits = version.Split('.');
                label6.Text = $"V{splits[0]}.{splits[1]}";
            }
            catch { }

            comboBox1.DrawMode = DrawMode.OwnerDrawVariable;

            m_Tooltip = new ToolTip();
            m_Tooltip.ToolTipTitle = string.Empty;
            m_Tooltip.UseFading = true;
            m_Tooltip.UseAnimation = true;
            m_Tooltip.IsBalloon = false;
            m_Tooltip.ShowAlways = true;
            m_Tooltip.AutoPopDelay = 5000;
            m_Tooltip.InitialDelay = 1000;
            m_Tooltip.ReshowDelay = 0;

            SetCheckBox(CheckBoxType.Count);
            RefreshDropDownList();
            RefreshUI();

            var rootPath = Environment.CurrentDirectory;
            var playStationFile = Path.Combine(rootPath, "playstation");
            Gamepad = File.Exists(playStationFile) ? new PlaystationGamepad() : (Gamepad)new XboxGamepad();
            Gamepad.Connect();

            switch (Gamepad.GamepadType)
            {
                case GamepadType.Xbox:
                    m_ControllerForm = new XboxController();
                    break;
                case GamepadType.PlayStation:
                    m_ControllerForm = new PlayStationController();
                    break;
            }
            m_ControllerForm.MainForm = this;
            m_ControllerForm.Hide();

            textBox1.Text = Gamepad.ApplicationName;
            pictureBox1.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(Gamepad.PictureName);
            pictureBox1.Visible = !string.IsNullOrEmpty(Gamepad.PictureName);

            m_ScriptEngine = new V8ScriptEngine("v8-engine");
            m_ScriptEngine.DisableFloatNarrowing = true;
            m_ScriptEngine.ExposeHostObjectStaticMembers = false;
            m_ScriptEngine.AddHostObject(string.Empty, Microsoft.ClearScript.HostItemFlags.GlobalMembers, Gamepad);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop(true);

            Gamepad?.Dispose();
            Gamepad = null;

            m_ScriptEngine?.Dispose();
            m_ScriptEngine = null;
        }

        private void countText_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是退格和数字，则屏蔽输入
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            SetCheckBox(CheckBoxType.Count);
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            SetCheckBox(CheckBoxType.Loop);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var isRunning = m_IsRunning;
            if (!isRunning)
            {
                var totalSec = (DateTime.Now - m_LastStartTime).TotalSeconds;
                if (totalSec <= StartIntervalTime)
                {
                    MessageBox.Show($"请不要太快操作！");
                    return;
                }

                Start();
            }
            else
            {
                Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_TickTime = m_TickTime.AddMilliseconds(timer1.Interval);
            RefreshTimeUI();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("窗口标题不能为空！");
                return;
            }

            var handle = GetProcessWindow(text);
            Gamepad.Handle = handle;

            var msg = $"窗口绑定状态：{(handle != IntPtr.Zero ? "绑定成功" : "绑定失败")}(Ptr:{handle})";
            label3.Text = msg;
            Console.WriteLine(msg);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshDropDownList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_MouseEnter(object sender, EventArgs e)
        {
            ShowTooltip(comboBox1.Text);
        }

        private void comboBox1_MouseLeave(object sender, EventArgs e)
        {
            HideTooltip();
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            var content = comboBox1.Items[e.Index].ToString();
            if (m_ShowTooltipInItem && (e.State & DrawItemState.Selected) == DrawItemState.Selected)
                ShowTooltip(content);

            e.DrawBackground();
            e.Graphics.DrawString(content, e.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
            e.DrawFocusRectangle();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            m_ShowTooltipInItem = true;
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            m_ShowTooltipInItem = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (m_ControllerForm.Visible)
            {
                m_ControllerForm.Hide();
            }
            else
            {
                // 设置Owner，子窗体会随主窗体的激活而激活
                m_ControllerForm.ShowInTaskbar = false;
                m_ControllerForm.Owner = this;
                m_ControllerForm.Show();

                UpdateControllerFormPos();
            }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (m_ControllerForm?.Visible ?? false)
                UpdateControllerFormPos();
        }

        void UpdateControllerFormPos()
        {
            m_ControllerForm.Left = Left + Width - 5;
            m_ControllerForm.Top = Top + Height - m_ControllerForm.Height - 8;
        }

        void SetCheckBox(CheckBoxType type)
        {
            m_CheckBoxType = type;
            countText.Visible = type == CheckBoxType.Count;

            checkBox1.Checked = type == CheckBoxType.Count;
            checkBox2.Checked = type == CheckBoxType.Loop;
        }

        void RefreshUI()
        {
            var action = new Action(() =>
            {
                var isRunning = m_IsRunning;
                if (isRunning)
                {
                    button1.Text = "停止";
                    button1.BackColor = m_StopColor;
                    timer1.Start();

                    m_ExecuteCount = 0;
                    m_TickTime = new DateTime();
                    m_LastStartTime = DateTime.Now;
                }
                else
                {
                    button1.Text = "启动";
                    button1.BackColor = m_StartColor;
                    timer1.Stop();
                }

                groupBox3.Enabled = !isRunning;

                if (m_ControllerForm != null)
                    m_ControllerForm.Enabled = !isRunning;

                RefreshTimeUI();
            });

            if (InvokeRequired)
            {
                button1.Invoke(action);
            }
            else
            {
                action();
            }
        }

        void RefreshTimeUI()
        {
            var day = m_TickTime.Day - 1;
            if (day == 0)
                label5.Text = string.Format("({0}) {1}", m_ExecuteCount, m_TickTime.ToString("HH:mm:ss"));
            else
                label5.Text = string.Format("({0}) {1}:{2}", m_ExecuteCount, day, m_TickTime.ToString("HH:mm:ss"));
        }

        IntPtr GetProcessWindow(string processWindowTitle)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.MainWindowTitle == processWindowTitle)
                {
                    return process.MainWindowHandle;
                }
            }

            var msg = $"当前未运行{textBox1.Text}应用";
            MessageBox.Show(msg);

            return IntPtr.Zero;
        }

        void ShowTooltip(string text)
        {
            m_Tooltip.Show(text, this, groupBox3.Location.X, groupBox3.Location.Y - 20);
        }

        void HideTooltip()
        {
            m_Tooltip.Hide(this);
        }

        void RefreshDropDownList()
        {
            comboBox1.Items.Clear();

            var rootPath = Environment.CurrentDirectory;
            var filePaths = Directory.GetFiles(rootPath, $"*{FileExtension}");
            foreach (var item in filePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(item);
                comboBox1.Items.Add(fileName);
            }
        }

        void Start()
        {
            if (m_IsRunning)
                return;

            var selectIndex = comboBox1.SelectedIndex;
            if (selectIndex == -1)
            {
                MessageBox.Show($"请选择一个脚本");
                return;
            }

            var rootPath = Environment.CurrentDirectory;
            var fileName = comboBox1.Items[selectIndex].ToString();
            var filePath = Path.Combine(rootPath, fileName + FileExtension);

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"脚本文件不存在{filePath}");
                return;
            }

            switch (m_CheckBoxType)
            {
                case CheckBoxType.Count:
                    int.TryParse(countText.Text, out m_MaxExecuteCount);
                    break;
                case CheckBoxType.Loop:
                    m_MaxExecuteCount = -1;
                    break;
            }

            var jsCode = File.ReadAllText(filePath).Trim();
            if (string.IsNullOrWhiteSpace(jsCode))
            {
                MessageBox.Show($"{filePath} 文件中无代码可执行！");
                return;
            }

            // 标记状态为运行
            m_IsRunning = true;
            Gamepad.AbortThread = false;

            // 将目标窗口激活
            Gamepad.SetForeground();

            // 刷新UI
            RefreshUI();

            var scriptObj = m_ScriptEngine.Compile(jsCode);
            ThreadPool.QueueUserWorkItem(state => Execute(scriptObj));
        }

        void Stop(bool force = false)
        {
            if (!m_IsRunning && !force)
                return;

            var totalSec = (DateTime.Now - m_LastStartTime).TotalSeconds;
            if (!force && totalSec <= StopIntervalTime)
            {
                MessageBox.Show($"请不要太快操作！");
                return;
            }

            // 标记状态为停止
            m_IsRunning = false;
            Gamepad.AbortThread = true;

            // 刷新UI
            RefreshUI();
        }

        void Execute(object codeObj)
        {
            while (m_IsRunning)
            {
                try
                {
                    if (m_MaxExecuteCount != -1 && m_ExecuteCount >= m_MaxExecuteCount)
                        break;

                    m_ExecuteCount++;
                    var code = (V8Script)codeObj;
                    m_ScriptEngine.Execute(code);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Console.WriteLine(ex);
                    break;
                }
            }

            Stop();
        }
    }

    public enum CheckBoxType
    {
        Count,
        Loop,
    }
}
