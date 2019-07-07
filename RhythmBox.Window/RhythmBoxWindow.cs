using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Platform;

namespace RhythmBox.Window
{
    public class RhythmBoxWindow : RythmBoxResources
    {
        public override void SetHost(GameHost host)
        {
            host.Window.Title = Name;

            base.SetHost(host);
        }
    }
}
