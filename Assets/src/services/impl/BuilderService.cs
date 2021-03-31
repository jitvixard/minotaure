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

		readonly Dictionary<Color, BeaconController> beaconColorMap = new Dictionary<Color, BeaconController>();
		
		//stores
		readonly HashSet<GameObject> beaconsToBuild = new HashSet<GameObject>();

		readonly HashSet<TowerController> buildings = new HashSet<TowerController>();

		readonly HashSet<IDestroyable> destructables = new HashSet<IDestroyable>();

		readonly BeaconController[] beacons = new BeaconController[2];

		TowerController originTower;

		BuilderController                    builder;


		public IDestroyable[] Destructibles => destructables.ToArray();


		/*===============================
         *  Instantiation
         ==============================*/
		public void Init()
		{
			Environment.WaveService.NextWave += WaveChange;
			
			beaconColorMap.Add(Environment.ButtonColourA, null);
			beaconColorMap.Add(Environment.ButtonColourB, null);
		}
		
		

		/*===============================
         *  Handling
         ==============================*/

		public bool PlaceBeacon(RaycastHit hit)
		{
			if (beaconsToBuild.Count >= Environment.MAX_BEACONS)
				return false;

			var prototypeBeacon = Resources.Load(Environment.RESOURCE_BEACON)
				as GameObject;
			
			var newBeacon = Object.Instantiate(
				prototypeBeacon,
				hit.point,
				new Quaternion());

			var beaconController = newBeacon.GetComponent<BeaconController>();
			var colorArr = beaconColorMap.ToArray();
			foreach (var entry in colorArr)
			{
				if (entry.Value == null)
				{
					beaconColorMap.Remove(entry.Key);
					beaconColorMap.Add(entry.Key, beaconController);
					beaconController.SelectionColor = entry.Key;
					break;
				}
			}

			return beaconsToBuild.Add(newBeacon);
		}

		public void AddBuilding(TowerController tower)
		{ 
			if (tower is BeaconController beaconController)
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
			
			buildings.Add(tower);
			destructables.Add(tower);
		}

		public void ReadyBuilder(TowerController originTower)
		{
			this.originTower = originTower;
		}
		
		void WaveChange(Wave wave)
		{
			if (beaconsToBuild.Count < 1) return;
			if (originTower is null) return;
			if (!(builder is null)) return;

			var beacon = beaconsToBuild.First();
			
			var spawnPosition = originTower.transform.position;
			var direction = (beacon.transform.position - spawnPosition).normalized;
			direction.y = 0f;

			var prototypeBuilder = Resources.Load(Environment.RESOURCE_BUILDER) 
				as GameObject;
			
			var gO = Object.Instantiate(
				prototypeBuilder, 
				spawnPosition, 
				Quaternion.LookRotation(direction));
			builder          =  gO.GetComponent<BuilderController>();

			originTower = null;
		}

		public void TargetDestroyed(IDestroyable destroyable)
		{
			if (destroyable is BeaconController beaconController)
			{
				beaconsToBuild.Remove(beaconController.gameObject);
				var colorArr = beaconColorMap.ToArray();
				foreach (var keyValuePair in colorArr)
				{
					if (keyValuePair.Value == beaconController)
					{
						beaconColorMap.Remove(keyValuePair.Key);
						beaconColorMap.Add(keyValuePair.Key, null);
						break;
					}
				}
			}
			if (destroyable is TowerController buildingController)
				buildings.Remove(buildingController);
			if (destroyable is BuilderController)
				builder = null;
			
			destructables.Remove(destroyable);

			BuildingDestroyed(destroyable.GetTransform().gameObject);
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

		public int GetHealth(TowerController towerController)
		{
			if (towerController is BeaconController)
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

		public void BuilderFailed(GameObject beacon)
		{
			if (beaconsToBuild.Count >= Environment.MAX_BEACONS)
			{
				BuildingDestroyed(beacon);
				Object.Destroy(beacon);
			}
			else
			{
				beaconsToBuild.Add(beacon);
			}
		}
	}
}