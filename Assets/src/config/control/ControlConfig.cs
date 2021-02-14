using UnityEngine;

namespace src.config.control
{
    public abstract class ControlConfig
    {
        
        protected abstract bool Tapping();
        protected abstract bool Holding();

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