using Mines.Defines;
using Mines.Manager.GameManager;
using PerseusCommon.Manager.AssemblyManager;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mines.Controls
{
    class PlayerControl
    {
        private int playerIndex;
        private ComboBox AssemblySelector;
        private PictureBox StateCanvas;
        private Image StateBackImage;
        private Label NameLabel;

        private Label ScoreTitleLabel;
        private Label PowerTitleLabel;
        private Label StunTitleLabel;

        private Label ScoreLabel;
        private Label PowerLabel;
        private Label StunLabel;

        public PlayerControl(Form mainForm, int playerIndex)
        {
            this.playerIndex = playerIndex;
            var playerControlWidth = Convert.ToInt32((double)mainForm.ClientSize.Width / Config.PlayerCount);
            var playerControlBottom = Convert.ToInt32(mainForm.ClientSize.Height * Config.PlayerControlRatio);
            
            this.AssemblySelector = new ComboBox();
            this.AssemblySelector.DropDownStyle = ComboBoxStyle.DropDownList;
            this.AssemblySelector.SetBounds(playerControlWidth * this.playerIndex, 0, playerControlWidth, Convert.ToInt32(playerControlBottom * .1));
            foreach (var playerItem in AssemblyManager.Instance.Players)
                this.AssemblySelector.Items.Add(new PlayerComboBoxItem(playerItem.Key, playerItem.Value));
            if(this.AssemblySelector.Items.Count > 0)
                this.AssemblySelector.SelectedIndex = 0;
            mainForm.Controls.Add(this.AssemblySelector);

            this.StateCanvas = new PictureBox();
            this.StateCanvas.BorderStyle = BorderStyle.Fixed3D;
            this.StateCanvas.SetBounds(playerControlWidth * this.playerIndex, AssemblySelector.Height, playerControlWidth / 5, playerControlBottom - AssemblySelector.Height);
            this.StateBackImage = new Bitmap(this.StateCanvas.Width, this.StateCanvas.Height);
            mainForm.Controls.Add(this.StateCanvas);

            this.NameLabel = new Label();
            this.NameLabel.BorderStyle = BorderStyle.Fixed3D;
            this.NameLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5), AssemblySelector.Height, (playerControlWidth / 5) * 4, (playerControlBottom - AssemblySelector.Height) / 3);
            mainForm.Controls.Add(this.NameLabel);

            var labelWidth = ((playerControlWidth / 5) * 4) / 3;

            this.ScoreTitleLabel = new Label();
            this.ScoreTitleLabel.BorderStyle = BorderStyle.Fixed3D;
            this.ScoreTitleLabel.Text = "Score";
            this.ScoreTitleLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5), AssemblySelector.Height + this.NameLabel.Height, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            mainForm.Controls.Add(this.ScoreTitleLabel);

            this.PowerTitleLabel = new Label();
            this.PowerTitleLabel.BorderStyle = BorderStyle.Fixed3D;
            this.PowerTitleLabel.Text = "Power";
            this.PowerTitleLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5) + labelWidth, AssemblySelector.Height + this.NameLabel.Height, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            mainForm.Controls.Add(this.PowerTitleLabel);

            this.StunTitleLabel = new Label();
            this.StunTitleLabel.BorderStyle = BorderStyle.Fixed3D;
            this.StunTitleLabel.Text = "Stun";
            this.StunTitleLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5) + labelWidth * 2, AssemblySelector.Height + this.NameLabel.Height, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            mainForm.Controls.Add(this.StunTitleLabel);

            this.ScoreLabel = new Label();
            this.ScoreLabel.BorderStyle = BorderStyle.Fixed3D;
            this.ScoreLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5), AssemblySelector.Height + this.NameLabel.Height * 2, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            mainForm.Controls.Add(this.ScoreLabel);

            this.PowerLabel = new Label();
            this.PowerLabel.BorderStyle = BorderStyle.Fixed3D;
            this.PowerLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5) + labelWidth, AssemblySelector.Height + this.NameLabel.Height * 2, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            mainForm.Controls.Add(this.PowerLabel);

            this.StunLabel = new Label();
            this.StunLabel.BorderStyle = BorderStyle.Fixed3D;
            this.StunLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5) + labelWidth * 2, AssemblySelector.Height + this.NameLabel.Height * 2, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            mainForm.Controls.Add(this.StunLabel);
        }

        public KeyValuePair<int, IPlayer> CreatePlayerAsm(int totalPlayerCount, int col, int row)
        {
            var ret = (this.AssemblySelector.SelectedItem as PlayerComboBoxItem).PlayerGenerator.New();
            ret.Initialize(playerIndex, totalPlayerCount, col, row);
            return new KeyValuePair<int, IPlayer>(playerIndex, ret);
        }

        public void Resize(Size clientSize)
        {
            var playerControlWidth = Convert.ToInt32((double)clientSize.Width / Config.PlayerCount);
            var playerControlBottom = Convert.ToInt32(clientSize.Height * Config.PlayerControlRatio);
            
            this.AssemblySelector.SetBounds(playerControlWidth * this.playerIndex, 0, playerControlWidth, Convert.ToInt32(playerControlBottom * .1));
            this.StateCanvas.SetBounds(playerControlWidth * this.playerIndex, AssemblySelector.Height, playerControlWidth / 5, playerControlBottom - AssemblySelector.Height);
            this.NameLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5), AssemblySelector.Height, (playerControlWidth / 5) * 4, (playerControlBottom - AssemblySelector.Height) / 3);

            var labelWidth = ((playerControlWidth / 5) * 4) / 3;
            this.ScoreTitleLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5), AssemblySelector.Height + this.NameLabel.Height, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            this.PowerTitleLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5) + labelWidth, AssemblySelector.Height + this.NameLabel.Height, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            this.StunTitleLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5) + labelWidth * 2, AssemblySelector.Height + this.NameLabel.Height, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            this.ScoreLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5), AssemblySelector.Height + this.NameLabel.Height * 2, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            this.PowerLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5) + labelWidth, AssemblySelector.Height + this.NameLabel.Height * 2, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
            this.StunLabel.SetBounds((playerControlWidth * this.playerIndex) + (playerControlWidth / 5) + labelWidth * 2, AssemblySelector.Height + this.NameLabel.Height * 2, labelWidth, (playerControlBottom - AssemblySelector.Height) / 3);
        }

        public Image GetBackImage()
        {
            return this.StateBackImage;
        }

        public void UpdateStateImage()
        {
            this.StateCanvas.BeginInvoke(new Action(() =>
            {
                using (var stateCanvasGraphics = this.StateCanvas.CreateGraphics())
                    lock(this.StateBackImage)
                        stateCanvasGraphics.DrawImage(this.StateBackImage, 0, 0, this.StateCanvas.Width, this.StateCanvas.Height);
            }));
        }

        public void UpdateScore(string name, string score, string power, string stun)
        {
            this.StateCanvas.BeginInvoke(new Action(() =>
            {
                this.NameLabel.Text = name;
                this.ScoreLabel.Text = score;
                this.PowerLabel.Text = power;
                this.StunLabel.Text = stun;
            }));
        }
    }
}
