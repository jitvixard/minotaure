using System;
using System.Collections.Generic;
using System.Linq;
using src.actors.controllers;
using src.buildings.controllers;
using src.interfaces;
using src.level;
using UnityEngine;
using Environment = src.util.Environment;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace src.services.impl
{
	public class BuilderService : IService
	{
		public delegate void SpawnBuilder(BuilderController builderController);
		public event SpawnBuilder Builder = delegate {  };

		public delegate void EmitBuildingDestroyed(GameObject destroyed);
		public event EmitBuildingDestroyed BuildingDestroyed = delegate {  };
		
		//stores
		readonly HashSet<GameObject> beaconsToBuild = new HashSet<GameObject>();

		readonly HashSet<BuildingController> buildings = new HashSet<BuildingController>();

		readonly HashSet<IDestroyable> destructables = new HashSet<IDestroyable>();

		readonly BeaconController[] beacons = new BeaconController[2];
		
		
		PlayerService playerService;

		BuilderController                    builder;
		GameObject                           builderOrigin;
		Tuple<BuilderController, GameObject> builderAndBeacon;


		public IDestroyable[] Destructibles => destructables.ToArray();


		/*===============================
         *  Instantiation
         ==============================*/
		public void Init()
		{
			playerService = Environment.PlayerService;
			
			Environment.WaveService.NextWave += WaveChange;
		}
		
		

		/*===============================
         *  Handling
         ==============================*/
		public void PlaceBeacon(RaycastHit hit)
		{
			var prototypeBeacon = Resources.Load(Environment.RESOURCE_BEACON)
				as GameObject;
			
			var newBeacon = Object.Instantiate(
				prototypeBeacon,
				hit.point,
				new Quaternion());

			beaconsToBuild.Add(newBeacon);
		}

		public void AddBuilding(BuildingController building)
		{ 
			if (building is BeaconController beaconController)
			{
				for (int i = 0; i < beacons.Length; i++)
				{
					if (beacons[i] == null)
					{
						beacons[i] = beaconController;
						break;
					}
				}

				if (beacons[0] != beaconController
				    && beacons[1] != beaconController)
				{
					Object.Destroy(beaconController);
					return;
				}
			}
			
			buildings.Add(building);
			destructables.Add(building);
		}
		
		void WaveChange(Wave wave)
		{
			if (beaconsToBuild.Count < 1) return;
			if (!(playerService.Scrap > Environment.BUILD_COST)) return;
			if (!(builder is null)) return;

			var beacon = beaconsToBuild.First();

			var spawnOrigin = GetBuilderSpawn(beacon);
			var spawnPosition = spawnOrigin.transform.position;
			var direction = (beacon.transform.position - spawnPosition).normalized;
			direction.y = 0f;

			var prototypeBuilder = Resources.Load(Environment.RESOURCE_BUILDER) 
				as GameObject;
			
			var gO = Object.Instantiate(
				prototypeBuilder, 
				spawnPosition, 
				Quaternion.LookRotation(direction));
			builder          =  gO.GetComponent<BuilderController>();
			builderAndBeacon =  new Tuple<BuilderController, GameObject>(builder, beacon);
		}
		
		
		
		/*===============================
         *  Utility
         ==============================*/
		public GameObject GetNextBeacon()
		{
			if (beaconsToBuild.Count >= 1)
			{
				var toBuild = beaconsToBuild.First();
				beaconsToBuild.Remove(toBuild);
				return toBuild;
			}

			return null;
		}

		public int GetHealth(BuildingController buildingController)
		{
			if (buildingController is BeaconController)
				return Environment.HEALTH_BEACON;
			
			return Environment.HEALTH_BUILDER;

		}
		
		
		
		/*===============================
         *  Utility
         ==============================*/
		public GameObject GetInfrastructureTarget()
		{
			if (!(builder is null)) return builder.gameObject;
			
			var buildingArr = buildings.ToArray();
			var index = Random.Range(0, buildingArr.Length - 1);
			return buildingArr[index].transform.gameObject;
		}

		public bool CanGetInfrastructureTarget()
		{
			return buildings.Count > 0;
		}

		GameObject GetBuilderSpawn(GameObject beacon)
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

			return origin.transform.gameObject;
		}
	}
}