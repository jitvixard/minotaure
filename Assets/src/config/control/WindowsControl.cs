using UnityEngine;

namespace src.config.control
{
    public class WindowsControl : ControlConfig
    {
        public override bool OnClick()
        {
            throw new System.NotImplementedException();
        }

        public override bool OnHold()
        {
            throw new System.NotImplementedException();
        }

        public override bool OnRelease()
        {
            throw new System.NotImplementedException();
        }

        public override Vector3 InputPosition()
        {
            return Input.mousePosition;
        }
    }
}