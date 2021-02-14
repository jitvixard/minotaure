using UnityEngine;

namespace src.config.control
{
    public class PhoneControl : ControlConfig
    {
        public override bool OnClick()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began) return true;
            }

            return false;
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
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) return touch.position;
            //TODO handle a better return type for this scenario
            return new Vector2();
        }
    }
}