using System;
using src.services.impl;
using UnityEngine;

namespace src.util
{
    public static class Environment
    {
        /************ In Editor Values ************/
        /*===============================
        *  GameObject Names
        ==============================*/
        public const string OVERHEAD_UI    = "overhead-ui";
        public const string SWARM_MEMBER   = "swarm-member";
        public const string UI_CARD_TEXT   = "card-text";
        public const string UI_CARD_BUTTON = "button-box";
        public const string UI_CARD_BUTTON_TEXT = "button-text";
        public const string UI_CARD_JOINER = "joiner";
        
        /*===============================
        *  GameObject Tags
        ==============================*/
        public const string TAG_CARD_PLACE_HOLDER = "CardPlaceHolder";
        public const string TAG_CARD_TAB          = "CardTab";
        public const string TAG_CURSOR_BASE       = "CursorBase";
        public const string TAG_FLOOR             = "Floor";
        public const string TAG_HEAT_ZONE         = "HeatZone";
        public const string TAG_MAIN_CAMERA       = "MainCamera";
        public const string TAG_PAWN              = "Pawn";
        public const string TAG_SPAWNER           = "Spawner";
        public const string TAG_SWARM             = "SwarmActor";


        /*===============================
         *  UI
         ==============================*/
        public const float UI_CARD_SLIDE_OUT = 1.5f;
        public const float UI_CARD_TAB_WIDTH = 213f;


        /*===============================
         *  Resource Paths
         ==============================*/
        //actors
        public const string RESOURCE_HEAT_ZONE    = "Actors/heat_zone";
        public const string RESOURCE_SWARM_MEMBER = "Actors/swarm_member";
        //cards
        public const string RESOURCE_CARD_ROOT = "UI/Cards";
        public const string RESOURCE_CARD_BEACON  = "UI/Cards/card-beacon";
        public const string RESOURCE_CARD_EYE  = "UI/Cards/card-eye";
        //cursors
        public const string RESOURCE_CURSOR_BEACON = "UI/Cursors/cursor-beacon";
        public const string RESOURCE_CURSOR_EYE = "UI/Cursors/cursor-eye";


        /*===============================
         *  Navigation
         ==============================*/
        public const float SPEED_PAWN_IDLE   = 3.5f;
        public const float SPEED_PAWN        = 4.5f;
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
        public const int   IDLE_RANGE      = 3;

        /*===============================
         *  UI & UX
         ==============================*/
        public const float UI_BUTTON_FADE                 = 0.75f;
        public const float UI_OVERHEAD_SELECTION_INTERVAL = 0.3f;

        /*===============================
         *  Swarm - Fields
         ==============================*/
        public const int SWARM_MAX_ATTACKERS       = 3;
        public const int SWARM_MAX_LOCATE_ATTEMPTS = 5;

        public const float SWARM_VISION_RANGE = 4f;

        /*===============================
         *  Swarm - Spawning
         ==============================*/
        public const float SPAWN_DELAY_LOWER    = 0.5f;
        public const float SPAWN_DELAY_UPPER    = 4f;
        public const float SPAWN_MARGIN         = 10f;
        public const float SPAWN_INTERVAL_LOWER = 2f;
        public const float SPAWN_INTERVAL_UPPER = 5f;

        /*===============================
         *  Camera
         ==============================*/
        public const float CAMERA_SMOOTH_DIST = 5f;
        public const int   CAMERA_SMOOTH_TIME = 10000;
        public const int   HEAT_ZONE_DELAY    = 5;

        /*===============================
         *  Loots
         ==============================*/
        public const float LOOT_DROP_RATE = 0.333f;

        /*===============================
        *  Services
        ==============================*/
        public static readonly CardService   CardService   = new CardService();
        public static readonly GameService   GameService   = new GameService();
        public static readonly LootService   LootService   = new LootService();
        public static readonly PawnService   PawnService   = new PawnService();
        public static readonly PlayerService PlayerService = new PlayerService();
        public static readonly SwarmService  SwarmService  = new SwarmService();
        public static readonly WaveService   WaveService   = new WaveService();


        /*===============================
         *  Tags
         ==============================*/
        public static readonly string[] PoiTags = {"Pawn"};

        public static void Init()
        {
            CardService.Init();
            GameService.Init();
            LootService.Init();
            PawnService.Init();
            PlayerService.Init();
            SwarmService.Init();
            WaveService.Init();
        }

        /*===============================
         *  Prototypes
         ==============================*/
        public static GameObject GetSwarmProtoype()
        {
            return Resources.Load(RESOURCE_SWARM_MEMBER) as GameObject;
        }
        
        /************ Logging ************/
        public static void Log(Type t, string message)
        {
           Debug.Log("[" + t + "] " + message);
        }
    }
}