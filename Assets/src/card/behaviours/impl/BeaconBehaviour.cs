using System.Collections;
using UnityEngine;

namespace src.card.behaviours.impl
{
	public class BeaconBehaviour : CardBehaviour
	{
		protected override IEnumerator BehaviourRoutine()
		{
			yield return null;
			print("fired card");
		}
	}
}