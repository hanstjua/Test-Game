using UnityEngine;
using Common;
using Battle;
using TMPro;

#nullable enable

public class CharacterScreen : MonoBehaviour
{
    // focus color
    [field: SerializeField] public Color FocusColor { get; private set; }

    // stats colors
    [field: SerializeField] public Color StatNormalColor { get; private set; }
    [field: SerializeField] public Color StatDecreasedColor { get; private set; }
    [field: SerializeField] public Color StatIncrasedColor { get; private set; }

    // equipment colors
    [field: SerializeField] public Color EquipmentSlotNormalColor { get; private set; }
    [field: SerializeField] public Color EquipmentSlotHighlightedColor { get; private set; }
    [field: SerializeField] public Color EquipmentSlotNormalFontColor { get; private set; }
    [field: SerializeField] public Color EquipmentSlotHighlightedFontColor { get; private set; }
    [field: SerializeField] public Color EquipmentOptionNormalColor { get; private set; }
    [field: SerializeField] public Color EquipmentOptionHighlightColor { get; private set; }
    [field: SerializeField] public Color EquipmentOptionNormalFontColor { get; private set; }
    [field: SerializeField] public Color EquipmentOptionHighlightFontColor { get; private set; }

    // Actions colors
    [field: SerializeField] public Color ActiveArbellumFontColor { get; private set; }
    [field: SerializeField] public Color ArbellumOptionNormalColor { get; private set; }
    [field: SerializeField] public Color ArbellumOptionHighlightedColor { get; private set; }
    [field: SerializeField] public Color ArbellumOptionNormalFontColor { get; private set; }
    [field: SerializeField] public Color ArbellumOptionHighlightedFontColor { get; private set; }
    [field: SerializeField] public Color ActionNormalColor { get; private set; }
    [field: SerializeField] public Color ActionHighlightedColor { get; private set; }
    [field: SerializeField] public Color ActionNormalFontColor { get; private set; }
    [field: SerializeField] public Color ActionHighlightedFontColor { get; private set; }

    public UnitOfWork? UnitOfWork { get; private set; }
    public Agent? Character { get; private set; }
    private int _equipmentOptionHeadIndex = 0;
    private int _arbellumOptionHeadIndex = 0;
    private int _actionsHeadIndex = 0;
    public EquipmentOption[]? EquipmentOptions { get; private set; }

    private CharacterScreenSelectable? _activatedSelectable = null;
    public CharacterScreenSelectable? ActivatedSelectable 
    { 
        get => _activatedSelectable;
        set
        {
            if (_activatedSelectable != null)
            {
                _activatedSelectable.Deactivate();
            }

            _activatedSelectable = value;
            if (_activatedSelectable != null)
            {
                _activatedSelectable.Activate();
            }
        } 
    }

    private bool _hasInit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;

        EquipmentOptions = GetComponentsInChildren<EquipmentOption>();

        foreach (var s in GetComponentsInChildren<CharacterScreenSelectable>())
        {
            s.CharacterScreen = this;
        }

        _hasInit = true;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        UnitOfWork = null;
        gameObject.SetActive(false);
    }

    public void SetCharacter(Agent character, UnitOfWork unitOfWork)
    {
        if (!_hasInit) Init(unitOfWork);

        Character = character;

        // character panel
        transform.Find("Top/Panel/Right/Name").GetComponent<TMP_Text>().text = Character.Name;

        transform.Find("Top/Panel/Right/Values/HP/Number").GetComponent<TMP_Text>().text = Character.Hp.ToString();
        var maxHpBarSize = transform.Find("Top/Panel/Right/Values/HP/Gauge/BarContainer").GetComponent<RectTransform>().sizeDelta;
        transform.Find("Top/Panel/Right/Values/HP/Gauge/BarContainer/Bar").GetComponent<RectTransform>().sizeDelta = new(Character.Hp / Character.Stats.MaxHp * maxHpBarSize.x, maxHpBarSize.y);

        transform.Find("Top/Panel/Right/Values/MP/Number").GetComponent<TMP_Text>().text = Character.Mp.ToString();
        var maxMpBarSize = transform.Find("Top/Panel/Right/Values/MP/Gauge/BarContainer").GetComponent<RectTransform>().sizeDelta;
        transform.Find("Top/Panel/Right/Values/MP/Gauge/BarContainer/Bar").GetComponent<RectTransform>().sizeDelta = new(Character.Mp / Character.Stats.MaxMp * maxMpBarSize.x, maxMpBarSize.y);

        // TODO: Update statuses

        // stats panel
        var maxStatsBarWidth = 300;
        var maxStats = StatLevels.MaxLevels.ToStats();

        SetStatValue("Str", Character.Stats.Strength / maxStats.Strength, maxStatsBarWidth);
        SetStatValue("Mag", Character.Stats.Magic / maxStats.Magic, maxStatsBarWidth);
        SetStatValue("Def", Character.Stats.Defense / maxStats.Defense, maxStatsBarWidth);
        SetStatValue("Mdef", Character.Stats.MagicDefense / maxStats.MagicDefense, maxStatsBarWidth);
        SetStatValue("Agi", Character.Stats.Agility / maxStats.Agility, maxStatsBarWidth);
        SetStatValue("Eva", Character.Stats.Evasion / maxStats.Evasion, maxStatsBarWidth);
        SetStatValue("Acc", Character.Stats.Accuracy / maxStats.Accuracy, maxStatsBarWidth);
        SetStatValue("Luk", Character.Stats.Luck / maxStats.Luck, maxStatsBarWidth);

        // equipment panel
        transform.Find("Middle/Equipment/Equipped/RightHand").GetComponent<EquipmentSlot>().Equipment = Character.RightHand;
        transform.Find("Middle/Equipment/Equipped/LeftHand").GetComponent<EquipmentSlot>().Equipment = Character.LeftHand;
        transform.Find("Middle/Equipment/Equipped/Armour").GetComponent<EquipmentSlot>().Equipment = Character.Armour;
        transform.Find("Middle/Equipment/Equipped/Footwear").GetComponent<EquipmentSlot>().Equipment = Character.Footwear;
        transform.Find("Middle/Equipment/Equipped/Accessory1").GetComponent<EquipmentSlot>().Equipment = Character.Accessory1;
        transform.Find("Middle/Equipment/Equipped/Accessory2").GetComponent<EquipmentSlot>().Equipment = Character.Accessory2;

        // description panel
        transform.Find($"Bottom/Description").GetComponent<TMP_Text>().text = "";

        // TODO: update panels
    }

    private void SetStatValue(string initials, float fraction, int maxBarWidth)
    {
        var statNumberObject = transform.Find($"Top/Stats/Left/{initials}/Number");
        statNumberObject = statNumberObject == null ? transform.Find($"Top/Stats/Right/{initials}/Number") : statNumberObject;
        statNumberObject.GetComponent<TMP_Text>().text = Character!.Stats.Defense.ToString();
        var statBarObject = transform.Find($"Top/Stats/Left/{initials}/Bar");
        statBarObject = statBarObject == null ? transform.Find($"Top/Stats/Right/{initials}/Bar") : statBarObject;
        var rectTransform = statBarObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new(fraction * maxBarWidth, rectTransform.sizeDelta.y);
    }

    public void ScrollEquipmentOptions(bool down, Equipment[] items)
    {
        if (down && _equipmentOptionHeadIndex + EquipmentOptions!.Length < (items.Length + 1))
        {
            _equipmentOptionHeadIndex += 1;
            SetEquipmentOptions(_equipmentOptionHeadIndex, items);

            var enableDownArrow = (_equipmentOptionHeadIndex + EquipmentOptions.Length) == (items.Length - 1);
            transform.Find("Middle/Equipment/Options/Down/Arrow").gameObject.SetActive(enableDownArrow);
        }
        else if (!down && _equipmentOptionHeadIndex > 0)
        {
            _equipmentOptionHeadIndex -= 1;
            SetEquipmentOptions(_equipmentOptionHeadIndex, items);

            var enableUpArrow = _equipmentOptionHeadIndex == 0;
            transform.Find("Middle/Equipment/Options/Up/Arrow").gameObject.SetActive(enableUpArrow);
        }
    }

    public void SetEquipmentOptions(int startingIndex, Equipment[] items)
    {
        for (int i = 0; i < EquipmentOptions!.Length; i++)
        {
            if (i < items.Length) 
            {
                var option = EquipmentOptions[i];

                option.Activate();

                if (items[i] != null) option.SetEquipment(items[startingIndex + i], items[startingIndex + i].Type);
                else option.SetEquipment(null, null);
            }
            else EquipmentOptions[i].Deactivate();
        }

        _equipmentOptionHeadIndex = startingIndex;
    }

    public void ClearEquipmentOptions()
    {
        for (int i = 0; i < EquipmentOptions!.Length; i++) EquipmentOptions[i].Deactivate();

        _equipmentOptionHeadIndex = 0;
    }
}