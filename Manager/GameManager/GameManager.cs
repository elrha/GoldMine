using Mines.Controls;
using Mines.Defines;
using Mines.Helper;
using Mines.Renderer;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mines.Manager.GameManager
{
    class GameManager
    {
        private bool playFlag;
        private Thread mainThread;
        private MainRenderer renderer;

        public GameManager(MainRenderer renderer)
        {
            this.renderer = renderer;
            this.playFlag = false;
        }

        public void StartGame()
        {
            this.StopGame();
            
            var contextContainer = new ContextContainer();

            // note : fill MainContext
            var rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var startPosition = (new List<int>() { 0, Config.BlockCol - 1, Config.BlockCol * (Config.BlockRow - 1), Config.BlockCol * Config.BlockRow - 1}).OrderBy(item => rand.Next()).ToList();
            contextContainer.MainContext.IsFinish = false;
            contextContainer.MainContext.FieldCol = Config.BlockCol;
            contextContainer.MainContext.FieldRow = Config.BlockRow;
            contextContainer.MainContext.GameField = MapHelper.CreateField(Config.BlockCol, Config.BlockRow, startPosition);

            // note : fill PlayerContext
            contextContainer.PlayerCtx.AddRange(renderer.CreatePlayerAsm(Config.BlockCol, Config.BlockRow).Select(item => new PlayerContext()
            {
                PlayerAsm = item.Value,
                Name = item.Value.GetName(), Number = item.Key, Score = 0,
                Command = Arrow.UP, LastPos = startPosition[item.Key], Power = Config.DefaultPower, Stun = 0
            }));

            this.renderer.Start();

            this.playFlag = true;
            this.mainThread = new Thread(GameProc);
            this.mainThread.Start(contextContainer);
        }

        public void StopGame()
        {
            if (this.playFlag)
            {
                this.playFlag = false;
                this.mainThread.Join();
            }

            this.renderer.Stop();
        }

        private void GameProc(object obj)
        {
            var contextContainer = obj as ContextContainer;

            while (this.playFlag)
            {
                if (!contextContainer.MainContext.IsFinish)
                {
                    this.generateRandomItem(contextContainer);

                    var orderedCollection = contextContainer.PlayerCtx.OrderBy(item => item.Number);
                    var clonedField = contextContainer.MainContext.GameField.ToArray();

                    var processTarget = contextContainer.PlayerCtx.Where(item => item.Stun <= 0);
                    var nonProcessTarget = contextContainer.PlayerCtx.Where(item => item.Stun > 0);
                    processTarget.Where(item => item.Stun <= 0).AsParallel().ForAll(item => 
                    { 
                        try 
                        { 
                            item.Command = item.PlayerAsm.Process(new GameInfo() 
                            {
                                PlayerPosition = orderedCollection.Select(innerItem => innerItem.LastPos).ToArray(),
                                PlayerPower = contextContainer.PlayerCtx.Select(InnerItem => InnerItem.Power).ToArray(),
                                PlayerStun = contextContainer.PlayerCtx.Select(InnerItem => InnerItem.Stun).ToArray()
                            }, clonedField); 
                        } catch { } 
                    });

                    var actions = new List<TurnAction>();
                    var renderContexts = new List<PlayerRenderContext>();
                    foreach(var processPlayerContext in processTarget)
                        this.processCommandTurn(contextContainer, processPlayerContext, renderContexts, actions);
                    foreach (var processPlayerContext in nonProcessTarget)
                        this.processStunTurn(processPlayerContext, renderContexts);
                    foreach (var action in actions)
                        action.Excute();
                    
                    this.renderer.PushData(
                        new MainRenderContext(
                            contextContainer.MainContext.GameField.ToArray(),
                            renderContexts));

                    contextContainer.MainContext.IsFinish = processFinishState(contextContainer.MainContext.GameField);
                }
                else
                {
                    this.renderer.PushData(
                        new MainRenderContext(
                            contextContainer.MainContext.GameField.ToArray(),
                            this.processFinishRenderContext(contextContainer)));
                }
            }
        }

        private List<PlayerRenderContext> processFinishRenderContext(ContextContainer contextContainer)
        {
            var renderContexts = new List<PlayerRenderContext>();

            var maxScore = contextContainer.PlayerCtx.Max(item => item.Score);
            foreach (var playerContext in contextContainer.PlayerCtx)
            {
                if (playerContext.Score == maxScore)
                    renderContexts.Add(new PlayerRenderContext(playerContext, Motion.TURN));
                else
                    renderContexts.Add(new PlayerRenderContext(playerContext, Motion.STAND_DOWN));
            }

            return renderContexts;
        }

        private void processStunTurn(PlayerContext playerContext, List<PlayerRenderContext> renderContexts)
        {
            playerContext.Stun--;
            renderContexts.Add(new PlayerRenderContext(playerContext, Motion.TURN));
        }

        private void processCommandTurn(ContextContainer contextContainer, PlayerContext playerContext, List<PlayerRenderContext> renderContexts, List<TurnAction> actions)
        {
            switch (playerContext.Command)
            {
                case Arrow.UP:
                    if (playerContext.LastPos >= Config.BlockCol)
                        renderContexts.Add(this.processRenderContext(contextContainer, playerContext.Number, actions, playerContext.LastPos - Config.BlockCol, Motion.UP));
                    else
                        renderContexts.Add(new PlayerRenderContext(playerContext, Motion.STAND_UP));
                    break;
                case Arrow.RIGHT:
                    if ((playerContext.LastPos % Config.BlockCol) < (Config.BlockCol - 1))
                        renderContexts.Add(this.processRenderContext(contextContainer, playerContext.Number, actions, playerContext.LastPos + 1, Motion.RIGHT));
                    else
                        renderContexts.Add(new PlayerRenderContext(playerContext, Motion.STAND_RIGHT));
                    break;
                case Arrow.DOWN:
                    if (playerContext.LastPos < Config.BlockCol * (Config.BlockRow - 1))
                        renderContexts.Add(this.processRenderContext(contextContainer, playerContext.Number, actions, playerContext.LastPos + Config.BlockCol, Motion.DOWN));
                    else
                        renderContexts.Add(new PlayerRenderContext(playerContext, Motion.STAND_DOWN));
                    break;
                case Arrow.LEFT:
                    if ((playerContext.LastPos % Config.BlockCol) > 0)
                        renderContexts.Add(this.processRenderContext(contextContainer, playerContext.Number, actions, playerContext.LastPos - 1, Motion.LEFT));
                    else
                        renderContexts.Add(new PlayerRenderContext(playerContext, Motion.STAND_LEFT));
                    break;
            }
        }

        private PlayerRenderContext processRenderContext(ContextContainer contextContainer, int playerIndex, List<TurnAction> actions, int targetPos, Motion motion)
        {
            var targetFieldValue = (int)contextContainer.MainContext.GameField[targetPos];
            var targetPlayerContext = contextContainer.PlayerCtx[playerIndex];

            if (targetFieldValue > 0)
            {
                actions.Add(new DigAction(contextContainer, targetPos, -1 * targetPlayerContext.Power));
                return new PlayerRenderContext(targetPlayerContext, (int)motion + Motion.DIG_UP);
            }
            else
            {
                if (targetFieldValue <= (int)BlockType.ITEM_1)
                {
                    switch (targetFieldValue)
                    {
                        case (int)BlockType.ITEM_9:
                            targetPlayerContext.Power += 3;
                            break;
                        case (int)BlockType.ITEM_8:
                            targetPlayerContext.Power += 2;
                            break;
                        case (int)BlockType.ITEM_7:
                            targetPlayerContext.Power += 1;
                            break;
                        case (int)BlockType.ITEM_6:
                            foreach (var otherPlayerContext in contextContainer.PlayerCtx.Where(item => item.Number != playerIndex))
                                otherPlayerContext.Stun += Config.StunTurnLV3;
                            break;
                        case (int)BlockType.ITEM_5:
                            foreach (var otherPlayerContext in contextContainer.PlayerCtx.Where(item => item.Number != playerIndex))
                                otherPlayerContext.Stun += Config.StunTurnLV2;
                            break;
                        case (int)BlockType.ITEM_4:
                            foreach (var otherPlayerContext in contextContainer.PlayerCtx.Where(item => item.Number != playerIndex))
                                otherPlayerContext.Stun += Config.StunTurnLV1;
                            break;
                        case (int)BlockType.ITEM_3:
                            actions.Add(new LandSlideAction(contextContainer, playerIndex, 5));
                            break;
                        case (int)BlockType.ITEM_2:
                            actions.Add(new LandSlideAction(contextContainer, playerIndex, 3));
                            break;
                        case (int)BlockType.ITEM_1:
                            actions.Add(new LandSlideAction(contextContainer, playerIndex, 2));
                            break;
                        default:
                            break;
                    }

                    actions.Add(new ClearAction(contextContainer, targetPos));
                }                    
                else if (targetFieldValue < 0)
                {
                    targetPlayerContext.Score -= targetFieldValue;
                    actions.Add(new ClearAction(contextContainer, targetPos));
                }

                targetPlayerContext.LastPos = targetPos;
                return new PlayerRenderContext(targetPlayerContext, motion);
            }
        }

        private bool processFinishState(BlockType[] fields)
        {
            foreach (var field in fields)
                if (field == BlockType.GEM_5 || field == BlockType.GEM_3 || field == BlockType.GEM_2 || field == BlockType.GEM_1)
                    return false;
            return true;
        }

        private void generateRandomItem(ContextContainer contextContainer)
        {
            var rand = new Random(DateTime.UtcNow.Millisecond);

            if (rand.Next() % 30 == 7)
            {
                var itemNumber = rand.Next((int)BlockType.ITEM_9, (int)BlockType.ITEM_1 + 1);
                var index = rand.Next(contextContainer.MainContext.GameField.Length);

                if (contextContainer.PlayerCtx.Where(item => item.LastPos == index).Count() <= 0)
                    if (contextContainer.MainContext.GameField[index] == BlockType.NONE)
                        contextContainer.MainContext.GameField[index] = (BlockType)itemNumber;
            }
        }
    }
}
