using src.buildings.controllers;
using src.util;
using UnityEngine;

namespace src.card.behaviours.impl
{
	public class EyeBehaviour : CardBehaviour
	{
		BeaconController beacon;
		
		bool cameraAttached;
		
		protected override bool BehaviourDirective(RaycastHit hit)
		{
			var hitObject = hit.collider.gameObject;
			if (hitObject.name == Environment.OVERHEAD_UI)
			{
				hitObject = hitObject.transform.parent.gameObject;
			}

			if (hitObject.TryGetComponent<BeaconController>(out var beaconController))
			{
				beaconController.Setup(this);
				return true;
			}

			return false;
		}

		public override bool SetUpAsButton(BeaconController reference)
		{
			asButton = !beacon && reference.TryGetComponent(out beacon);
			if (asButton) ColorSetup(reference);
			return asButton;
		}

		public override bool ExecuteAction()
		{
			//TODO show camera
			return true;
		}
	}
}