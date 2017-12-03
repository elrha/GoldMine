using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerInterface
{
    public struct GameInfo
    {
        public int[] PlayerPosition { get; set; }
        public int[] PlayerPower { get; set; }
        public int[] PlayerStun { get; set; }
    }

    public enum BlockType
    {
        ITEM_9 = -1009,
        ITEM_8 = -1008,
        ITEM_7 = -1007,
        ITEM_6 = -1006,
        ITEM_5 = -1005,
        ITEM_4 = -1004,
        ITEM_3 = -1003,
        ITEM_2 = -1002,
        ITEM_1 = -1001,
        GEM_5 = -5,
        GEM_3 = -3,
        GEM_2 = -2,
        GEM_1 = -1,
        NONE = 0,
        ROCK_1 = 1,
        ROCK_2 = 2,
        ROCK_3 = 3,
        ROCK_4 = 4,
        ROCK_5 = 5,
        ROCK_6 = 6,
        ROCK_7 = 7,
        ROCK_8 = 8,
        ROCK_9 = 9,
        ROCK_10 = 10,
        ROCK_11 = 11,
        ROCK_12 = 12,
    };

    public enum Arrow
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }
}
