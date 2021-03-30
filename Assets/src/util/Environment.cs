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
        //pawn
        public const string PAWN_LOAD_INDICATOR   = "loading-indicator";
        public const string PAWN_POSSESSION_INDICATOR   = "possession-indicator";
        //ui (desk)
        public const string UI_CARDS_DESK    = "phones-desk";
        public const string UI_BUTTONS_RIGHT = "right-hand-phones";
        public const string UI_POINT_NAMES   = "phone-point";
        
        
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
        public const string TAG_SEED              = "Seed";
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
        public const string RESOURCE_PAWN         = "Actors/pawn-actor";
        public const string RESOURCE_SWARM_MEMBER = "Actors/swarm-actor";
        public const string RESOURCE_SEED = "Actors/seed";
        //cards
        public const string RESOURCE_CARD_BEACON  = "UI/Cards/card-beacon";
        public const string RESOURCE_CARD_EYE  = "UI/Cards/card-eye";
        //shot
        public const string RESOURCE_EXPLOSION          = "FX/explosion";
        public const string RESOURCE_EXPLOSION_BUILDING = "FX/explosion-building";
        public const string RESOURCE_SELECTION_LIGHT    = "FX/selection-light";
        public const string RESOURCE_SHOT               = "FX/shot";
        public const string RESOURCE_SPLATTER           = "FX/splatter";
        //building
        public const string RESOURCE_BEACON   = "Building/beacon";
        public const string RESOURCE_BUILDING = "Building/tower";


        /*===============================
         *  Navigation
         ==============================*/
        public const float STOPPING_DISTANCE = 0.1f;

        
        
        /*===============================
         *  Health
         ==============================*/
        public const int HEALTH_BEACON = 20;
        public const int HEALTH_BUILDER = 50;
        
        
        
        /*===============================
         *  Offsets (for nav)
         ==============================*/
        public const float BEACON_OFFSET   = 3f;
        public const float BUILDING_OFFSET = 9f;
        
        
        
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
         *  Swarm - Fields
         ==============================*/
        public const float SWARM_ATTACK_SPEED        = 0.5f;
        public const float SWARM_ATTACK_JAB_DISTANCE = 1f;
        public const float SWARM_ATTACKING_RANGE     = 2f;

        public const float SWARM_WAVE_MULTIPLIER_MAX = 1.5f;
        public const float SWARM_WAVE_MULTIPLIER_MIN = 1.1f;
        public const float SWARM_WAVE_PLAYER_MAX     = 2f / 3f;

        
        
        /*===============================
         *  Chance
         ==============================*/
        public const float BASE_SEED_SPAWN = 0.4f;
        
        
        /*===============================
         *  Beacon
         ==============================*/
        public const int MAX_BEACONS = 2;
        
        
        /*===============================
         *  Swarm
         ==============================*/
        public const float SWARM_SPAWN_FAR      = 65f;
        public const float SWARM_SPAWN_NEAR     = 50f;
        public const float SWARM_SPAWN_RADIUS   = 15f;
        public const float SPAWN_INTERVAL_LOWER = 2f;
        public const float SPAWN_INTERVAL_UPPER = 5f;
        
        

        /*===============================
         *  Camera
         ==============================*/
        public const float CAMERA_SMOOTH_DIST = 5f;
        public const float   CAMERA_SMOOTH_TIME = 100f;
        
        

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