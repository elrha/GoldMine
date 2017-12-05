using Mines.Defines;
using Mines.Helper;
using Mines.Manager.GameManager;
using Mines.Manager.MapManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mines.Controls
{
    class MainControl
    {
        private PictureBox MainCanvas;
        private Graphics MainGraphics;
        private ComboBox MapSelector;
        private Button PauseButton;
        private Button NewButton;
        
        private Func<bool> pauseAction;
        private Action newAction;
        
        public MainControl(Form mainForm, Func<bool> pauseAction, Action newAction)
        {
            var mainCanvasTop = Convert.ToInt32(mainForm.ClientSize.Height * Config.PlayerControlRatio);
            this.pauseAction = pauseAction;
            this.newAction = newAction;

            this.MainCanvas = new PictureBox();
            this.MainCanvas.Location = new Point(0, mainCanvasTop);
            this.MainCanvas.Size = new Size(mainForm.ClientSize.Width, mainForm.ClientSize.Height - (mainCanvasTop + Config.StartButtonHeight));
            mainForm.Controls.Add(this.MainCanvas);

            this.MainGraphics = this.MainCanvas.CreateGraphics();

            var controlUnit = mainForm.ClientSize.Width / 5;

            this.MapSelector = new ComboBox();
            this.MapSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            this.MapSelector.SetBounds(0, this.MainCanvas.Bottom, controlUnit, Config.StartButtonHeight);
            foreach (var mapName in MapManager.Instance.GetMapNameList())
                this.MapSelector.Items.Add(mapName);
            this.MapSelector.SelectedIndex = 0;
            MapManager.Instance.SelectMap(this.MapSelector.SelectedItem.ToString());
            this.MapSelector.SelectedIndexChanged += (object sender, EventArgs e) => { MapManager.Instance.SelectMap(this.MapSelector.SelectedItem.ToString()); };
            mainForm.Controls.Add(this.MapSelector);

            this.PauseButton = new Button();
            this.PauseButton.Text = "Pause";
            this.PauseButton.SetBounds(controlUnit, this.MainCanvas.Bottom, controlUnit * 2, Config.StartButtonHeight);
            this.PauseButton.Click += PauseBtnClicked;
            mainForm.Controls.Add(this.PauseButton);

            this.NewButton = new Button();
            this.NewButton.Text = "New Game";
            this.NewButton.SetBounds(controlUnit * 3, this.MainCanvas.Bottom, controlUnit * 2, Config.StartButtonHeight);
            this.NewButton.Click += NewBtnClicked;
            mainForm.Controls.Add(this.NewButton);
        }

        private void NewBtnClicked(object sender, EventArgs e)
        {
            this.newAction();
            this.PauseButton.Text = "Pause";
        }

        private void PauseBtnClicked(object sender, EventArgs e)
        {
            if (this.pauseAction()) this.PauseButton.Text = "Resume";
            else this.PauseButton.Text = "Pause";
        }

        public void Resize(Size clientSize)
        {
            var mainCanvasTop = Convert.ToInt32(clientSize.Height * Config.PlayerControlRatio);
            this.MainCanvas.SetBounds(0, mainCanvasTop, clientSize.Width, clientSize.Height - (mainCanvasTop + Config.StartButtonHeight));

            this.MainGraphics.Dispose();
            this.MainGraphics = this.MainCanvas.CreateGraphics();

            var controlUnit = clientSize.Width / 5;
            this.MapSelector.SetBounds(0, this.MainCanvas.Bottom, controlUnit, Config.StartButtonHeight);
            this.PauseButton.SetBounds(controlUnit, this.MainCanvas.Bottom, controlUnit * 2, Config.StartButtonHeight);
            this.NewButton.SetBounds(controlUnit * 3, this.MainCanvas.Bottom, controlUnit * 2, Config.StartButtonHeight);
        }

        public void Update(Image image)
        {
            this.MainCanvas.BeginInvoke(new Action(() =>
            {
                lock(image)
                    this.MainGraphics.DrawImage(image, 0, 0, this.MainCanvas.Width, this.MainCanvas.Height);
            }));
        }
    }
}
