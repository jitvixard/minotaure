using System.Collections;
using src.actors.controllers;
using src.util;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace src.actors.handlers.sprite
{
	public class PawnSpriteHandler : SpriteHandler
	{
		protected readonly ProceduralImage loadingIndicator;
		
		protected Coroutine loadRoutine;
		
		public PawnSpriteHandler(AbstractActorController controller) : base(controller)
		{
			loadingIndicator            = controller.GetComponentInChildren<ProceduralImage>();
			loadingIndicator.fillAmount = 0f;
		}
		
		public void Load()
		{
			if (loadRoutine != null) controller.StopCoroutine(loadRoutine);
			loadRoutine = controller.StartCoroutine(LoadRoutine());
		}
		
		IEnumerator LoadRoutine()
		{
			var loadTime = Environment.PlayerService.loadTime;
			var origin = loadingIndicator.fillAmount;
			var targetValue = origin > 0.1f
				? 0f
				: 1f;
			var t = 0f;
            
			while (loadingIndicator.fillAmount != targetValue)
			{
				t += Time.deltaTime;
				loadingIndicator.fillAmount = 
					Mathf.Lerp(origin, targetValue, t / loadTime);
                
				yield return null;
			}
		}
	}
}