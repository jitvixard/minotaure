using System.Collections;
using System.Linq;
using src.actors.controllers;
using src.util;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace src.actors.handlers.sprite
{
	public class PawnSpriteHandler : SpriteHandler
	{
		public delegate void Ready(bool loaded);
		public event Ready Loaded = delegate {  };

		protected readonly ProceduralImage healthIndicator;
		protected readonly ProceduralImage loadingIndicator;

		protected Coroutine healRoutine;
		protected Coroutine loadRoutine;

		int health;
		
		/*===============================
         *  Initialisation
         ==============================*/
		public PawnSpriteHandler(AbstractActorController controller) : base(controller)
		{
			var images = controller.GetComponentsInChildren<ProceduralImage>();
			foreach (var proceduralImage in images)
			{
				if (proceduralImage.name.Equals(Environment.PAWN_HEALTH_INDICATOR))
					healthIndicator = proceduralImage;
				else if (proceduralImage.name.Equals(Environment.PAWN_LOAD_INDICATOR))
					loadingIndicator = proceduralImage;
			}
			
			loadingIndicator.fillAmount = 0f;
			
			

			health = controller.actor.health;
		}

		
		/*===============================
         *  Handling
         ==============================*/
		public void UpdateHealth()
		{
			if (healRoutine != null) controller.StopCoroutine(healRoutine);
			healRoutine = controller.StartCoroutine(HealthRoutine());
		}
		
		public void Load()
		{
			if (loadRoutine != null) controller.StopCoroutine(loadRoutine);
			loadRoutine = controller.StartCoroutine(LoadRoutine(true));
		}

		public void Fired()
		{
			if (loadRoutine != null) controller.StopCoroutine(loadRoutine);
			loadRoutine = controller.StartCoroutine(LoadRoutine(false));
		}
		
		
		
		/*===============================
         *  Routines
         ==============================*/
		IEnumerator LoadRoutine(bool loading)
		{
			var loadTime = loading
				? Environment.PlayerService.loadTime
				: Environment.PlayerService.loadTime / 10;
			var origin = loadingIndicator.fillAmount;
			var targetValue = loading
				? 1f
				: 0f;
			var t = 0f;
            
			while (loadingIndicator.fillAmount != targetValue)
			{
				t += Time.deltaTime;
				loadingIndicator.fillAmount = 
					Mathf.Lerp(origin, targetValue, t / loadTime);
                
				yield return null;
			}

			Loaded(targetValue == 1f);
		}

		IEnumerator HealthRoutine()
		{
			var origin = healthIndicator.fillAmount;
			var target = controller.actor.health * 0.5f;
			var t = 0f;
			var interval = Environment.UI_OVERHEAD_HEAL_TIME;

			if (target > 1f) target      = 1f;
			else if (target < 0f) target = 0f;

			while (healthIndicator.fillAmount != target)
			{
				t                          += Time.deltaTime;
				healthIndicator.fillAmount =  Mathf.Lerp(origin, target, t / interval);
				yield return null;
			}
			
			if (controller.actor.health <= 0) controller.Die();
		}
	}
}