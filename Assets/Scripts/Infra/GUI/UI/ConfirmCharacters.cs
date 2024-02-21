using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmCharacters : MonoBehaviour
{
    public UnityEvent Yes { get; private set; }
    public UnityEvent No { get; private set; }

    void Awake()
    {
        Yes = new();
        No = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        var yesButton = transform.Find("Window/Yes").GetComponent<ConfirmSelectionButton>();
        Debug.Log(yesButton.Selected);
        yesButton.Selected.AddListener(() => Yes.Invoke());

        var noButton = transform.Find("Window/No").GetComponent<ConfirmSelectionButton>();
        noButton.Selected.AddListener(() => No.Invoke());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        LeanTween.value(gameObject,i => GetComponent<CanvasGroup>().alpha = i, 1, 0, 0.2f);
        // GetComponent<CanvasGroup>().alpha = 0;
    }

    public void Show()
    {
        LeanTween.value(gameObject,i => GetComponent<CanvasGroup>().alpha = i, 0, 1, 0.2f);
        // GetComponent<CanvasGroup>().alpha = 1;
    }
}
