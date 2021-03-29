using System.Collections.Generic;
using System.Linq;
using src.card.behaviours.impl;
using src.level;
using src.util;
using UnityEngine;

namespace src.services.impl
{
	public class BuilderService : IService
	{
		public delegate void SpawnBuilder(BuilderController builderController);
		public event SpawnBuilder Builder = delegate {  };

		readonly HashSet<GameObject> buildings = new HashSet<GameObject>();

		PlayerService playerService;

		BeaconBehaviour beacon;

		BuilderController builder;
		GameObject prototypeBuilder;

		GameObject builderOrigin;



		public BuilderController ActiveBuilder => builder;
		public BeaconBehaviour   Beacon        => beacon;
		
		
		/*===============================
         *  Instantiation
         ==============================*/
		public void Init()
		{
			prototypeBuilder = Resources.Load(Environment.RESOURCE_BUILDER) 
				as GameObject;

			playerService = Environment.PlayerService;
			
			Environment.WaveService.NextWave += WaveChange;
		}

		/*===============================
         *  Handling
         ==============================*/
		public void BeaconPlaced(BeaconBehaviour beacon)
		{
			this.beacon = beacon ? beacon : null;
		}
		
		void WaveChange(Wave wave)
		{
			if (!beacon) return;
			if (!(playerService.Scrap > Environment.BUILD_COST)) return;
			if (!(builder is null)) return;

			var spawnOrigin = GetBuilderSpawn();
			var spawnPosition = spawnOrigin.transform.position;
			var direction = (beacon.transform.position - spawnPosition).normalized;
			direction.y = 0f;

			var gO = Object.Instantiate(
				prototypeBuilder, 
				spawnPosition, 
				Quaternion.LookRotation(direction));
			builder       =  gO.GetComponent<BuilderController>();
			builder.Built += Built;
		}

		void Built(GameObject building) => buildings.Add(building);
		
		
		
		/*===============================
         *  Utility
         ==============================*/
		GameObject GetBuilderSpawn()
		{
			var buildingArr = buildings.ToArray();
			var origin = buildingArr[0];
			var closest = origin.transform.position;
			var beaconPosition = beacon.transform.position;

			foreach (var b in buildingArr)
			{
				if (Vector3.Distance(b.transform.position, beaconPosition)
				    < Vector3.Distance(closest, beaconPosition))
				{
					origin  = b;
					closest = origin.transform.position;
				}
			}

			return origin;
		}
	}
}