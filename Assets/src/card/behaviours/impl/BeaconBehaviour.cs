using System.Collections;

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