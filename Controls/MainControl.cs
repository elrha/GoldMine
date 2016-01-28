using Mines.Defines;
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
        private Button StartButton;

        public MainControl(Form mainForm, EventHandler startCallback)
        {
            var mainCanvasTop = Convert.ToInt32(mainForm.ClientSize.Height * Config.PlayerControlRatio);

            this.MainCanvas = new PictureBox();
            this.MainCanvas.Location = new Point(0, mainCanvasTop);
            this.MainCanvas.Size = new Size(mainForm.ClientSize.Width, mainForm.ClientSize.Height - (mainCanvasTop + Config.StartButtonHeight));
            mainForm.Controls.Add(this.MainCanvas);
            
            this.StartButton = new Button();
            this.StartButton.Text = "Start Game";
            this.StartButton.Size = new Size(mainForm.ClientSize.Width, Config.StartButtonHeight);
            this.StartButton.Location = new Point(0, this.MainCanvas.Bottom);
            this.StartButton.Click += startCallback;
            mainForm.Controls.Add(this.StartButton);
        }

        public void Resize(Size clientSize)
        {
            var mainCanvasTop = Convert.ToInt32(clientSize.Height * Config.PlayerControlRatio);
            this.MainCanvas.SetBounds(0, mainCanvasTop, clientSize.Width, clientSize.Height - (mainCanvasTop + Config.StartButtonHeight));
            
            this.StartButton.Width = clientSize.Width ;
            this.StartButton.Height = Config.StartButtonHeight;
            this.StartButton.Location = new Point(0, this.MainCanvas.Bottom);
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
