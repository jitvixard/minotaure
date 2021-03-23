using src.services.impl;
using UnityEngine;

namespace src.util
{
 public static class Environment
 {
        /*===============================
        *  Services
        ==============================*/
        public static readonly GameService GameService = new GameService();
        public static readonly PawnService PawnService = new PawnService();
        public static readonly PlayerService PlayerService = new PlayerService();
        public static readonly SwarmService SwarmService = new SwarmService();
        public static readonly WaveService WaveService = new WaveService();

        public static void Init()
        {
            GameService.Init(); 
            PawnService.Init();
            PlayerService.Init();
            SwarmService.Init();
            WaveService.Init();
        }
        


        /*===============================
        *  GameObject Names
        ==============================*/
        public const string OVERHEAD_UI = "overhead_ui";
        public const string SWARM_MEMBER = "swarm_member";

        /*===============================
         *  Tags
         ==============================*/
        public static readonly string[] PoiTags = {"Pawn"};
        public const string TAG_FLOOR = "Floor";
        public const string TAG_HEAT_ZONE = "HeatZone";
        public const string TAG_MAIN_CAMERA = "MainCamera";
        public const string TAG_PAWN = "Pawn";
        public const string TAG_SPAWNER = "Spawner";
        public const string TAG_SWARM = "SwarmActor";

        /*===============================
         *  Navigation
         ==============================*/
        public const float SPEED_PAWN_IDLE = 3.5f;
        public const float SPEED_PAWN = 4.5f;
        public const float STOPPING_DISTANCE = 0.1f;
        
        /*===============================
         *  Combat
         ==============================*/
        public const float ATTACK_RANGE = 1f;

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
        
        /*===============================
         *  Swarm
         ==============================*/
        public const float SPAWN_DELAY_LOWER = 0.5f;
        public const float SPAWN_DELAY_UPPER = 4f;
        public const float SPAWN_MARGIN = 10f;
        public const int SWARM_MAX_ATTACKERS = 3;
        public const int SWARM_MAX_LOCATE_ATTEMPTS = 5;
        public const float SWARM_VISION_RANGE = 4f;
        
        /*===============================
         *  SMOOTHING
         ==============================*/
        public const float CAMERA_SMOOTH_DIST = 5f;
        public const int CAMERA_SMOOTH_TIME = 10000;
        public const int HEAT_ZONE_DELAY = 5000;

        /*===============================
         *  Resource Paths
         ==============================*/
        public const string RESOURCE_HEAT_ZONE = "Actors/heat_zone";
        public const string RESOURCE_SWARM_MEMBER = "Actors/swarm_member";
        
        /*===============================
         *  Prototypes
         ==============================*/
        public static GameObject GetSwarmProtoype()
        {
         return Resources.Load(RESOURCE_SWARM_MEMBER) as GameObject;
        } 
    }
}