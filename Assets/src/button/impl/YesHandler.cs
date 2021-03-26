using src.scripting.ui;

namespace src.button.impl
{
    public class YesHandler : ButtonHandler
    {
        public override void OnPointerDownDelegate()
        {
            transform.parent.GetComponent<InitialQuestion>().FadeOut();
        }
    }
}