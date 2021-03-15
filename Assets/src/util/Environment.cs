namespace src.util
{
    public class Environment
    {
        /*===============================
         *  GameObject Names
         ==============================*/
        public const string OVERHEAD_UI = "overhead_ui";

        /*===============================
         *  Tags
         ==============================*/
        public const string TAG_FLOOR = "Floor";
        public const string TAG_PAWN = "Pawn";

        /*===============================
         *  Navigation
         ==============================*/
        public const float SPEED_PAWN_IDLE = 3.5f;
        public const float SPEED_PAWN = 4.5f;
        public const float STOPPING_DISTANCE = 0.1f;

        /*===============================
         *  Layers
         ==============================*/
        public const int LAYER_FLOOR = 9;

        /*===============================
         *  State Machine Information
         ==============================*/
        public const float IDLE_WAIT_LOWER = 0.5f;
        public const float IDLE_WAIT_UPPER = 2.5f;
        public const int IDLE_RANGE = 3;

        /*===============================
         *  UI & UX
         ==============================*/
        public const float UI_OVERHEAD_SELECTION_INTERVAL = 0.3f;
    }
}