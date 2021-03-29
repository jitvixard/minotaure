using src.model;
using src.util;

namespace src.impl
{
	public class BuilderStateMachine : AbstractStateMachine
	{
		/*===============================
         *  Unity Lifecycle
         ==============================*/
		protected override void Awake()
		{
			controller    = GetComponent<BuilderController>();
			playerService = Environment.PlayerService;
		}
		
		
		
		/*===============================
         *  Directives
         ==============================*/
		void Move()
		{
			((BuilderController) controller).Move();
		}

		void Unload()
		{
			((BuilderController) controller).Unload();
		}
		
		
		/*===============================
         *  State Updates
         ==============================*/
		public override void CheckState()
		{
			if (CurrentState == State.Idle) Move();
			else if (CurrentState == State.Stopped) Unload();
		}
	}
}