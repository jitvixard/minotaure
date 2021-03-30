using System.Collections;
using System.Collections.Generic;
using src.button;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace src.scripting.ui
{
    public class InitialQuestion : MonoBehaviour
    {
        [SerializeField] GameObject question;
        [SerializeField] Text       text;

        [SerializeField] float               appearanceDelay = 1f;
        [SerializeField] float               fadeOutTime     = 1f;
        readonly         List<ButtonHandler> buttons         = new List<ButtonHandler>();
        readonly         List<Object>        elements        = new List<Object>();

        string originalText;

        Coroutine                 typeRoutine;
        readonly List<TW_Regular> writers = new List<TW_Regular>();

        void Awake()
        {
            originalText = text.text;
            foreach (var tw in GetComponentsInChildren<TW_Regular>())
            {
                if (!tw.gameObject.transform.parent.name.Equals(question.name)) writers.Add(tw);
                if (tw.transform.parent.TryGetComponent<ButtonHandler>(out var button)) buttons.Add(button);
                elements.Add(tw.GetComponent<Text>());
            }

            foreach (var image in GetComponentsInChildren<ProceduralImage>()) elements.Add(image);
        }

        void Update()
        {
            if (typeRoutine is null) typeRoutine = StartCoroutine(TypeRoutine());
        }

        public void FadeOut()
        {
            StartCoroutine(FadeRoutine());
        }

        IEnumerator TypeRoutine()
        {
            yield return null;
            while (!text.text.Equals(originalText)) yield return null;

            var t = 0f;
            while (t < appearanceDelay)
            {
                t += Time.deltaTime;
                yield return null;
            }

            foreach (var tw in writers) tw.StartTypewriter();
            //foreach (var button in buttons) button.ready = true;
        }

        IEnumerator FadeRoutine()
        {
            var alpha = 1f;

            while (alpha > 0f)
            {
                alpha -= Time.deltaTime / fadeOutTime;
                foreach (var element in elements)
                    switch (element)
                    {
                        case Text t:
                            t.color = new Color(t.color.r, t.color.g, t.color.b, alpha);
                            break;
                        case ProceduralImage i:
                            i.color = new Color(i.color.r, i.color.g, i.color.b, alpha);
                            break;
                    }

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}