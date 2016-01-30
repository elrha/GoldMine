using Mines.Manager.LogManager;
using Mines.Manager.PlayerManager;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PerseusCommon.Manager.AssemblyManager
{
    public class AssemblyManager
    {
        #region SingleTon

        private static AssemblyManager instance = new AssemblyManager();

        public static AssemblyManager Instance { get { return AssemblyManager.instance; } }

        #endregion

        #region Assembly Doc

        public string PlayerInfo { private set; get; }

        #endregion

        #region Assembly Container

        private Type playerType = typeof(IPlayer);

        private Dictionary<string, IAssemblyGenerator> players;

        public Dictionary<string, IAssemblyGenerator> Players { get { return players; } }

        #endregion

        private readonly string assemblyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Player");

        private AssemblyManager()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            LoadAssemblyFiles();
        }

        private void LoadAssemblyFiles()
        {
            players = new Dictionary<string, IAssemblyGenerator>();

            if (!Directory.Exists(assemblyFilePath))
                Directory.CreateDirectory(assemblyFilePath);

            foreach (var file in new DirectoryInfo(assemblyFilePath).GetFiles())
            {
                if (string.Compare(file.Extension, ".dll") == 0)
                {
                    try
                    {
                        foreach (var type in Assembly.LoadFile(file.FullName).GetTypes())
                        {
                            if (playerType.IsAssignableFrom(type))
                            {
                                var instance = Activator.CreateInstance(type) as IPlayer;

                                if (instance != null)
                                    players.Add(file.Name + ":" + instance.GetName(), new NormalAssemblyGenerator(type));
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        LogManager.Instance.WriteLog("FileLoadError - [FileName] : " + file.Name + e.Message);
                    }
                }
                else if (string.Compare(file.Extension, ".js") == 0)
                {
                    try
                    {
                        var assemblyGenerator = new JSAssemblyGenerator(file.FullName);
                        players.Add(file.Name + ":" + assemblyGenerator.New().GetName(), assemblyGenerator);
                    }
                    catch (Exception e) { LogManager.Instance.WriteLog("FileLoadError - [FileName] : " + file.Name + e.Message); }
                }
            }
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var searchResult = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, args.Name.Split(',').First() + ".dll", SearchOption.AllDirectories);

            if (searchResult.Count() > 0)
                return Assembly.LoadFrom(searchResult.First());

            return null;
        }
    }
}