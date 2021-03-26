using UnityEngine;

namespace src.button.impl
{
    public class NoHandler : ButtonHandler
    {
        public override void OnPointerDownDelegate()
        {
            Application.Quit();
        }
    }
}