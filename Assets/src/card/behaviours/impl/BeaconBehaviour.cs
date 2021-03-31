using src.buildings.controllers;
using src.util;
using UnityEngine;

namespace src.card.behaviours.impl
{
	public class BeaconBehaviour : CardBehaviour
	{
		protected override bool BehaviourDirective(RaycastHit hit)
		{
			var hitObject = hit.collider.gameObject;

			if (hitObject.TryGetComponent<TowerController>(out var towerController))
			{
				Environment.BuilderService.ReadyBuilder(towerController);
				return true;
			}
			if (hitObject.CompareTag(Environment.TAG_FLOOR))
				return Environment.BuilderService.PlaceBeacon(hit);
			return false;

		}

		public override bool SetUpAsButton(BeaconController reference)
		{
			return false;
		}

		public override bool ExecuteAction()
		{
			return false;
		}
	}
}