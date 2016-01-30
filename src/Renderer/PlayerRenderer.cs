using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mines.Renderer
{
    class PlayerRenderer
    {
        private PictureBox targetCanvas;
        private Graphics outGraphics;
        private Image image;
        private Graphics imageGraphics;
        private string playerName;
        private Image playerImage;

        //public PlayerRenderer(PictureBox targetCanvas, string playerName, Image playerImage)
        //{
        //    this.targetCanvas = targetCanvas;
        //    this.outGraphics = targetCanvas.CreateGraphics();
        //    this.image = new Bitmap(targetCanvas.Size.Width, targetCanvas.Size.Height);
        //    this.imageGraphics = Graphics.FromImage(this.image);
        //    this.playerName = playerName;
        //    this.playerImage = playerImage;
        //}

        //public void Update(bool own, int score, bool win)
        //{
        //    Brush targetBrush = null;
        //    this.imageGraphics.Clear(Color.White);

        //    if (own)
        //    {
        //        this.imageGraphics.DrawRectangle(new Pen(Color.Black, 5F), 0, 0, targetCanvas.Size.Width, targetCanvas.Size.Height);
        //        targetBrush = Brushes.Black;
        //    }
        //    else
        //    {
        //        targetBrush = Brushes.Gray;
        //    }

        //    this.imageGraphics.DrawString(
        //            playerName,
        //            new Font("Arial Black", 8),
        //            targetBrush,
        //            targetCanvas.Size.Width / 5,
        //            targetCanvas.Size.Height / 10);

        //    this.imageGraphics.DrawString(
        //            score.ToString(),
        //            new Font("Arial Black", 24),
        //            targetBrush,
        //            targetCanvas.Size.Width / 3,
        //            targetCanvas.Size.Height / 5);

        //    this.imageGraphics.DrawImage(
        //            playerImage,
        //            (targetCanvas.Size.Width / 2) - 50,
        //            (targetCanvas.Size.Height / 2) - 50,
        //            100,
        //            100);

        //    if (win)
        //    {
        //        this.imageGraphics.DrawString(
        //                "WINNER!",
        //                new Font("Arial Black", 15),
        //                Brushes.Red,
        //                targetCanvas.Size.Width / 10,
        //                (targetCanvas.Size.Height / 5) * 4);
        //    }

        //    targetCanvas.BeginInvoke(new Action(() =>
        //    {
        //        outGraphics.DrawImage(image, 0, 0, targetCanvas.Size.Width, targetCanvas.Size.Height);
        //    }));
        //}
    }
}
