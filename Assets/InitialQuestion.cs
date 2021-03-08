using System.Collections;
using System.Collections.Generic;
using src.button;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class InitialQuestion : MonoBehaviour
{
    List<ButtonHandler> buttons = new List<ButtonHandler>();
    List<TW_Regular> writers = new List<TW_Regular>();
    
    Coroutine typeRoutine;

    [SerializeField] GameObject question; 
    [SerializeField] Text text;

    [SerializeField] float appearanceDelay = 1f;
    
    string originalText;

    void Awake()
    {
        originalText = text.text;
        foreach (var tw in GetComponentsInChildren<TW_Regular>())
        {
            if (!tw.gameObject.transform.parent.name.Equals(question.name)) writers.Add(tw);
            if (tw.transform.parent.TryGetComponent<ButtonHandler>(out var button)) buttons.Add(button);
        }
    }

    void Update()
    {
        if (typeRoutine is null) typeRoutine = StartCoroutine(TypeRoutine());
    }

    IEnumerator TypeRoutine()
    {
        yield return null;
        while (!text.text.Equals(originalText))
        {
            yield return null;
        }

        var t = 0f;
        while (t < appearanceDelay)
        {
            t += Time.deltaTime;
            yield return null;
        }

        foreach (var tw in writers) tw.StartTypewriter();
        foreach (var button in buttons) button.ready = true;
    }
}
