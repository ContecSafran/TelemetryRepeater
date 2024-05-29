
namespace TelemetryRepeater
{
    partial class TelemetryRepeater
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.TmrSplitContainer = new System.Windows.Forms.SplitContainer();
            this.TmrInfoPanel = new System.Windows.Forms.Panel();
            this.OpenDirectoryButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SOSPort1TextBox = new System.Windows.Forms.TextBox();
            this.SOSPort1Label = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.SaveInfoButton = new System.Windows.Forms.Button();
            this.PortText = new System.Windows.Forms.TextBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.IPText = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.ForcedKillButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TmrSplitContainer)).BeginInit();
            this.TmrSplitContainer.Panel1.SuspendLayout();
            this.TmrSplitContainer.Panel2.SuspendLayout();
            this.TmrSplitContainer.SuspendLayout();
            this.TmrInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TmrSplitContainer
            // 
            this.TmrSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TmrSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.TmrSplitContainer.Name = "TmrSplitContainer";
            this.TmrSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // TmrSplitContainer.Panel1
            // 
            this.TmrSplitContainer.Panel1.Controls.Add(this.TmrInfoPanel);
            // 
            // TmrSplitContainer.Panel2
            // 
            this.TmrSplitContainer.Panel2.Controls.Add(this.LogTextBox);
            this.TmrSplitContainer.Size = new System.Drawing.Size(1073, 356);
            this.TmrSplitContainer.SplitterDistance = 137;
            this.TmrSplitContainer.TabIndex = 1;
            // 
            // TmrInfoPanel
            // 
            this.TmrInfoPanel.Controls.Add(this.ForcedKillButton);
            this.TmrInfoPanel.Controls.Add(this.OpenDirectoryButton);
            this.TmrInfoPanel.Controls.Add(this.label1);
            this.TmrInfoPanel.Controls.Add(this.SOSPort1TextBox);
            this.TmrInfoPanel.Controls.Add(this.SOSPort1Label);
            this.TmrInfoPanel.Controls.Add(this.StartButton);
            this.TmrInfoPanel.Controls.Add(this.SaveInfoButton);
            this.TmrInfoPanel.Controls.Add(this.PortText);
            this.TmrInfoPanel.Controls.Add(this.PortLabel);
            this.TmrInfoPanel.Controls.Add(this.IPText);
            this.TmrInfoPanel.Controls.Add(this.IPLabel);
            this.TmrInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TmrInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.TmrInfoPanel.Name = "TmrInfoPanel";
            this.TmrInfoPanel.Size = new System.Drawing.Size(1073, 137);
            this.TmrInfoPanel.TabIndex = 0;
            // 
            // OpenDirectoryButton
            // 
            this.OpenDirectoryButton.Location = new System.Drawing.Point(448, 6);
            this.OpenDirectoryButton.Name = "OpenDirectoryButton";
            this.OpenDirectoryButton.Size = new System.Drawing.Size(113, 77);
            this.OpenDirectoryButton.TabIndex = 11;
            this.OpenDirectoryButton.Text = "폴더 열기";
            this.OpenDirectoryButton.UseVisualStyleBackColor = true;
            this.OpenDirectoryButton.Click += new System.EventHandler(this.OpenDirectoryButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(321, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "SOS Port는 SOS가 이 프로그램으로 접속 하는 포트입니다";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SOSPort1TextBox
            // 
            this.SOSPort1TextBox.Location = new System.Drawing.Point(118, 62);
            this.SOSPort1TextBox.Name = "SOSPort1TextBox";
            this.SOSPort1TextBox.ReadOnly = true;
            this.SOSPort1TextBox.Size = new System.Drawing.Size(78, 21);
            this.SOSPort1TextBox.TabIndex = 9;
            // 
            // SOSPort1Label
            // 
            this.SOSPort1Label.AutoSize = true;
            this.SOSPort1Label.Location = new System.Drawing.Point(12, 68);
            this.SOSPort1Label.Name = "SOSPort1Label";
            this.SOSPort1Label.Size = new System.Drawing.Size(100, 12);
            this.SOSPort1Label.TabIndex = 8;
            this.SOSPort1Label.Text = "SosPort1(TMUA)";
            this.SOSPort1Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(329, 6);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(113, 77);
            this.StartButton.TabIndex = 7;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SaveInfoButton
            // 
            this.SaveInfoButton.Location = new System.Drawing.Point(202, 6);
            this.SaveInfoButton.Name = "SaveInfoButton";
            this.SaveInfoButton.Size = new System.Drawing.Size(121, 77);
            this.SaveInfoButton.TabIndex = 6;
            this.SaveInfoButton.Text = "서버 정보 수정";
            this.SaveInfoButton.UseVisualStyleBackColor = true;
            this.SaveInfoButton.Click += new System.EventHandler(this.SaveInfoButton_Click);
            // 
            // PortText
            // 
            this.PortText.Location = new System.Drawing.Point(47, 35);
            this.PortText.Name = "PortText";
            this.PortText.ReadOnly = true;
            this.PortText.Size = new System.Drawing.Size(149, 21);
            this.PortText.TabIndex = 5;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(12, 41);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(27, 12);
            this.PortLabel.TabIndex = 4;
            this.PortLabel.Text = "Port";
            this.PortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IPText
            // 
            this.IPText.Location = new System.Drawing.Point(47, 8);
            this.IPText.Name = "IPText";
            this.IPText.ReadOnly = true;
            this.IPText.Size = new System.Drawing.Size(149, 21);
            this.IPText.TabIndex = 3;
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(12, 14);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(16, 12);
            this.IPLabel.TabIndex = 2;
            this.IPLabel.Text = "IP";
            this.IPLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogTextBox
            // 
            this.LogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogTextBox.Location = new System.Drawing.Point(0, 0);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.Size = new System.Drawing.Size(1073, 215);
            this.LogTextBox.TabIndex = 1;
            // 
            // ForcedKillButton
            // 
            this.ForcedKillButton.Location = new System.Drawing.Point(957, 6);
            this.ForcedKillButton.Name = "ForcedKillButton";
            this.ForcedKillButton.Size = new System.Drawing.Size(113, 77);
            this.ForcedKillButton.TabIndex = 12;
            this.ForcedKillButton.Text = "강제종료";
            this.ForcedKillButton.UseVisualStyleBackColor = true;
            this.ForcedKillButton.Click += new System.EventHandler(this.ForcedKillButton_Click);
            // 
            // TelemetryRepeater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 356);
            this.Controls.Add(this.TmrSplitContainer);
            this.Name = "TelemetryRepeater";
            this.Text = "TelemetryRepeater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TelemetryRepeater_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TelemetryRepeater_FormClosed);
            this.Load += new System.EventHandler(this.TelemetryRepeater_Load);
            this.TmrSplitContainer.Panel1.ResumeLayout(false);
            this.TmrSplitContainer.Panel2.ResumeLayout(false);
            this.TmrSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TmrSplitContainer)).EndInit();
            this.TmrSplitContainer.ResumeLayout(false);
            this.TmrInfoPanel.ResumeLayout(false);
            this.TmrInfoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer TmrSplitContainer;
        private System.Windows.Forms.Panel TmrInfoPanel;
        private System.Windows.Forms.TextBox PortText;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox IPText;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button SaveInfoButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SOSPort1TextBox;
        private System.Windows.Forms.Label SOSPort1Label;
        private System.Windows.Forms.Button OpenDirectoryButton;
        private System.Windows.Forms.Button ForcedKillButton;
    }
}

