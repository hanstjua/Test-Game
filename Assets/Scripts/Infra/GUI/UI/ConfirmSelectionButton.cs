using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmSelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [field: SerializeField] private Color _backgroundHighlightColor;

    private Image _background;
    private Color _defaultBackgroundColor;

    public UnityEvent Focused { get; private set; }
    public UnityEvent Unfocused { get; private set; }
    public UnityEvent Selected { get; private set; }

    void Awake()
    {
        Focused = new();
        Unfocused = new();
        Selected = new();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _background = GetComponent<Image>();
        _defaultBackgroundColor = _background.color;

        Focused.AddListener(() => HandleFocus());
        Unfocused.AddListener(() => HandleUnfocus());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Selected.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Focused.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Unfocused.Invoke();
    }

    private void HandleFocus()
    {
        _background.color = _backgroundHighlightColor;
    }

    private void HandleUnfocus()
    {
        _background.color = _defaultBackgroundColor;
    }
}
