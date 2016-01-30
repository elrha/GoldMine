using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NativePlayer
{
    public class Template_Native_Loader : IPlayer
    {
        [DllImport("Template_Native.dll")]
        private static extern void InnerInitialize(int myNumber, int totalPlayerCount, int col, int row);

        [DllImport("Template_Native.dll")]
        private static extern int InnerProcess(int myNumber, int[] playerPosition, int[] playerPower, int[] playerStun, int[] mapBlocks);

        private int myNumber;

        public void Dispose()
        {
        }

        public string GetName()
        {
            return "NativePlayer";
        }

        public void Initialize(int myNumber, int totalPlayerCount, int col, int row)
        {
            this.myNumber = myNumber;
            InnerInitialize(myNumber, totalPlayerCount, col, row);
        }

        public Arrow Process(GameInfo gameInfo, BlockType[] mineField)
        {
            return (Arrow)InnerProcess(myNumber, gameInfo.PlayerPosition, gameInfo.PlayerPower, gameInfo.PlayerStun, Array.ConvertAll(mineField, item => (int)item));
        }
    }
}