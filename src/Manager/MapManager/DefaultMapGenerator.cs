﻿using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines.Manager.MapManager
{
    class DefaultMapGenerator
    {
        public static BlockType[] HeximalDevision(int col, int row)
        {
            var totalCount = col * row;
            var ret = new BlockType[totalCount];
            var readyIndex = new HashSet<int>();
            var rand = new Random(DateTime.UtcNow.Millisecond);

            for (int i = 0; i < totalCount; i++)
                ret[i] = BlockType.ROCK_1;

            int width = col / 4;
            int height = row / 4;

            var toolArea = new[] { new[] { 0, 1 }, new[] { 0, 2 }, new[] { 1, 0 }, new[] { 1, 3 }, new[] { 2, 0 }, new[] { 2, 3 }, new[] { 3, 1 }, new[] { 3, 2 } };

            foreach (var areaHW in toolArea)
            {
                int wStart = width * areaHW[1];
                int hStart = height * areaHW[0];

                var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                ret[targetIndex] = BlockType.ITEM_7;
                readyIndex.Add(targetIndex);
            }

            for (int h = 0; h < 4; h++)
            {
                for (int w = 0; w < 4; w++)
                {
                    int wStart = width * w;
                    int hStart = height * h;

                    int blockCount = 1;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_5;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 2;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_3;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 3;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_2;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 4;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_1;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    for (int rockIndex = 6; rockIndex-- > 0;)
                    {
                        blockCount = (width * height) / 10;
                        while (blockCount > 0)
                        {
                            var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                            if (readyIndex.Contains(targetIndex)) continue;

                            ret[targetIndex] = (BlockType)(rockIndex * (rand.Next(2) + 1));
                            readyIndex.Add(targetIndex);
                            blockCount--;
                        }
                    }
                }
            }

            return ret;
        }

        public static BlockType[] RectPattern1(int col, int row)
        {
            var totalCount = col * row;
            var ret = new BlockType[totalCount];
            var readyIndex = new HashSet<int>();
            var rand = new Random(DateTime.UtcNow.Millisecond);

            for (int i = 0; i < totalCount; i++)
                ret[i] = BlockType.ROCK_1;

            int wCurrent = 1;
            int hCurrent = 0;

            while (true)
            {
                for (int h = hCurrent; h < row - hCurrent; h++)
                {
                    ret[h * col + wCurrent] = BlockType.ROCK_12;
                    ret[h * col + (col - (wCurrent + 1))] = BlockType.ROCK_12;
                    readyIndex.Add(h * col + wCurrent);
                    readyIndex.Add(h * col + (col - (wCurrent + 1)));
                }

                hCurrent += 2;

                for (int w = wCurrent; w < col - wCurrent; w++)
                {
                    ret[hCurrent * col + w] = BlockType.ROCK_12;
                    ret[(row - (hCurrent + 1)) * col + w] = BlockType.ROCK_12;
                    readyIndex.Add(hCurrent * col + w);
                    readyIndex.Add((row - (hCurrent + 1)) * col + w);
                }

                wCurrent += 3;

                if ((wCurrent >= (col / 2)) || (hCurrent >= (row / 2)))
                {
                    var ww = col / 2 - 1;
                    var hh = row / 2 - 1;
                    for (int h = hh; h < row - hh; h++)
                    {
                        for (int w = ww; w < col - ww; w++)
                        {
                            var targetIndex = h * col + w;
                            ret[targetIndex] = BlockType.GEM_5;
                            if (!readyIndex.Contains(targetIndex)) readyIndex.Add(targetIndex);
                        }
                    }

                    break;
                }
            }

            int width = col / 4;
            int height = row / 4;

            var tool1Area = new[] { new[] { 0, 1 }, new[] { 0, 2 }, new[] { 3, 1 }, new[] { 3, 2 } };
            var tool2Area = new[] { new[] { 1, 0 }, new[] { 1, 3 }, new[] { 2, 0 }, new[] { 2, 3 } };

            foreach (var areaHW in tool1Area)
            {
                int wStart = width * areaHW[1];
                int hStart = height * areaHW[0];

                while (true)
                {
                    var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                    if (!readyIndex.Contains(targetIndex))
                    {
                        ret[targetIndex] = BlockType.ITEM_7;
                        readyIndex.Add(targetIndex);
                        break;
                    }
                }
            }

            foreach (var areaHW in tool2Area)
            {
                int wStart = width * areaHW[1];
                int hStart = height * areaHW[0];

                while (true)
                {
                    var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                    if (!readyIndex.Contains(targetIndex))
                    {
                        ret[targetIndex] = BlockType.ITEM_8;
                        readyIndex.Add(targetIndex);
                        break;
                    }
                }
            }

            for (int h = 0; h < 4; h++)
            {
                for (int w = 0; w < 4; w++)
                {
                    int wStart = width * w;
                    int hStart = height * h;

                    int blockCount = 1;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_5;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 2;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_3;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 2;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_2;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 2;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_1;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }
                }
            }

            return ret;
        }

        public static BlockType[] RoadPattern1(int col, int row)
        {
            var totalCount = col * row;
            var ret = new BlockType[totalCount];
            var readyIndex = new HashSet<int>();
            var rand = new Random(DateTime.UtcNow.Millisecond);

            for (int i = 0; i < totalCount; i++)
                ret[i] = BlockType.NONE;

            var isEven = (row % 2 == 0);
            var halfRow = row / 2;

            int halfStart = isEven ? (row / 2) - 1 : row / 2;
            int halfEnd = isEven ? row / 2 : row / 2 + 2;

            for (int h = halfStart; h <= halfEnd; h++)
            {
                for (int w = 0; w < col; w++)
                {
                    var targetIndex = h * col + w;
                    ret[targetIndex] = BlockType.ROCK_6;
                    readyIndex.Add(targetIndex);
                }
            }

            int halfWidth = col / 2;
            bool togleFlag = false;

            for (int w = 1; w <= halfWidth; w += 2)
            {
                int hs = 0;
                int he = halfStart - 1;
                int hre = halfStart + 1;
                if (togleFlag) { hs++; he++; }
                else hre++;

                for (int h = hs; h < he; h++)
                {
                    var targetIndex = h * col + w;
                    ret[targetIndex] = BlockType.ROCK_6;
                    if (!readyIndex.Contains(targetIndex)) readyIndex.Add(targetIndex);

                    targetIndex = h * col + ((col - w) - 1);
                    ret[targetIndex] = BlockType.ROCK_6;
                    if (!readyIndex.Contains(targetIndex)) readyIndex.Add(targetIndex);
                }

                for (int h = row - 1 - hs; h > hre; h--)
                {
                    var targetIndex = h * col + w;
                    ret[targetIndex] = BlockType.ROCK_6;
                    if (!readyIndex.Contains(targetIndex)) readyIndex.Add(targetIndex);

                    targetIndex = h * col + ((col - w) - 1);
                    ret[targetIndex] = BlockType.ROCK_6;
                    if (!readyIndex.Contains(targetIndex)) readyIndex.Add(targetIndex);
                }

                togleFlag = togleFlag ? false : true;
            }

            int width = col / 4;
            int height = row / 4;

            for (int h = 0; h < 4; h++)
            {
                for (int w = 0; w < 4; w++)
                {
                    int wStart = width * w;
                    int hStart = height * h;

                    int blockCount = 1;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_5;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 2;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_3;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 2;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_2;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }

                    blockCount = 2;
                    while (blockCount > 0)
                    {
                        var targetIndex = (rand.Next(height) + hStart) * col + (rand.Next(width) + wStart);
                        if (readyIndex.Contains(targetIndex)) continue;

                        ret[targetIndex] = BlockType.GEM_1;
                        readyIndex.Add(targetIndex);
                        blockCount--;
                    }
                }
            }

            return ret;
        }
    }
}