using UnityEngine;

namespace src.config.control
{
    public class WindowsControl : ControlConfig
    {
        public override bool OnClick()
        {
            return Input.GetMouseButtonDown(0);
        }

        public override bool OnHold()
        {
            return false;
        }

        public override bool OnRelease()
        {
            return false;
        }

        public override Vector3 InputPosition()
        {
            return Input.mousePosition;
        }
    }
}