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
    class MapHelper
    {
        //TOOL_3 = -1003,
        //TOOL_2 = -1002,
        //TOOL_1 = -1001,
        //GEM_5 = -5,
        //GEM_3 = -3,
        //GEM_2 = -2,
        //GEM_1 = -1,
        //NONE = 0,
        //ROCK_1 = 1,
        //ROCK_2 = 2,
        //ROCK_3 = 3,
        //ROCK_4 = 4,
        //ROCK_5 = 5,
        //ROCK_6 = 6

        public static BlockType[] CreateField(int col, int row, List<int> startPosition)
        {
            var result = random(col, row);

            foreach (var pos in startPosition)
                result[pos] = BlockType.NONE;
            
            return result;
        }

        private static BlockType[] random(int col, int row)
        {
            var totalCount = col * row;
            var rand = new Random(DateTime.UtcNow.Millisecond);

            var ret = new BlockType[totalCount];
            for (int i = 0; i < totalCount; i++)
            {
                var blockType = (rand.Next() % 12) - Config.BlockGemCount;

                if (blockType == 0) blockType = 1;
                if (blockType == -4) blockType = -1;

                ret[i] = (BlockType)blockType;
            }

            return ret;
        }

        private static BlockType[] continueRock(int col, int row)
        {
            var totalCount = col * row;
            var rand = new Random(DateTime.UtcNow.Millisecond);

            var ret = new BlockType[totalCount];

            for (int i = 0; i < totalCount; i++)
                ret[i] = BlockType.ROCK_1;

            for (int i = 0; i < 50; i++)
            {
                var targetIndex = rand.Next() % totalCount;
                var targetArrow = rand.Next() % 3141592;

                while (true)
                {
                    
                    if (ret[targetIndex] == BlockType.ROCK_1)
                    {
                        ret[targetIndex] = BlockType.ROCK_6;
                        break;
                    }
                }
            }

            for (int i = 0; i < totalCount; i++)
            {
                var blockType = (rand.Next() % 12) - Config.BlockGemCount;

                if (blockType == 0) blockType = 1;
                if (blockType == -4) blockType = -1;

                ret[i] = (BlockType)blockType;
            }

            return ret;
        }
    }
}