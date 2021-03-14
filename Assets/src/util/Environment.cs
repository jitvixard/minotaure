namespace src.util
{
    public class Environment
    {
        public const string PLAYER = "Player";
        public const string TILE_REF = "TileReference";
        public const string OVERHEAD_UI = "overhead_ui";

        public const string TAG_MOVEMENT = "MovementTile";
        public const string TAG_PAWN = "Pawn";

        public const string LEVEL_BASE = "level_base";
        public const string LOOK_AT = "look_point";

        public const float STOPPING_DISTANCE = 0.1f;

        public const int LAYER_FLOOR = 9;

        public const string TILE_NAME_FORMAT = "tile_c{0}_r{1}";

        public const float IDLE_WAIT_LOWER = 0.5f;
        public const float IDLE_WAIT_UPPER = 2.5f;
        public const int IDLE_RANGE = 3;

        public const float UI_OVERHEAD_SELECTION_INTERVAL = 0.3f;
    }
}