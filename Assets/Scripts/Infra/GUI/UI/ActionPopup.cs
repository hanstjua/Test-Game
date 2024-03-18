using System.Linq;
using Codice.Client.BaseCommands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPopup : MonoBehaviour
{
    [field: SerializeField] private int _minWidth;
    [field: SerializeField] private int _sizePerChar;
    [field: SerializeField] private int _padding;

    [field: SerializeField] private Color _normalTopColor;
    [field: SerializeField] private Color _normalTopFontColor;
    [field: SerializeField] private Color _normalBottomColor;
    [field: SerializeField] private Color _normalBottomFontColor;
    [field: SerializeField] private Color _preemptTopColor;
    [field: SerializeField] private Color _preemptTopFontColor;
    [field: SerializeField] private Color _preemptBottomColor;
    [field: SerializeField] private Color _preemptBottomFontColor;
    [field: SerializeField] private Color _respondTopColor;
    [field: SerializeField] private Color _respondTopFontColor;
    [field: SerializeField] private Color _respondBottomColor;
    [field: SerializeField] private Color _respondBottomFontColor;

    private RectTransform _rect;
    private Image _type;
    private TMP_Text _typeName;
    private Image _action;
    private TMP_Text _actionName;

    // Start is called before the first frame update
    void Start()
    {
        _rect = GetComponent<RectTransform>();

        _type = transform.Find("Type").GetComponent<Image>();
        _typeName = _type.transform.Find("Name").GetComponent<TMP_Text>();

        _action = transform.Find("Action").GetComponent<Image>();
        _actionName = _action.transform.Find("Name").GetComponent<TMP_Text>();
    }

    public void Hide() => GetComponent<CanvasGroup>().alpha = 0;
    public void Show() => GetComponent<CanvasGroup>().alpha = 1;

    private void SetAction(string name)
    {
        _actionName.text = name;

        var newWidth = name.Count() * _sizePerChar + _padding;
        _rect.sizeDelta = new Vector2(newWidth < _minWidth ? _minWidth : newWidth, _rect.sizeDelta.y);
    }

    public void SetNormal(string action)
    {
        SetAction(action);

        _typeName.text = "Action";

        _type.color = _normalTopColor;
        _typeName.color = _normalTopFontColor;
        _action.color = _normalBottomColor;
        _actionName.color = _normalBottomFontColor;
    }

    public void SetPreempt(string action)
    {
        SetAction(action);

        _typeName.text = "Preempt";

        _type.color = _preemptTopColor;
        _typeName.color = _preemptTopFontColor;
        _action.color = _preemptBottomColor;
        _actionName.color = _preemptBottomFontColor;
    }

    public void SetRespond(string action)
    {
        SetAction(action);

        _typeName.text = "Respond";

        _type.color = _respondTopColor;
        _typeName.color = _respondTopFontColor;
        _action.color = _respondBottomColor;
        _actionName.color = _respondBottomFontColor;
    }
}
