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
        public bool IsFinish { get; set; }
        public int FieldCol { get; set; }
        public int FieldRow { get; set; }
        public BlockType[] GameField { get; set; }
    }

    class ContextContainer
    {
        private List<PlayerContext> playerCtx = new List<PlayerContext>();
        public List<PlayerContext> PlayerCtx { get { return this.playerCtx; } }

        private MainContext mainContext = new MainContext();
        public MainContext MainContext { get { return this.mainContext; } }
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
