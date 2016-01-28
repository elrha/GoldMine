using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines.Defines
{
    public class Config
    {
        public static readonly int PlayerCount = 4;

        public static readonly double PlayerControlRatio = .1;

        public static readonly int StartButtonHeight = 24;

        public static readonly int AnimateCycleTime = 200;

        public static readonly int AnimateFrameCount = 4;

        public static readonly int AnimateFramePerCycle = 4;

        public static readonly int BlockItemCount = Math.Abs((int)BlockType.ITEM_9);

        public static readonly int BlockGemCount = Math.Abs((int)BlockType.GEM_5);

        public static readonly int BlockPixelSize = 16;

        public static readonly int BlockPixelStep = Convert.ToInt32((double)Config.BlockPixelSize / Config.AnimateFramePerCycle);

        public static readonly int StateImageWidth = 400;

        public static readonly int StateImageHeight = 200;

        public static readonly Color Player1Color = Color.Red;

        public static readonly Color Player2Color = Color.Green;

        public static readonly Color Player3Color = Color.Purple;

        public static readonly Color Player4Color = Color.Blue;

        public static readonly int ConversionColorR = 255;

        public static readonly int ConversionColorG = 0;

        public static readonly int ConversionColorB = 0;

        public static readonly int DefaultPower = 1;

        public static int stunTurnLV1 = 10;
        public static int StunTurnLV1 { get { return Config.stunTurnLV1; } }
        
        public static int stunTurnLV2 = 20;
        public static int StunTurnLV2 { get { return Config.stunTurnLV2; } }
        
        public static int stunTurnLV3 = 30;
        public static int StunTurnLV3 { get { return Config.stunTurnLV3; } }

        private static int blockCol;
        public static int BlockCol { get { return Config.blockCol; } }

        private static int blockRow;
        public static int BlockRow { get { return Config.blockRow; } }

        public static void Initialize(int col, int row)
        {
            Config.blockCol = col;
            Config.blockRow = row;

            var StunSid = Math.Sqrt(Math.Pow((double)col, 2) + Math.Pow((double)row, 2)) + 0.5;
            Config.stunTurnLV1 = Convert.ToInt32(StunSid * .3);
            Config.stunTurnLV2 = Convert.ToInt32(StunSid * .6);
            Config.stunTurnLV3 = Convert.ToInt32(StunSid);
        }
    }
}