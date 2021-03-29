using System.Collections;
using src.util;
using UnityEngine;

namespace src.card.behaviours.impl
{
	public class BeaconBehaviour : CardBehaviour
	{
		protected override IEnumerator BehaviourRoutine(RaycastHit hit)
		{
			Environment.BuilderService.PlaceBeacon(hit);
			yield return null;
		}
	}
}