using System.Timers;

namespace PuyoPuyo.Toolbox
{
    /// <summary>
    /// Timer of input to manage the activation
    /// </summary>
    public class InputTimer
    {
        public Timer Timer { get; }
        public bool Usable { get; set; }
        public InputTimer()
        {
            Timer timer = new Timer();
            timer.Interval = Consts.INPUT_TIMERTICK;
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) => InputActivation(sender, e);
            Timer = timer;

            Usable = true;
        }

        /// <summary>
        /// Reactivate the input after the time elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputActivation(object sender, ElapsedEventArgs e)
        {
            Usable = true;
        }
    }
}
