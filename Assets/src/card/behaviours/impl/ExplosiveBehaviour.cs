using src.actors.controllers;
using src.buildings.controllers;
using src.interfaces;
using src.util;
using UnityEngine;

namespace src.card.behaviours.impl
{
	public class ExplosiveBehaviour : CardBehaviour
	{
		public delegate void TriggerExplosion();
		public event TriggerExplosion Trigger = delegate { };

		IDestroyable destroyable;

		GameObject explosion;

		protected override void Awake()
		{
			base.Awake();
			explosion = Resources.Load(Environment.RESOURCE_EXPLOSION_BUILDING)
				as GameObject;
		}

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

			if (hitObject.TryGetComponent<AbstractActorController>(out var ac))
			{
				Instantiate(explosion, ac.transform.position, new Quaternion());
				var colliders = new Collider[1];
				Physics.OverlapSphereNonAlloc(ac.transform.position, 5f, colliders);
				
				foreach (var col in colliders)
				{
					var go = col.gameObject;
					if (go.TryGetComponent<AbstractActorController>(out ac))
					{
						ac.Die();
					}
				}
				return true;
			}

			return false;
		}

		public override bool SetUpAsButton(BeaconController reference)
		{
			if (reference.TryGetComponent<TowerController>(out var tower))
				if (tower is IDestroyable destroyable)
				{
					this.destroyable = destroyable;
					asButton         = true;
					ColorSetup(reference);
					return true;
				}

			return false;
		}

		public override bool ExecuteAction()
		{
			//TODO make boom;
			return true;
		}
	}
}