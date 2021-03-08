using UnityEngine;

namespace src.config
{
    public class Preferences
    {
        public readonly Color accentColor;
        public readonly Color selectionColor;

        public readonly float transitionDuration;

        public Preferences(
            Color accentColor,
            Color selectionColor,
            float transitionDuration)
        {
            this.accentColor = accentColor;
            this.selectionColor = selectionColor;

            this.transitionDuration = transitionDuration;
        }
    }
}