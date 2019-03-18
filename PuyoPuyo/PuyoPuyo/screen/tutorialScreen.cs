
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.screen
{
    class TutorialScreen : MenuScreen
    {

        public TutorialScreen(IServiceProvider serviceProvider, Main main)
                : base(serviceProvider, main)
        {
            main.IsMouseVisible = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
