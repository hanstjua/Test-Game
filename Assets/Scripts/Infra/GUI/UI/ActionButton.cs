using System.Collections;
using System.Collections.Generic;
using Battle;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool _isFocused;
    [field: SerializeField] private bool _isSelectable;
    [field: SerializeField] private Color _backgroundHighlightColor;
    [field: SerializeField] private Color _textEnabledColor;
    [field: SerializeField] private Color _textDisabledColor;

    private TMP_Text _name;
    private Image _background;
    private Color _defaultBackgroundColor;
    private Material _defaultButtonMaterial;

    public UnityEvent<Action> Focused { get; private set; }
    public UnityEvent<Action> Unfocused { get; private set; }
    public UnityEvent<Action> Selected { get; private set; }

    private Action _action;
    private NullAction _null;

    private class NullAction : Action
    {
        public NullAction() : base("", "")
        {
        }

        public override AreaOfEffect AreaOfEffect => throw new System.NotImplementedException();

        public override ActionType Type => throw new System.NotImplementedException();

        public override SkillType Skill => throw new System.NotImplementedException();

        public override ActionCriterion[] Criteria => throw new System.NotImplementedException();

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle.Battle battle, UnitOfWork unitOfWork)
        {
            throw new System.NotImplementedException();
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            throw new System.NotImplementedException();
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isSelectable) Selected.Invoke(_action);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelectable) Focused.Invoke(_action);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelectable) Unfocused.Invoke(_action);
    }

    // Start is called before the first frame update
    void Start()
    {
        _isFocused = false;
        _name = transform.Find("Name").GetComponent<TMP_Text>();
        _background = GetComponent<Image>();
        _defaultBackgroundColor = _background.color;
        _defaultButtonMaterial = _background.material;

        Focused = new();
        Unfocused = new();
        Selected = new();
        _action = new NullAction();

        Focused.AddListener(_ => HandleFocus());
        Unfocused.AddListener(_ => HandleUnfocus());

        _null = new();

        // for demo purposes
        // if (_isSelectable) Enable(_action);
        // else Disable();

        // RemoveAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Focus()
    {
        Focused.Invoke(_action);
    }

    public void Unfocus()
    {
        Unfocused.Invoke(_action);
    }

    private void HandleFocus()
    {
        _isFocused = true;

        _background.color = _backgroundHighlightColor;
        _background.material = null;
    }

    private void HandleUnfocus()
    {
        _isFocused = false;

        _background.color = _defaultBackgroundColor;
        _background.material = _defaultButtonMaterial;
    }

    public void Enable(Action action)
    {
        _isSelectable = true;

        _name.color = _textEnabledColor;
        _background.color = _defaultBackgroundColor;
        _background.material = _defaultButtonMaterial;
        
        SetAction(action);
    }

    public void Disable()
    {
        _isSelectable = false;

        _name.color = _textDisabledColor;
        _background.color = _defaultBackgroundColor;
        _background.material = _defaultButtonMaterial;
        _action = _null;
    }

    public void SetAction(Action action)
    {
        _action = action;

        _name.text = action.Name;
    }

    public void SetNoAction()
    {
        _action = _null;

        _name.text = _action.Name;

        Disable();
    }
}
