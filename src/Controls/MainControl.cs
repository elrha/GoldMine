using Mines.Defines;
using Mines.Helper;
using Mines.Manager.GameManager;
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
        private ComboBox MapSelector;
        private Button StartButton;

        public MainControl(Form mainForm, EventHandler startCallback)
        {
            var mainCanvasTop = Convert.ToInt32(mainForm.ClientSize.Height * Config.PlayerControlRatio);

            this.MainCanvas = new PictureBox();
            this.MainCanvas.Location = new Point(0, mainCanvasTop);
            this.MainCanvas.Size = new Size(mainForm.ClientSize.Width, mainForm.ClientSize.Height - (mainCanvasTop + Config.StartButtonHeight));
            mainForm.Controls.Add(this.MainCanvas);

            this.MapSelector = new ComboBox();
            this.MapSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            this.MapSelector.SetBounds(0, this.MainCanvas.Bottom, mainForm.ClientSize.Width / 5, Config.StartButtonHeight);
            for(int i = 0; i < MapHelper.GetMapCount(); i++) this.MapSelector.Items.Add(i);
            if (this.MapSelector.Items.Count > 0) this.MapSelector.SelectedIndex = 0;
            mainForm.Controls.Add(this.MapSelector);
            this.MapSelector.SelectedIndexChanged += (object sender, EventArgs e) => { MapHelper.SelectMap(this.MapSelector.SelectedIndex); };

            this.StartButton = new Button();
            this.StartButton.Text = "Start Game";
            this.StartButton.SetBounds(mainForm.ClientSize.Width / 5, this.MainCanvas.Bottom, (mainForm.ClientSize.Width / 5) * 4, Config.StartButtonHeight);
            this.StartButton.Click += startCallback;
            mainForm.Controls.Add(this.StartButton);
        }

        public void Resize(Size clientSize)
        {
            var mainCanvasTop = Convert.ToInt32(clientSize.Height * Config.PlayerControlRatio);
            this.MainCanvas.SetBounds(0, mainCanvasTop, clientSize.Width, clientSize.Height - (mainCanvasTop + Config.StartButtonHeight));
            
            this.MapSelector.SetBounds(0, this.MainCanvas.Bottom, clientSize.Width / 5, Config.StartButtonHeight);
            this.StartButton.SetBounds(clientSize.Width / 5, this.MainCanvas.Bottom, (clientSize.Width / 5) * 4, Config.StartButtonHeight);
        }

        public void Update(Image image)
        {
            this.MainCanvas.BeginInvoke(new Action(() =>
            {
                lock(image)
                    using (var mainCanvasGraphics = this.MainCanvas.CreateGraphics())
                        mainCanvasGraphics.DrawImage(image, 0, 0, this.MainCanvas.Width, this.MainCanvas.Height);
            }));
        }
    }
}
