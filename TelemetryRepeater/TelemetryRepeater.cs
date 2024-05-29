using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelemetryRepeater
{
    public partial class TelemetryRepeater : Form
    {
        Info info = new Info();
        string infoPath = "";
        Repeater repeater1 = new Repeater();
        public TelemetryRepeater()
        {
            InitializeComponent();
            Log.LogTextBox = LogTextBox;
            infoPath = Directory.GetParent(Application.ExecutablePath) + "\\info.txt";
            LoadInfo();
        }

        private void LoadInfo()
        {
            if (File.Exists(infoPath))
            {
                string s = File.ReadAllText(infoPath);
                info = JsonConvert.DeserializeObject<Info>(s);
            }
            this.IPText.Text = info.CrtIp;
            this.PortText.Text = info.Port;
            this.SOSPort1TextBox.Text = info.SOSPort1;
        }
        private void SaveInfoButton_Click(object sender, EventArgs e)
        {
            if (IPText.ReadOnly)
            {
                this.SaveInfoButton.Text = "서버 정보 저장";
                IPText.ReadOnly = false;
                PortText.ReadOnly = false;
                this.SOSPort1TextBox.ReadOnly = false;
            }
            else
            {
                info.CrtIp = this.IPText.Text;
                info.Port = this.PortText.Text;
                info.SOSPort1 = this.SOSPort1TextBox.Text;
                string s = JsonConvert.SerializeObject(info);
                File.WriteAllText(infoPath, s);
                IPText.ReadOnly = true;
                PortText.ReadOnly = true;
                this.SOSPort1TextBox.ReadOnly = true;
                this.SaveInfoButton.Text = "서버 정보 수정";

                MessageBox.Show("수정된 서버 정보를 적용하기 위해서는 재 시작 해야합니다.");
                this.Close();
            }
        }
        bool isRunning = false;
        private void StartButton_Click(object sender, EventArgs e)
        {
            Start();
            
        }
        public void Start()
        {
            if (isRunning)
            {
                this.Close();
            }
            else
            {
                isRunning = true;
                StartButton.Text = "중지 및 종료";
                repeater1.CrtIP = info.CrtIp;
                repeater1.CrtPort = Int32.Parse(info.Port);
                repeater1.ServerPort = Int32.Parse(info.SOSPort1);
                repeater1.Start();
            }
        }
        private void OpenDirectoryButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Directory.GetParent(Application.ExecutablePath).ToString());
            
        }

        private void TelemetryRepeater_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("서버 종료 및 재시작 하시겠습니까?", "서버 종료", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Process.Start(Application.ExecutablePath);
            }
        }

        private void TelemetryRepeater_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void TelemetryRepeater_Load(object sender, EventArgs e)
        {
            Process[] process = Process.GetProcessesByName("TelemetryRepeater");
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process p in process)
            {
                if (p.Id != currentProcess.Id)
                    p.Kill();
            }
            Thread.Sleep(2000);
            Start();
        }

        private void ForcedKillButton_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("강제 종료 하시겠습니까?", "서버 종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process[] process = Process.GetProcessesByName("TelemetryRepeater");
                Process currentProcess = Process.GetCurrentProcess();
                foreach (Process p in process)
                {
                    p.Kill();
                }
            }
        }
    }
}
