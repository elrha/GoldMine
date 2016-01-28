using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines.Manager.GameManager
{
    interface TurnAction
    {
        void Excute();
    }

    class DigAction : TurnAction
    {
        private ContextContainer contextContainer;
        private int targetPos;
        private int power;

        public DigAction(ContextContainer contextContainer, int targetPos, int power)
        {
            this.contextContainer = contextContainer;
            this.targetPos = targetPos;
            this.power = power;
        }

        public void Excute()
        {
            var resultValue = this.contextContainer.MainContext.GameField[this.targetPos] + power;
            if (resultValue < 0) resultValue = 0;
            this.contextContainer.MainContext.GameField[this.targetPos] = resultValue;
        }
    }

    class ClearAction : TurnAction
    {
        private ContextContainer contextContainer;
        private int targetPos;

        public ClearAction(ContextContainer contextContainer, int targetPos)
        {
            this.contextContainer = contextContainer;
            this.targetPos = targetPos;
        }

        public void Excute()
        {
            this.contextContainer.MainContext.GameField[this.targetPos] = BlockType.NONE;
        }
    }

    class LandSlideAction : TurnAction
    {
        private static Random fieldRand = new Random(DateTime.UtcNow.Second);
        private ContextContainer contextContainer;
        private int ignorePlayerIndex;
        private int width;
        
        public LandSlideAction(ContextContainer contextContainer, int ignorePlayerIndex, int width)
        {
            this.contextContainer = contextContainer;
            this.ignorePlayerIndex = ignorePlayerIndex;
            this.width = width;
        }

        public void Excute()
        {
            foreach (var playerContext in this.contextContainer.PlayerCtx.Where(item => item.Number != this.ignorePlayerIndex))
            {
                var fieldCol = this.contextContainer.MainContext.FieldCol;
                var fieldRow = this.contextContainer.MainContext.FieldRow;
                var baseW = playerContext.LastPos % this.contextContainer.MainContext.FieldCol;
                var baseH = playerContext.LastPos / this.contextContainer.MainContext.FieldCol;
                var minW = baseW - this.width; var maxW = baseW + this.width;
                var minH = baseH - this.width; var maxH = baseH + this.width;

                if (minW < 0) minW = 0; if (maxW > this.contextContainer.MainContext.FieldCol) maxW = this.contextContainer.MainContext.FieldCol;
                if (minH < 0) minH = 0; if (maxH > this.contextContainer.MainContext.FieldRow) maxH = this.contextContainer.MainContext.FieldRow;

                for (int w = minW; w < maxW; w++)
                {
                    for (int h = minH; h < maxH; h++)
                    {
                        if(w == baseW && h == baseH) continue;

                        if(this.contextContainer.MainContext.GameField[h * fieldCol + w] == BlockType.NONE)
                            this.contextContainer.MainContext.GameField[h * fieldCol + w] = (BlockType)LandSlideAction.fieldRand.Next((int)BlockType.ROCK_6) + 1;
                    }
                }
            }
        }
    }
}
