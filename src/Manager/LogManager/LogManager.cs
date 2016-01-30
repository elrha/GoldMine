using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines.Manager.LogManager
{
    class LogManager
    {
        private static LogManager instance = new LogManager();

        public static LogManager Instance { get { return LogManager.instance; } }

        private LogManager()
        { 

        }

        public void WriteLog(string Message)
        { 

        }
    }
}
