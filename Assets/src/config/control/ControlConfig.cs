using UnityEngine;

namespace src.config.control
{
    public abstract class ControlConfig
    {
        public abstract bool OnClick();
        public abstract bool OnHold();
        public abstract bool OnRelease();

        public abstract Vector3 InputPosition();


        public static ControlConfig GetControl()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return new PhoneControl();
                case RuntimePlatform.WindowsPlayer:
                    return new WindowsControl();
                default:
                    return new WindowsControl();
            }
        }
    }
}