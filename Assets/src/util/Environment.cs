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
        //pawn
        public const string PAWN_LOAD_INDICATOR   = "loading-indicator";
        public const string PAWN_POSSESSION_INDICATOR   = "possession-indicator";
        //ui
        public const string UI_CARD_TEXT        = "card-text";
        public const string UI_CARD_BUTTON      = "button-box";
        public const string UI_CARD_BUTTON_TEXT = "button-text";
        public const string UI_CARD_JOINER      = "joiner";
        public const string UI_KNOT             = "knot";
        public const string UI_HEALTH_INDICATOR = "health-indicator";
        
        
        
        /*===============================
        *  GameObject Tags
        ==============================*/
        public const string TAG_BEACON            = "Beacon";
        public const string TAG_BUILDER           = "Builder";
        public const string TAG_CARD_PLACE_HOLDER = "CardPlaceHolder";
        public const string TAG_CARD_TAB          = "CardTab";
        public const string TAG_CURSOR_BASE       = "CursorBase";
        public const string TAG_FLOOR             = "Floor";
        public const string TAG_MAIN_CAMERA       = "MainCamera";
        public const string TAG_PAWN              = "Pawn";
        public const string TAG_SPAWNER           = "Spawner";
        public const string TAG_SWARM             = "SwarmActor";
        public const string TAG_TOWER             = "Tower";
        //collection
        public static string[] noOverlapTags = 
        {
         TAG_BEACON,
         TAG_BUILDER,
         TAG_SWARM,
         TAG_TOWER
        };

        
        
        /*===============================
         *  UI & UX
         ==============================*/
        //button
        public const float UI_BUTTON_FADE    = 0.75f;
        //card
        public const float UI_CARD_SLIDE_OUT = 1.5f;
        public const float UI_CARD_TAB_WIDTH = 213f;
        //overhead
        public const float UI_OVERHEAD_HEAL_TIME          = 0.3f;
        public const float UI_OVERHEAD_SELECTION_INTERVAL = 0.3f;
        //FX
        public const float FX_BLAST_FORCE     = 10f;
        public const float FX_CAMERA_ROTATION = 30f;
        public const float FX_SPLATTER_TIME   = 5f;



        /*===============================
         *  Resource Paths
         ==============================*/
        //actors
        public const string RESOURCE_BUILDER      = "Actors/builder";
        public const string RESOURCE_HEAT_ZONE    = "Actors/heat_zone";
        public const string RESOURCE_SWARM_MEMBER = "Actors/swarm_member";
        //cards
        public const string RESOURCE_CARD_ROOT = "UI/Cards";
        public const string RESOURCE_CARD_BEACON  = "UI/Cards/card-beacon";
        public const string RESOURCE_CARD_EYE  = "UI/Cards/card-eye";
        //cursors
        public const string RESOURCE_CURSOR_BEACON = "UI/Cursors/cursor-beacon";
        public const string RESOURCE_CURSOR_EYE = "UI/Cursors/cursor-eye";
        //shot
        public const string RESOURCE_EXPLOSION = "FX/explosion";
        public const string RESOURCE_SHOT      = "FX/shot";
        public const string RESOURCE_SPLATTER  = "FX/splatter";
        //building
        public const string RESOURCE_BEACON   = "Building/beacon";
        public const string RESOURCE_BUILDING = "Building/tower";


        /*===============================
         *  Navigation
         ==============================*/
        public const float SPEED_PAWN_IDLE   = 3.5f;
        public const float SPEED_PAWN        = 4.5f;
        public const float STOPPING_DISTANCE = 0.1f;

        
        
        /*===============================
         *  Builder
         ==============================*/
        public const int   BUILD_COST              = 450;
        public const float BUILDER_FLOAT_DISTANCE  = 18f;
        public const float BUILDER_FLOAT_TIME      = 2.5f;
        public const float BUILDER_UNLOAD_DISTANCE = 9f;
        public const float BUILDER_UNLOAD_TIME     = 1f;
        
        
        
        /*===============================
         *  Combat
         ==============================*/
        public const float COMBAT_ATTACK_RANGE = 1f;
        public const float COMBAT_LOAD_TIME    = 2f;
        //projectile
        public const float COMBAT_PROJECTILE_SPEED = 10f;
        public const float COMBAT_PROJECTILE_LIFE  = 7.5f;



        /*===============================
         *  State Machine Information
         ==============================*/
        public const float IDLE_WAIT_LOWER = 0.5f;
        public const float IDLE_WAIT_UPPER = 2.5f;
        public const int   IDLE_RANGE      = 3;

        
        
        /*===============================
         *  Swarm - Fields
         ==============================*/
        public const int   SWARM_MAX_ATTACKERS       = 3;
        public const int   SWARM_MAX_LOCATE_ATTEMPTS = 5;
        public const float SWARM_VISION_RANGE        = 4f;
        public const float SWARM_ATTACK_DELAY        = 1f;
        public const float SWARM_ATTACK_SPEED        = 0.5f;
        public const float SWARM_ATTACK_JAB_DISTANCE = 1f;
        public const float SWARM_ATTACKING_RANGE     = 2f;

        public const float SWARM_WAVE_MULTIPLIER_MAX = 1.5f;
        public const float SWARM_WAVE_MULTIPLIER_MIN = 1.1f;
        public const float SWARM_WAVE_PLAYER_MAX     = 2f / 3f;

        
        
        /*===============================
         *  Swarm - Spawning
         ==============================*/
        public const float SWARM_SPAWN_FAR    = 65f;
        public const float SWARM_SPAWN_NEAR   = 50f;
        public const float SWARM_SPAWN_RADIUS = 15f;
        
        
        
        public const float SPAWN_DELAY_LOWER    = 0.5f;
        public const float SPAWN_DELAY_UPPER    = 4f;
        public const float SPAWN_MARGIN         = 10f;
        public const float SPAWN_INTERVAL_LOWER = 2f;
        public const float SPAWN_INTERVAL_UPPER = 5f;
        
        

        /*===============================
         *  Camera
         ==============================*/
        public const float CAMERA_SMOOTH_DIST = 5f;
        public const float   CAMERA_SMOOTH_TIME = 100f;
        public const int   HEAT_ZONE_DELAY    = 5;
        
        

        /*===============================
         *  Loots
         ==============================*/
        public const float LOOT_DROP_RATE = 0.333f;
        
        

        /*===============================
        *  Services
        ==============================*/
        public static readonly BuilderService BuilderService = new BuilderService();
        public static readonly CardService    CardService    = new CardService();
        public static readonly GameService    GameService    = new GameService();
        public static readonly LootService    LootService    = new LootService();
        public static readonly PawnService    PawnService    = new PawnService();
        public static readonly PlayerService  PlayerService  = new PlayerService();
        public static readonly SwarmService   SwarmService   = new SwarmService();
        public static readonly WaveService    WaveService    = new WaveService();
        
        
        
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

        /************ Logging ************/
        public static void Log(Type t, string message)
        {
           Debug.Log("[" + t + "] " + message);
        }
    }
}