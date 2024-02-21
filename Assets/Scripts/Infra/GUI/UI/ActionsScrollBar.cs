using System.Collections;
using System.Collections.Generic;
using Battle;
using Codice.Client.Commands.TransformerRule;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionScrollBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [field: SerializeField] private Color _backgroundHighlightColor;

    private Image _background;
    private Color _defaultBackgroundColor;
    private Material _defaultButtonMaterial;

    public UnityEvent<Action> Focused { get; private set; }
    public UnityEvent<Action> Unfocused { get; private set; }
    public UnityEvent<Action> Selected { get; private set; }

    private int _sectionIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _background = transform.GetChild(0).GetComponent<Image>();
        _defaultBackgroundColor = _background.color;
        _defaultButtonMaterial = _background.material;

        _sectionIndex = 0;

        Focused = new();
        Unfocused = new();
        Selected = new();

        // for demo purposes
        // if (_isSelectable) Enable(_action);
        // else Disable();

        // RemoveAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSections(int count)
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            var child = transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        transform.GetChild(0).GetComponent<Image>().material = _defaultButtonMaterial;

        for (int i = 0; i < count - 1; i++)
        {
            var scroller = Instantiate(transform.GetChild(0).gameObject);
            scroller.transform.SetParent(transform, worldPositionStays: false);
        }

        transform.GetChild(0).GetComponent<Image>().material = null;
        transform.GetChild(0).GetComponent<Image>().color = _backgroundHighlightColor;
    }

    private void SetSection(int index)
    {
        transform.GetChild(_sectionIndex).GetComponent<Image>().material = _defaultButtonMaterial;

        _sectionIndex = index;

        transform.GetChild(_sectionIndex).GetComponent<Image>().material = null;
        transform.GetChild(_sectionIndex).GetComponent<Image>().color = _backgroundHighlightColor;
    }

    public void NextSection()
    {
        SetSection((_sectionIndex + 1) % transform.childCount);
    }

    public void PreviousSection()
    {
        SetSection((_sectionIndex - transform.childCount) % transform.childCount + (transform.childCount - 1));
    }
}
