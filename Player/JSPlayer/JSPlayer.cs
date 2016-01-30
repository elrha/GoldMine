using log4net;
using Microsoft.ClearScript.V8;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSPlayer
{
    public class IJSPlayer : IPlayer
    {
        private ILog iLog;
        private V8ScriptEngine engin;

        public IJSPlayer(string fileFullPath, ILog iLog)
        {
            this.iLog = iLog;
            engin = new V8ScriptEngine();
            engin.Execute(File.ReadAllText(fileFullPath));
            engin.AddHostObject("Console", iLog);
        }

        public void Dispose()
        {
            if (engin != null) engin.Dispose();
        }

        public string GetName()
        {
            return engin.Script.GetName();
        }

        public void Initialize(int myNumber, int totalPlayerCount, int col, int row)
        {
            engin.Script.Initialize(myNumber, totalPlayerCount, col, row);
        }

        public Arrow Process(GameInfo gameInfo, BlockType[] mineField)
        {
            try
            {
                return (Arrow)engin.Script.Process(gameInfo.PlayerPosition, gameInfo.PlayerPower, gameInfo.PlayerStun, mineField);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
