using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Follower
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        private void OnLoad(object sender, EventArgs e)
        {
            this.leaderNameBox.Text = Settings.Instance.leaderName;
            this.maxLeaderDistanceBox.Text = Settings.Instance.leaderDistance.ToString();
            this.lootDistanceBox.Text = Settings.Instance.lootDistance.ToString();
            this.fightDistanceBox.Text = Settings.Instance.fightDistance.ToString();
            this.difficultyBox.Text = Settings.Instance.difficulty.ToString();
        }
        private void OnSave(object sender, EventArgs e)
        {
            Settings.Instance.leaderName = this.leaderNameBox.Text;
            Settings.Instance.lootDistance = float.Parse(this.lootDistanceBox.Text);
            Settings.Instance.fightDistance = float.Parse(this.fightDistanceBox.Text);
            Settings.Instance.leaderDistance = float.Parse(this.maxLeaderDistanceBox.Text);
            Settings.Instance.difficulty = Int32.Parse(this.difficultyBox.Text);
            Settings.Save();
            this.Close();
        }
    }
}
