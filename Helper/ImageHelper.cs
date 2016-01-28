using Mines.Defines;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines.Helper
{
    class ImageHelper
    {
        private static readonly int motionCount = 9;

        private static Image[] blockImages;
        public static Image[] BlockImages { get { return ImageHelper.blockImages; } }

        private static Image[][][] playerImages;
        public static Image[][][] PlayerImages { get { return ImageHelper.playerImages; } }

        private static bool initialized = ImageHelper.initialize();

        private static bool initialize()
        {
            ImageHelper.initializeBlockImages();
            ImageHelper.initializeAllPlayerImages();
            return true;
        }

        private static void initializeBlockImages()
        {
            ImageHelper.blockImages = new Image[21];
            ImageHelper.blockImages[0] = ImageHelper.convertColor(Properties.Resources.tool, Color.RoyalBlue);
            ImageHelper.blockImages[1] = ImageHelper.convertColor(Properties.Resources.tool, Color.DarkSeaGreen);
            ImageHelper.blockImages[2] = ImageHelper.convertColor(Properties.Resources.tool, Color.DarkGray);
            ImageHelper.blockImages[3] = ImageHelper.convertColor(Properties.Resources.clock, Color.RoyalBlue);
            ImageHelper.blockImages[4] = ImageHelper.convertColor(Properties.Resources.clock, Color.DarkSeaGreen);
            ImageHelper.blockImages[5] = ImageHelper.convertColor(Properties.Resources.clock, Color.DarkGray);
            ImageHelper.blockImages[6] = ImageHelper.convertColor(Properties.Resources.stone, Color.RoyalBlue);
            ImageHelper.blockImages[7] = ImageHelper.convertColor(Properties.Resources.stone, Color.DarkSeaGreen);
            ImageHelper.blockImages[8] = ImageHelper.convertColor(Properties.Resources.stone, Color.DarkGray);
            ImageHelper.blockImages[9] = Properties.Resources.gem5;
            ImageHelper.blockImages[10] = Properties.Resources.Error;
            ImageHelper.blockImages[11] = Properties.Resources.gem3;
            ImageHelper.blockImages[12] = Properties.Resources.gem2;
            ImageHelper.blockImages[13] = Properties.Resources.gem1;
            ImageHelper.blockImages[14] = Properties.Resources.rock_back;
            ImageHelper.blockImages[15] = Properties.Resources.rock1;
            ImageHelper.blockImages[16] = Properties.Resources.rock2;
            ImageHelper.blockImages[17] = Properties.Resources.rock3;
            ImageHelper.blockImages[18] = Properties.Resources.rock4;
            ImageHelper.blockImages[19] = Properties.Resources.rock5;
            ImageHelper.blockImages[20] = Properties.Resources.rock6;
        }

        private static void initializeAllPlayerImages()
        {
            ImageHelper.playerImages = new Image[4][][];
            ImageHelper.playerImages[0] = initializePlayerImages();
            ImageHelper.playerImages[1] = ImageHelper.copyPlayer(ImageHelper.playerImages[0], Config.Player2Color);
            ImageHelper.playerImages[2] = ImageHelper.copyPlayer(ImageHelper.playerImages[0], Config.Player3Color);
            ImageHelper.playerImages[3] = ImageHelper.copyPlayer(ImageHelper.playerImages[0], Config.Player4Color);
        }

        private static Image[][] initializePlayerImages()
        {
            var playerImages = new Image[ImageHelper.motionCount][];
            playerImages[0] = new Image[4]; playerImages[1] = new Image[4]; playerImages[2] = new Image[4]; playerImages[3] = new Image[4];
            playerImages[4] = new Image[4]; playerImages[5] = new Image[4]; playerImages[6] = new Image[4]; playerImages[7] = new Image[4];
            playerImages[8] = new Image[4];

            // note : normal images
            playerImages[2][0] = Properties.Resources.player_run2;
            playerImages[2][2] = ImageHelper.getReverseImage(new Bitmap(playerImages[2][0]));
            playerImages[2][3] = playerImages[2][1] = Properties.Resources.player_run1;

            playerImages[3][0] = ImageHelper.getRotatedImage(playerImages[2][0], 90.0F);
            playerImages[3][2] = ImageHelper.getRotatedImage(playerImages[2][2], 90.0F);
            playerImages[3][3] = playerImages[3][1] = ImageHelper.getRotatedImage(playerImages[2][1], 90.0F);

            playerImages[0][0] = ImageHelper.getRotatedImage(playerImages[2][0], 180.0F);
            playerImages[0][2] = ImageHelper.getRotatedImage(playerImages[2][2], 180.0F);
            playerImages[0][3] = playerImages[0][1] = ImageHelper.getRotatedImage(playerImages[2][1], 180.0F);

            playerImages[1][0] = ImageHelper.getRotatedImage(playerImages[2][0], 270.0F);
            playerImages[1][2] = ImageHelper.getRotatedImage(playerImages[2][2], 270.0F);
            playerImages[1][3] = playerImages[1][1] = ImageHelper.getRotatedImage(playerImages[2][1], 270.0F);

            // note : work images
            playerImages[6][0] = Properties.Resources.player_work1;
            playerImages[6][1] = Properties.Resources.player_work2;
            playerImages[6][2] = Properties.Resources.player_work3;
            playerImages[6][3] = Properties.Resources.player_work4;

            playerImages[7][0] = ImageHelper.getRotatedImage(playerImages[6][0], 90.0F);
            playerImages[7][1] = ImageHelper.getRotatedImage(playerImages[6][1], 90.0F);
            playerImages[7][2] = ImageHelper.getRotatedImage(playerImages[6][2], 90.0F);
            playerImages[7][3] = ImageHelper.getRotatedImage(playerImages[6][3], 90.0F);

            playerImages[4][0] = ImageHelper.getRotatedImage(playerImages[6][0], 180.0F);
            playerImages[4][1] = ImageHelper.getRotatedImage(playerImages[6][1], 180.0F);
            playerImages[4][2] = ImageHelper.getRotatedImage(playerImages[6][2], 180.0F);
            playerImages[4][3] = ImageHelper.getRotatedImage(playerImages[6][3], 180.0F);

            playerImages[5][0] = ImageHelper.getRotatedImage(playerImages[6][0], 270.0F);
            playerImages[5][1] = ImageHelper.getRotatedImage(playerImages[6][1], 270.0F);
            playerImages[5][2] = ImageHelper.getRotatedImage(playerImages[6][2], 270.0F);
            playerImages[5][3] = ImageHelper.getRotatedImage(playerImages[6][3], 270.0F);

            // note : finish images
            playerImages[8][0] = playerImages[2][3];
            playerImages[8][1] = playerImages[3][3];
            playerImages[8][2] = playerImages[0][3];
            playerImages[8][3] = playerImages[1][3];

            return playerImages;
        }

        private static Image[][] copyPlayer(Image[][] player, Color color)
        {
            var playerImages = new Bitmap[ImageHelper.motionCount][];

            for (int i = 0; i < ImageHelper.motionCount; i++)
            {
                playerImages[i] = new Bitmap[4];

                for (int j = 0; j < 4; j++)
                {
                    playerImages[i][j] = ImageHelper.convertColor(player[i][j], color);
                }
            }

            return playerImages;
        }

        private static Bitmap convertColor(Image src, Color dstColor)
        {
            var ret = new Bitmap(src);

            for (int w = 0; w < ret.Width; w++)
            {
                for (int h = 0; h < ret.Height; h++)
                {
                    var pixel = ret.GetPixel(w, h);
                    if (pixel.R == Config.ConversionColorR && pixel.G == Config.ConversionColorG && pixel.B == Config.ConversionColorB)
                        ret.SetPixel(w, h, dstColor);
                }
            }

            return ret;
        }

        private static Bitmap getReverseImage(Bitmap originImage)
        {
            var retImage = new Bitmap(originImage.Width, originImage.Height);

            for (int j = 0; j < retImage.Height; j++)
                for (int i = 0; i < retImage.Width; i++)
                    retImage.SetPixel(i, j, originImage.GetPixel((originImage.Width - 1) - i, j));

            return retImage;
        }

        private static Image getRotatedImage(Image originImage, float angle)
        {
            var ret = new Bitmap(originImage.Width, originImage.Height);
            using (var graphics = Graphics.FromImage(ret))
            {
                graphics.TranslateTransform(originImage.Width / 2, originImage.Height / 2);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-(originImage.Width / 2), -(originImage.Height / 2));
                graphics.DrawImage(originImage, 0, 0);
            }
            return ret;
        }
    }
}