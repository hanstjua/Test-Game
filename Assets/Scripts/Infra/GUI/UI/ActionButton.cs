using Battle;
using Common;
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
        public NullAction() : base(new(""), "")
        {
        }

        public override AreaOfEffect TargetArea => throw new System.NotImplementedException();

        public override ArbellumType Arbellum => throw new System.NotImplementedException();

        public override ActionPrerequisite[] Criteria => throw new System.NotImplementedException();

        public override AreaOfEffect AreaOfEffect => throw new System.NotImplementedException();

        public override bool IsActorAbleToExecute(Agent actor, Battle.Battle battle, UnitOfWork unitOfWork)
        {
            throw new System.NotImplementedException();
        }

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

    void Awake()
    {
        Focused = new();
        Unfocused = new();
        Selected = new();

        _null = new();
        _name = transform.Find("Name").GetComponent<TMP_Text>();

        _isFocused = false;
        
        _background = GetComponent<Image>();
        _defaultBackgroundColor = _background.color;
        _defaultButtonMaterial = _background.material;

        _action = new NullAction();
    }

    // Start is called before the first frame update
    void Start()
    {
        Focused.AddListener(_ => HandleFocus());
        Unfocused.AddListener(_ => HandleUnfocus());

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

        _name.text = action.Type.Name;
    }

    public void SetNoAction()
    {
        _action = _null;

        Debug.Log(_name.text);
        Debug.Log(_action);

        _name.text = _action.Type.Name;

        Disable();
    }
}
