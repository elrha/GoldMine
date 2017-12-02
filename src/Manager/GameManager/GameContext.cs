using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines.Manager.GameManager
{
    class PlayerContext
    {
        // note : default
        public IPlayer PlayerAsm { get; set; }
        
        // note : player info
        public string Name { get; set; }
        public int Number { get; set; }   
        public int Score { get; set; }

        // note : player state
        public Arrow Command { get; set; }
        public int LastPos { get; set; }
        public int Power { get; set; }
        public int Stun { get; set; }
    }

    class MainContext
    {
        private int col;
        private int row;
        private BlockType[] gameField;
        private int gemCount;
        
        public MainContext(int col, int row, BlockType[] gameField)
        {
            this.col = col;
            this.row = row;
            this.gameField = gameField;
            this.gemCount = this.gameField.Where(item => item == BlockType.GEM_1 || item == BlockType.GEM_2 || item == BlockType.GEM_3 || item == BlockType.GEM_5).Count();
        }

        public bool IsFinish()
        {
            return this.gemCount <= 0;
        }

        public int GetCol()
        {
            return this.col;
        }

        public int GetRow()
        {
            return this.row;
        }

        public BlockType[] CloneGameFields()
        {
            return this.gameField.ToArray();
        }

        public BlockType GetFieldType(int index)
        {
            return this.gameField[index];
        }

        public void SetFieldType(int index, BlockType newType)
        {
            if (newType == BlockType.GEM_1 || newType == BlockType.GEM_2 || newType == BlockType.GEM_3 || newType == BlockType.GEM_5)
                this.gemCount++;

            var oldType = this.gameField[index];
            if (oldType == BlockType.GEM_1 || oldType == BlockType.GEM_2 || oldType == BlockType.GEM_3 || oldType == BlockType.GEM_5)
                this.gemCount--;

            this.gameField[index] = newType;
        }
    }

    class ContextContainer
    {
        public List<PlayerContext> PlayerCtxList;
        public MainContext MainCtx;

        public Action GetDigAction(int targetPos, int power)
        {
            return () => {
                var resultValue = this.MainCtx.GetFieldType(targetPos) + power;
                this.MainCtx.SetFieldType(targetPos, resultValue < 0 ? 0 : resultValue);
            };
        }

        public Action GetClearAction(int targetPos)
        {
            return () => {
                this.MainCtx.SetFieldType(targetPos, BlockType.NONE);
            };
        }

        public Action GetLandFillAction(int ignorePlayerIndex, int width)
        {
            return () =>
            {
                var fieldRand = new Random(DateTime.UtcNow.Second);
                var col = this.MainCtx.GetCol();
                var row = this.MainCtx.GetRow();
                foreach (var playerContext in this.PlayerCtxList.Where(item => item.Number != ignorePlayerIndex))
                {
                    var baseW = playerContext.LastPos % col;
                    var baseH = playerContext.LastPos / col;
                    var minW = baseW - width; var maxW = baseW + width;
                    var minH = baseH - width; var maxH = baseH + width;

                    if (minW < 0) minW = 0; if (maxW > col) maxW = col;
                    if (minH < 0) minH = 0; if (maxH > row) maxH = row;

                    for (int w = minW; w < maxW; w++)
                    {
                        for (int h = minH; h < maxH; h++)
                        {
                            if (w == baseW && h == baseH) continue;

                            var targetIndex = h * col + w;
                            if (this.MainCtx.GetFieldType(targetIndex) == BlockType.NONE)
                                this.MainCtx.SetFieldType(targetIndex, (BlockType)fieldRand.Next((int)BlockType.ROCK_6) + 1);
                        }
                    }
                }
            };
        }
    }

    public enum Motion
    {
        TURN = -2,
        NONE = -1,
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3,
        STAND_UP = 10,
        STAND_RIGHT = 11,
        STAND_DOWN = 12,
        STAND_LEFT = 13,
        DIG_UP = 20,
        DIG_RIGHT = 21,
        DIG_DOWN = 22,
        DIG_LEFT = 23
    }

    class PlayerRenderContext
    {
        // note : info
        public string Name { get; set; }
        public int Number { get; set; }
        public int Score { get; set; }
        
        // note : state
        public Motion Motion { get; set; }
        public int LastPos { get; set; }
        public int Power { get; set; }
        public int Stun { get; set; }

        public PlayerRenderContext(PlayerContext playerContext, Motion motion)
        {
            this.Name = playerContext.Name;
            this.Number = playerContext.Number;
            this.Score = playerContext.Score;

            this.Motion = motion;
            this.LastPos = playerContext.LastPos;
            this.Power = playerContext.Power;
            this.Stun = playerContext.Stun;
        }
    }

    class MainRenderContext
    {
        public List<PlayerRenderContext> PlayerCtx { get; set; }
        public BlockType[] GameField { get; set; }

        public MainRenderContext(BlockType[] gameField, List<PlayerRenderContext> playerContext)
        {
            this.GameField = gameField;
            this.PlayerCtx = playerContext;
        }
    }
}
