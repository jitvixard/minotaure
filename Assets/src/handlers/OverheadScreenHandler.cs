using UnityEngine;

namespace src.screens
{
    public class OverheadScreenHandler : ScreenHandler
    {
        protected override void HandleHit(RaycastHit hit)
        {
            print("hit");
        }
    }
}