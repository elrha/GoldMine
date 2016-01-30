using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerInterface
{
    public interface IPlayer : IDisposable
    {
        string GetName();

        void Initialize(int myNumber, int totalPlayerCount, int col, int row);

        Arrow Process(GameInfo gameInfo, BlockType[] mineField);
    }
}