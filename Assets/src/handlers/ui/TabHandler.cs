using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Environment = src.util.Environment;

namespace src.handlers.ui
{
    public class TabHandler : MonoBehaviour, IPointerClickHandler
    {
        /*===============================
        *  Fields
        ==============================*/
        [SerializeField] bool movingLeft;

        /*=============Tab=============*/
        protected GameObject   tab;
        protected RectTransform rectTransform;

        /*=========Transition=========*/
        Coroutine transitionRoutine;
        Vector3   origin;
        Vector3   displayed;
        bool      isOut;
    
        /*===============================
        *  Lifecycle
        ==============================*/
        protected virtual void Awake()
        {
            var tabOffset = Environment.UI_CARD_TAB_WIDTH;
            tabOffset = movingLeft ? -tabOffset : tabOffset;
            
            tab           = transform.parent.gameObject;
            rectTransform = tab.GetComponent<RectTransform>();
            origin        = rectTransform.position;
            displayed     = new Vector3(origin.x + tabOffset, origin.y, origin.z);
        }

        /*===============================
        *  Transition
        ==============================*/
        public void OnPointerClick(PointerEventData eventData)
        {
            if (transitionRoutine != null) StopCoroutine(transitionRoutine);
            transitionRoutine = StartCoroutine(TransitionRoutine());
        }

        IEnumerator TransitionRoutine()
        {
            var start = rectTransform.position;
            var target = isOut ? origin : displayed;
            var duration = Environment.UI_CARD_SLIDE_OUT;
            var t = 0f;

            isOut = !isOut;

            while (t < duration)
            {
                rectTransform.position = new Vector3(
                    Mathf.Lerp(start.x, target.x, t / duration),
                    Mathf.Lerp(start.y, target.y, t / duration),
                    Mathf.Lerp(start.z, target.z, t / duration));
                t += Time.deltaTime;
                yield return null;
            }


            rectTransform.position = target;
        }
    }
}
