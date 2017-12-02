using Mines.Controls;
using Mines.Defines;
using Mines.Helper;
using Mines.Manager.GameManager;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mines.Renderer
{
    class MainRenderer
    {
        private Image[] blockImages;
        private Image[] itemImages;
        private Image[][][] playerImages;

        private Form mainForm;
        private MainControl mainControl;
        private List<PlayerControl> playerControlList;

        private MainRenderContext lastContextContainer;
        private AutoResetEvent outerEvent;
        private AutoResetEvent innerEvent;

        private bool renderFlag;
        private Thread renderThread;

        private Image backGroundImage;
        private Graphics backGroundGraphics;
        private Image animateImage;
        private Graphics animateGraphics;
        
        public MainRenderer(Form mainForm, MainControl mainControl, List<PlayerControl> playerControlList)
        {
            this.blockImages = ImageHelper.BlockImages;
            this.playerImages = ImageHelper.PlayerImages;

            this.mainForm = mainForm;
            this.mainControl = mainControl;
            this.playerControlList = playerControlList;

            this.outerEvent = new AutoResetEvent(false);
            this.innerEvent = new AutoResetEvent(false);

            this.backGroundImage = new Bitmap(Config.BlockCol * Config.BlockPixelSize, Config.BlockRow * Config.BlockPixelSize);
            this.backGroundGraphics = Graphics.FromImage(this.backGroundImage);
            this.animateImage = new Bitmap(Config.BlockCol * Config.BlockPixelSize, Config.BlockRow * Config.BlockPixelSize);
            this.animateGraphics = Graphics.FromImage(this.animateImage);

            mainForm.Resize += this.Resize;
        }

        public void Resize(object sender, EventArgs e)
        {
            var clientSize = mainForm.ClientSize;

            this.mainControl.Resize(clientSize);

            foreach (var playerControl in this.playerControlList)
                playerControl.Resize(clientSize);
        }

        public IEnumerable<KeyValuePair<int, IPlayer>> CreatePlayerAsm(int col, int row)
        {
            return this.playerControlList.Select(item => item.CreatePlayerAsm(playerControlList.Count, col, row));
        }

        public void Start()
        {
            this.Stop();

            this.renderFlag = true;
            this.renderThread = new Thread(this.RenderProc);
            this.renderThread.Start();
        }

        public void Stop()
        {
            if (this.renderFlag)
            {
                this.renderFlag = false;
                this.innerEvent.Set();
                this.renderThread.Join();
            }
        }

        private void RenderProc(object obj)
        {
            var tickHelper = new TickHelper(Config.AnimateCycleTime / Config.AnimateFramePerCycle);
            this.outerEvent.Set();

            while (this.renderFlag)
            {
                this.innerEvent.WaitOne();

                this.UpdateBackGround(this.lastContextContainer.GameField);

                for (int i = 0; i < Config.AnimateFramePerCycle; i++)
                {
                    tickHelper.Set();
                    this.UpdateAnimate(i, this.lastContextContainer.GameField, this.lastContextContainer.PlayerCtx);
                    this.mainControl.Update(this.animateImage);
                    foreach (var playerControl in this.playerControlList)
                        playerControl.UpdateStateImage();
                    tickHelper.Wait();
                }

                for (int i = 0; i < this.lastContextContainer.PlayerCtx.Count(); i++)
                    this.playerControlList[this.lastContextContainer.PlayerCtx[i].Number].UpdateScore(this.lastContextContainer.PlayerCtx[i].Name, this.lastContextContainer.PlayerCtx[i].Score.ToString(), this.lastContextContainer.PlayerCtx[i].Power.ToString(), this.lastContextContainer.PlayerCtx[i].Stun.ToString());
                
                this.outerEvent.Set();
            }
        }

        public void PushData(MainRenderContext contextContainer)
        {
            this.outerEvent.WaitOne();
            this.lastContextContainer = contextContainer;
            this.innerEvent.Set();
        }

        public void UpdateBackGround(BlockType[] gameField)
        {
            for (int h = 0; h < Config.BlockRow; h++)
            {
                for (int w = 0; w < Config.BlockCol; w++)
                {
                    var targetFieldType = (int)gameField[h * Config.BlockCol + w];
                    if (targetFieldType <= (int)BlockType.ITEM_1)
                        targetFieldType -= (int)BlockType.ITEM_9;
                    else
                        targetFieldType -= ((int)BlockType.GEM_5 - 9);

                    this.backGroundGraphics.DrawImage(blockImages[targetFieldType], w * Config.BlockPixelSize, h * Config.BlockPixelSize, Config.BlockPixelSize, Config.BlockPixelSize);
                }
            }
        }

        public void UpdateAnimate(int frameNumber, BlockType[] gameField, List<PlayerRenderContext> playerContext)
        {
            lock (this.animateImage)
                this.animateGraphics.DrawImage(this.backGroundImage, 0, 0, this.animateImage.Width, this.animateImage.Height);

            foreach (var pc in playerContext)
            {
                var x = (pc.LastPos % Config.BlockCol) * Config.BlockPixelSize;
                var y = (pc.LastPos / Config.BlockCol) * Config.BlockPixelSize;
                var motionIndex = (int)Motion.UP;
                var animateIndex = Convert.ToInt32(Math.Round(((double)frameNumber / Config.AnimateFramePerCycle) * Config.AnimateFrameCount - 0.49, 0));

                switch (pc.Motion)
                {
                    case Motion.TURN:
                        motionIndex = 8;
                        break;
                    case Motion.UP:
                        y += (Config.BlockPixelStep * (Config.AnimateFramePerCycle - frameNumber));
                        motionIndex = (int)pc.Motion;
                        break;
                    case Motion.RIGHT:
                        x -= (Config.BlockPixelStep * (Config.AnimateFramePerCycle - frameNumber));
                        motionIndex = (int)pc.Motion;
                        break;
                    case Motion.DOWN:
                        y -= (Config.BlockPixelStep * (Config.AnimateFramePerCycle - frameNumber));
                        motionIndex = (int)pc.Motion;
                        break;
                    case Motion.LEFT:
                        x += (Config.BlockPixelStep * (Config.AnimateFramePerCycle - frameNumber));
                        motionIndex = (int)pc.Motion;
                        break;
                    case Motion.STAND_UP:
                        motionIndex = (int)pc.Motion - (int)Motion.STAND_UP;
                        break;
                    case Motion.STAND_RIGHT:
                        motionIndex = (int)pc.Motion - (int)Motion.STAND_UP;
                        break;
                    case Motion.STAND_DOWN:
                        motionIndex = (int)pc.Motion - (int)Motion.STAND_UP;
                        break;
                    case Motion.STAND_LEFT:
                        motionIndex = (int)pc.Motion - (int)Motion.STAND_UP;
                        break;
                    case Motion.DIG_UP:
                        motionIndex = (int)pc.Motion - (int)Motion.DIG_UP + 4;
                        break;
                    case Motion.DIG_RIGHT:
                        motionIndex = (int)pc.Motion - (int)Motion.DIG_UP + 4;
                        break;
                    case Motion.DIG_DOWN:
                        motionIndex = (int)pc.Motion - (int)Motion.DIG_UP + 4;
                        break;
                    case Motion.DIG_LEFT:
                        motionIndex = (int)pc.Motion - (int)Motion.DIG_UP + 4;
                        break;
                }

                lock (this.animateImage)
                    this.animateGraphics.DrawImage(
                        this.playerImages[pc.Number][motionIndex][animateIndex],
                        x, y, Config.BlockPixelSize, Config.BlockPixelSize);
                
                var image = this.playerControlList[pc.Number].GetBackImage();
                lock (image)
                {
                    using (var graphics = Graphics.FromImage(image))
                    {
                        graphics.DrawImage(this.blockImages[-((int)BlockType.GEM_5 - 9)], 0, 0, image.Width, image.Height);
                        graphics.DrawImage(this.playerImages[pc.Number][motionIndex][animateIndex], 0, 0, image.Width, image.Height);
                    }
                }
            }
        }
    }
}