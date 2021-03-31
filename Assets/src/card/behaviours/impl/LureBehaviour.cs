using System.Collections;
using src.buildings.controllers;
using UnityEngine;

namespace src.card.behaviours.impl
{
	public class LureBehaviour : CardBehaviour
	{
		protected override bool BehaviourDirective(RaycastHit hit)
		{
			return false;
		}

		public override bool SetUpAsButton(BeaconController reference)
		{
			return false;
		}

		public override bool ExecuteAction()
		{
			//TODO get everything in swarm to chase
			return false;
		}
	}
}