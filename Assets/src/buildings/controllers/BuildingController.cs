using System;
using System.Collections.Generic;
using System.Linq;
using src.actors.controllers;
using src.interfaces;
using src.level;
using src.services.impl;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using Environment = src.util.Environment;
using Random = UnityEngine.Random;

namespace src.buildings.controllers
{
	public class BuildingController : MonoBehaviour, IDestroyable
	{
		readonly List<GameObject> seeds = new List<GameObject>();
		
		protected BuilderService builderService;
		protected WaveService    waveService;
		
		protected GameObject explosiveEffect;
		protected GameObject seedReference;
		
		protected ProceduralImage healthIndicator;

		protected int   health = 50;
		protected float healthSegment;



		/*===============================
        *  Lifecycle
        ==============================*/
		void Awake()
		{
			builderService = Environment.BuilderService;
			waveService    = Environment.WaveService;
			
			explosiveEffect = Resources.Load(Environment.RESOURCE_EXPLOSION_BUILDING)
				as GameObject;
			seedReference = Resources.Load(Environment.RESOURCE_SEED) 
				as GameObject;

			health = builderService.GetHealth(this);
			healthSegment = 1f/ health;

			healthIndicator = GetComponentsInChildren
					<ProceduralImage>()
				.First(img => img.name.Contains(Environment.UI_HEALTH_INDICATOR));
			builderService.AddBuilding(this);

			if (!(this is BeaconController)) waveService.NextWave += TrySpawnSeed;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.P) && !(this is BeaconController)) TrySpawnSeed(new Wave(3, 3, 3, 3));
		}


		/*===============================
        *  Handling
        ==============================*/
		void TrySpawnSeed(Wave wave)
		{
			var chance  = Environment.BASE_SEED_SPAWN + wave.waveNumber / 100f;
			var roll = Random.Range(0, 1f);

			if (roll <= chance)
			{
				var offset = ExtraOffset() + 3f;
				var rand2DPoint = Random.insideUnitCircle.normalized * offset;
				var spawnPoint = new Vector3(
					rand2DPoint.x,
					0,
					rand2DPoint.y);

				var seed = Instantiate(
					seedReference,
					spawnPoint + transform.position,
					new Quaternion());
				seed.name = "seed" + seed.GetInstanceID();
				
				seeds.Add(seed);
			}
		}
		
		
		/*===============================
        *  IDestroyable
        ==============================*/
		public bool Damage(AbstractActorController actorController)
		{
			if (health == 0) return false;
			
			health--;
			RefreshUI();
			if (health == 0) Explode();
			return true;
		}

		public int Health() => health;

		public virtual float ExtraOffset() => Environment.BUILDING_OFFSET;

		public Transform GetTransform() => transform;
		
		
		
		/*===============================
        *  Feedback
        ==============================*/
		protected void RefreshUI()
		{
			healthIndicator.fillAmount = healthSegment * health;
		}
		
		protected void Explode()
		{
			Instantiate(explosiveEffect, transform.position, new Quaternion());
			foreach (var seed in seeds)
				Destroy(seed);
			Destroy(gameObject);
		}
	}
}