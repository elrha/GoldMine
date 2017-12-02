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

            var rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var startPosition = (new List<int>() { 0, Config.BlockCol - 1, Config.BlockCol * (Config.BlockRow - 1), Config.BlockCol * Config.BlockRow - 1}).OrderBy(item => rand.Next()).ToList();

            var contextContainer = new ContextContainer() {
                MainCtx = new MainContext(Config.BlockCol, Config.BlockRow, MapHelper.CreateField(Config.BlockCol, Config.BlockRow, startPosition)),
                PlayerCtxList = renderer.CreatePlayerAsm(Config.BlockCol, Config.BlockRow).Select(item => new PlayerContext()
                {
                    PlayerAsm = item.Value,
                    Name = item.Value.GetName(),
                    Number = item.Key,
                    Score = 0,
                    Command = Arrow.UP,
                    LastPos = startPosition[item.Key],
                    Power = Config.DefaultPower,
                    Stun = 0
                }).ToList()
            };
            
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
                if (!contextContainer.MainCtx.IsFinish())
                {
                    var orderedCollection = contextContainer.PlayerCtxList.OrderBy(item => item.Number);
                    var clonedField = contextContainer.MainCtx.CloneGameFields();

                    var processTarget = contextContainer.PlayerCtxList.Where(item => item.Stun <= 0);
                    var nonProcessTarget = contextContainer.PlayerCtxList.Where(item => item.Stun > 0);
                    processTarget.Where(item => item.Stun <= 0).AsParallel().ForAll(item => 
                    { 
                        try 
                        { 
                            item.Command = item.PlayerAsm.Process(new GameInfo() 
                            {
                                PlayerPosition = orderedCollection.Select(innerItem => innerItem.LastPos).ToArray(),
                                PlayerPower = contextContainer.PlayerCtxList.Select(InnerItem => InnerItem.Power).ToArray(),
                                PlayerStun = contextContainer.PlayerCtxList.Select(InnerItem => InnerItem.Stun).ToArray()
                            }, clonedField); 
                        } catch { } 
                    });

                    var actions = new List<Action>();
                    var renderContexts = new List<PlayerRenderContext>();
                    foreach(var processPlayerContext in processTarget)
                        this.processCommandTurn(contextContainer, processPlayerContext, renderContexts, actions);
                    foreach (var processPlayerContext in nonProcessTarget)
                        this.processStunTurn(processPlayerContext, renderContexts);
                    foreach (var action in actions)
                        action();
                    
                    this.renderer.PushData(
                        new MainRenderContext(
                            contextContainer.MainCtx.CloneGameFields(),
                            renderContexts));
                }
                else
                {
                    this.renderer.PushData(
                        new MainRenderContext(
                            contextContainer.MainCtx.CloneGameFields(),
                            this.processFinishRenderContext(contextContainer)));
                }
            }
        }

        private List<PlayerRenderContext> processFinishRenderContext(ContextContainer contextContainer)
        {
            var renderContexts = new List<PlayerRenderContext>();

            var maxScore = contextContainer.PlayerCtxList.Max(item => item.Score);
            foreach (var playerContext in contextContainer.PlayerCtxList)
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

        private void processCommandTurn(ContextContainer contextContainer, PlayerContext playerContext, List<PlayerRenderContext> renderContexts, List<Action> actions)
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

        private PlayerRenderContext processRenderContext(ContextContainer contextContainer, int playerIndex, List<Action> actions, int targetPos, Motion motion)
        {
            var targetFieldValue = (int)contextContainer.MainCtx.GetFieldType(targetPos);
            var targetPlayerContext = contextContainer.PlayerCtxList[playerIndex];

            if (targetFieldValue > 0)
            {
                actions.Add(contextContainer.GetDigAction(targetPos, -1 * targetPlayerContext.Power));
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
                            foreach (var otherPlayerContext in contextContainer.PlayerCtxList.Where(item => item.Number != playerIndex))
                                otherPlayerContext.Stun += Config.StunTurnLV3;
                            break;
                        case (int)BlockType.ITEM_5:
                            foreach (var otherPlayerContext in contextContainer.PlayerCtxList.Where(item => item.Number != playerIndex))
                                otherPlayerContext.Stun += Config.StunTurnLV2;
                            break;
                        case (int)BlockType.ITEM_4:
                            foreach (var otherPlayerContext in contextContainer.PlayerCtxList.Where(item => item.Number != playerIndex))
                                otherPlayerContext.Stun += Config.StunTurnLV1;
                            break;
                        case (int)BlockType.ITEM_3:
                            actions.Add(contextContainer.GetLandFillAction(playerIndex, 5));
                            break;
                        case (int)BlockType.ITEM_2:
                            actions.Add(contextContainer.GetLandFillAction(playerIndex, 3));
                            break;
                        case (int)BlockType.ITEM_1:
                            actions.Add(contextContainer.GetLandFillAction(playerIndex, 2));
                            break;
                        default:
                            break;
                    }

                    actions.Add(contextContainer.GetClearAction(targetPos));
                }                    
                else if (targetFieldValue < 0)
                {
                    targetPlayerContext.Score -= targetFieldValue;
                    actions.Add(contextContainer.GetClearAction(targetPos));
                }

                targetPlayerContext.LastPos = targetPos;
                return new PlayerRenderContext(targetPlayerContext, motion);
            }
        }
    }
}
