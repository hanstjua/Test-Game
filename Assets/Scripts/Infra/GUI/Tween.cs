using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Tween : MonoBehaviour
{
    private TMP_Text _text;

    [SerializeField] public float duration;
    [SerializeField] public float start;
    [SerializeField] public float end;
    [SerializeField] public ColorsObject co;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            LeanTween.value(_text.gameObject, start, end, duration)
            .setOnUpdate(updateText)
            .setOnComplete(delay(1, disappear));
            _text.alpha = 1;
            _text.text = start.ToString("F0");
        }
    }

    Action delay(float sec, Action func)
    {
        return () => LeanTween.value(1, 0, sec).setOnComplete(func);
    }

    void updateText(float f)
    {
        _text.text = f.ToString("F0");
    }

    void updateAlpha(float f)
    {
        _text.alpha = f;
    }

    void disappear()
    {
        LeanTween.value(_text.gameObject, 1, 0, duration).setOnUpdate(updateAlpha);
        LeanTween.moveY(_text.gameObject, _text.transform.position.y + 50, duration)
        .setOnComplete(reset);
    }

    void reset()
    {
        _text.transform.position -= new Vector3(0, 50, 0);
    }
}
