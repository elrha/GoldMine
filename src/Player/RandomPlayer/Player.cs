using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewPlayer
{
    public class Player : IPlayer
    {
        private int myNumber;
        private int blockCol;
        private int blockRow;

        public void Dispose()
        {
        }

        public string GetName()
        {
            return "Mario";
        }

        public void Initialize(int myNumber, int totalPlayerCount, int col, int row)
        {
            this.myNumber = myNumber;
            this.blockCol = col;
            this.blockRow = row;
        }

        public Arrow Process(GameInfo gameInfo, BlockType[] mineField)
        {
            return (Arrow)(new Random(DateTime.UtcNow.Millisecond + myNumber)).Next(4);
        }
    }
}