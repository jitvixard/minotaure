using System;
using System.Collections.Generic;
using System.Linq;
using src.card.behaviours.impl;
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

		readonly HashSet<GameObject> beaconsToBuild = new HashSet<GameObject>();

		readonly HashSet<GameObject> buildings = new HashSet<GameObject>();

		PlayerService playerService;

		BuilderController builder;


		GameObject                           builderOrigin;
		Tuple<BuilderController, GameObject> builderAndBeacon;
		//beacon & cursor;
		Tuple<GameObject, GameObject>        activeBeacon;



		public BuilderController ActiveBuilder => builder;

		public GameObject NextBeacon
		{
			get
			{
				if (beaconsToBuild.Count >= 1)
				{
					var toBuild = beaconsToBuild.First();
					beaconsToBuild.Remove(toBuild);
					return toBuild;
				}

				return null;
			}
		}
		
		
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
			
			var newBeacon = GameObject.Instantiate(
				prototypeBeacon,
				hit.point,
				new Quaternion());

			var cursor = Environment.CardService.DetachCursor();
			
			if (!cursor.TryGetComponent<CursorBehaviour>(out var cursorBehaviour))
			{
				GameObject.Destroy(cursor);
				cursor = null;
			}

			activeBeacon = new Tuple<GameObject, GameObject>(newBeacon, cursor);
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
			builder.Built    += Built;
		}

		void Built(BuilderController builder, GameObject building)
		{
			if (builderAndBeacon.Item1 == builder)
			{
				beaconsToBuild.Clear();
				this.builder = null;
			}
		}
		
		
		
		/*===============================
         *  Utility
         ==============================*/
		public GameObject GetInfrastructureTarget()
		{
			if (!(builder is null)) return builder.gameObject;
			
			var buildingArr = buildings.ToArray();
			var index = Random.Range(0, buildingArr.Length - 1);
			return buildingArr[index];
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

			return origin;
		}
	}
}