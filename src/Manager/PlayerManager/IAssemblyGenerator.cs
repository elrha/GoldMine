using JSPlayer;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines.Manager.PlayerManager
{
    public interface IAssemblyGenerator
    {
        IPlayer New();
    }

    class JSAssemblyGenerator : IAssemblyGenerator
    {
        private string fileFullPath;

        public JSAssemblyGenerator(string fileFullPath)
        {
            this.fileFullPath = fileFullPath;
        }

        public IPlayer New()
        {
            return new IJSPlayer(fileFullPath, Log.Logger);
        }
    }

    public class NormalAssemblyGenerator : IAssemblyGenerator
    {
        private Type playerType;

        public NormalAssemblyGenerator(Type playerType)
        {
            this.playerType = playerType;
        }

        public IPlayer New()
        {
            return Activator.CreateInstance(playerType) as IPlayer;
        }
    }
}
