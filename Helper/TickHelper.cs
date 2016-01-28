using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mines.Helper
{
    class TickHelper
    {
        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long freq;
        private long start;
        private int waitTime;

        public TickHelper(int waitTime)
        {
            this.waitTime = waitTime;
            QueryPerformanceFrequency(out this.freq);
        }

        public void Set()
        {
            QueryPerformanceCounter(out this.start);
        }

        public void Wait()
        {
            long end;
            QueryPerformanceCounter(out end);
            var gap = this.waitTime - Convert.ToInt32(((double)(end - this.start)) / this.freq);
            if (gap > 0) Thread.Sleep(gap);
        }
    }
}
