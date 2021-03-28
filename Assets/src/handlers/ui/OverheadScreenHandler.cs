using UnityEngine;

namespace src.handlers.ui
{
    public class OverheadScreenHandler : ScreenHandler
    {
        protected override void HandleHit(RaycastHit hit)
        {
            print("hit");
        }
    }
}