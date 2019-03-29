using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PuyoPuyo.Toolbox
{
    public class InputTimer
    {
        public Timer Timer { get; }
        public bool Usable { get; set; }
        public InputTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 80;
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) => InputActivation(sender, e);
            Timer = timer;

            Usable = true;
        }

        private void InputActivation(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("toto");
            Usable = true;
        }
    }
}
