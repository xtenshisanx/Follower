namespace Follower
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SaveButton = new System.Windows.Forms.Button();
            this.leaderNameLabel = new System.Windows.Forms.Label();
            this.leaderNameBox = new System.Windows.Forms.TextBox();
            this.maxLeaderDistanceBox = new System.Windows.Forms.TextBox();
            this.lootDistanceBox = new System.Windows.Forms.TextBox();
            this.fightDistanceBox = new System.Windows.Forms.TextBox();
            this.difficultyBox = new System.Windows.Forms.TextBox();
            this.maxLeaderDistanceLabel = new System.Windows.Forms.Label();
            this.lootDistanceLabel = new System.Windows.Forms.Label();
            this.fightDistanceLabel = new System.Windows.Forms.Label();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.difficultyToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(15, 136);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(260, 23);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "SAVE";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.OnSave);
            // 
            // leaderNameLabel
            // 
            this.leaderNameLabel.AutoSize = true;
            this.leaderNameLabel.Location = new System.Drawing.Point(12, 9);
            this.leaderNameLabel.Name = "leaderNameLabel";
            this.leaderNameLabel.Size = new System.Drawing.Size(64, 13);
            this.leaderNameLabel.TabIndex = 1;
            this.leaderNameLabel.Text = "leaderName";
            // 
            // leaderNameBox
            // 
            this.leaderNameBox.Location = new System.Drawing.Point(172, 6);
            this.leaderNameBox.Name = "leaderNameBox";
            this.leaderNameBox.Size = new System.Drawing.Size(100, 20);
            this.leaderNameBox.TabIndex = 2;
            // 
            // maxLeaderDistanceBox
            // 
            this.maxLeaderDistanceBox.Location = new System.Drawing.Point(172, 32);
            this.maxLeaderDistanceBox.Name = "maxLeaderDistanceBox";
            this.maxLeaderDistanceBox.Size = new System.Drawing.Size(100, 20);
            this.maxLeaderDistanceBox.TabIndex = 3;
            // 
            // lootDistanceBox
            // 
            this.lootDistanceBox.Location = new System.Drawing.Point(172, 58);
            this.lootDistanceBox.Name = "lootDistanceBox";
            this.lootDistanceBox.Size = new System.Drawing.Size(100, 20);
            this.lootDistanceBox.TabIndex = 4;
            // 
            // fightDistanceBox
            // 
            this.fightDistanceBox.Location = new System.Drawing.Point(172, 84);
            this.fightDistanceBox.Name = "fightDistanceBox";
            this.fightDistanceBox.Size = new System.Drawing.Size(100, 20);
            this.fightDistanceBox.TabIndex = 5;
            // 
            // difficultyBox
            // 
            this.difficultyBox.Location = new System.Drawing.Point(172, 110);
            this.difficultyBox.Name = "difficultyBox";
            this.difficultyBox.Size = new System.Drawing.Size(100, 20);
            this.difficultyBox.TabIndex = 6;
            this.difficultyToolTip.SetToolTip(this.difficultyBox, "1 -> Normal\r\n2 -> Cruel\r\n3 -> Merveil");
            // 
            // maxLeaderDistanceLabel
            // 
            this.maxLeaderDistanceLabel.AutoSize = true;
            this.maxLeaderDistanceLabel.Location = new System.Drawing.Point(12, 35);
            this.maxLeaderDistanceLabel.Name = "maxLeaderDistanceLabel";
            this.maxLeaderDistanceLabel.Size = new System.Drawing.Size(101, 13);
            this.maxLeaderDistanceLabel.TabIndex = 7;
            this.maxLeaderDistanceLabel.Text = "maxLeaderDistance";
            // 
            // lootDistanceLabel
            // 
            this.lootDistanceLabel.AutoSize = true;
            this.lootDistanceLabel.Location = new System.Drawing.Point(12, 61);
            this.lootDistanceLabel.Name = "lootDistanceLabel";
            this.lootDistanceLabel.Size = new System.Drawing.Size(66, 13);
            this.lootDistanceLabel.TabIndex = 8;
            this.lootDistanceLabel.Text = "lootDistance";
            // 
            // fightDistanceLabel
            // 
            this.fightDistanceLabel.AutoSize = true;
            this.fightDistanceLabel.Location = new System.Drawing.Point(12, 87);
            this.fightDistanceLabel.Name = "fightDistanceLabel";
            this.fightDistanceLabel.Size = new System.Drawing.Size(69, 13);
            this.fightDistanceLabel.TabIndex = 9;
            this.fightDistanceLabel.Text = "fightDistance";
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(12, 110);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(45, 13);
            this.difficultyLabel.TabIndex = 10;
            this.difficultyLabel.Text = "difficulty";
            // 
            // difficultyToolTip
            // 
            this.difficultyToolTip.IsBalloon = true;
            this.difficultyToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.difficultyToolTip.ToolTipTitle = "Difficulty";
            // 
            // SettingsWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.difficultyLabel);
            this.Controls.Add(this.fightDistanceLabel);
            this.Controls.Add(this.lootDistanceLabel);
            this.Controls.Add(this.maxLeaderDistanceLabel);
            this.Controls.Add(this.difficultyBox);
            this.Controls.Add(this.fightDistanceBox);
            this.Controls.Add(this.lootDistanceBox);
            this.Controls.Add(this.maxLeaderDistanceBox);
            this.Controls.Add(this.leaderNameBox);
            this.Controls.Add(this.leaderNameLabel);
            this.Controls.Add(this.SaveButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "SettingsWindow";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SettingsWindow";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label leaderNameLabel;
        private System.Windows.Forms.TextBox leaderNameBox;
        private System.Windows.Forms.TextBox maxLeaderDistanceBox;
        private System.Windows.Forms.TextBox lootDistanceBox;
        private System.Windows.Forms.TextBox fightDistanceBox;
        private System.Windows.Forms.TextBox difficultyBox;
        private System.Windows.Forms.Label maxLeaderDistanceLabel;
        private System.Windows.Forms.Label lootDistanceLabel;
        private System.Windows.Forms.Label fightDistanceLabel;
        private System.Windows.Forms.Label difficultyLabel;
        private System.Windows.Forms.ToolTip difficultyToolTip;
    }
}