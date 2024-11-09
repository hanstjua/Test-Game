using UnityEngine;
using Common;
using Battle;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using Battle.Statuses;

#nullable enable

public class CharacterScreen : MonoBehaviour
{
    // focus color
    [field: SerializeField] public Color FocusColor { get; private set; }

    // stats colors
    [field: SerializeField] public Color StatNormalColor { get; private set; }
    [field: SerializeField] public Color StatDecreasedColor { get; private set; }
    [field: SerializeField] public Color StatIncreasedColor { get; private set; }

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
    [field: SerializeField] public Color ArbellumOptionToggledColor { get; private set; }
    [field: SerializeField] public Color ArbellumOptionNormalFontColor { get; private set; }
    [field: SerializeField] public Color ArbellumOptionHighlightedFontColor { get; private set; }
    [field: SerializeField] public Color ArbellumOptionToggledFontColor { get; private set; }
    [field: SerializeField] public Color ActionNormalColor { get; private set; }
    [field: SerializeField] public Color ActionHighlightedColor { get; private set; }
    [field: SerializeField] public Color ActionNormalFontColor { get; private set; }
    [field: SerializeField] public Color ActionHighlightedFontColor { get; private set; }
    [field: SerializeField] public Color ActionMutedFontColor { get; private set; }
    [field: SerializeField] public Color DescriptionHighlightFontColor { get; private set; }

    public UnitOfWork? UnitOfWork { get; private set; }
    public Agent? Character { get; private set; }
    private int _equipmentOptionHeadIndex = 0;
    private int _arbellumOptionHeadIndex = 0;
    private int _actionsHeadIndex = 0;
    public EquipmentOption[]? EquipmentOptions { get; private set; }
    public ActiveArbellum[]? ActiveArbellums { get; private set; }
    public ArbellumOption[]? ArbellumOptions { get; private set; }
    public AvailableAction[]? AvailableActions { get; private set; }

    private CharacterScreenSelectable? _activatedSelectable = null;
    private CharacterScreenSelectable? _focusedArbellum = null;
    private CharacterPanel? _characterPanel = null;

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

        ActiveArbellums = GetComponentsInChildren<ActiveArbellum>();
        foreach (var a in ActiveArbellums)
        {
            a.CharacterScreen = this;
        }

        ArbellumOptions = GetComponentsInChildren<ArbellumOption>();
        foreach (var a in ArbellumOptions)
        {
            a.CharacterScreen = this;
        }

        AvailableActions = GetComponentsInChildren<AvailableAction>();
        foreach (var a in AvailableActions)
        {
            a.CharacterScreen = this;
        }

        _characterPanel = GetComponentInChildren<CharacterPanel>();        

        _hasInit = true;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetCharacter(Agent character, UnitOfWork unitOfWork)
    {
        if (!_hasInit) Init(unitOfWork);

        Character = character;

        Refresh();
    }

    public void Refresh()
    {
        Character = UnitOfWork!.AgentRepository.Get(Character!.Id() as AgentId);

        // character panel
        transform.Find("Top/Panel/Right/Name").GetComponent<TMP_Text>().text = Character.Name;

        transform.Find("Top/Panel/Right/Values/HP/Number").GetComponent<TMP_Text>().text = Character.Hp.ToString();
        var maxHpBarSize = transform.Find("Top/Panel/Right/Values/HP/Gauge/BarContainer").GetComponent<RectTransform>().sizeDelta;
        transform.Find("Top/Panel/Right/Values/HP/Gauge/BarContainer/Bar").GetComponent<RectTransform>().sizeDelta = new(Character.Hp / Character.Stats.MaxHp * maxHpBarSize.x, maxHpBarSize.y);

        transform.Find("Top/Panel/Right/Values/MP/Number").GetComponent<TMP_Text>().text = Character.Mp.ToString();
        var maxMpBarSize = transform.Find("Top/Panel/Right/Values/MP/Gauge/BarContainer").GetComponent<RectTransform>().sizeDelta;
        transform.Find("Top/Panel/Right/Values/MP/Gauge/BarContainer/Bar").GetComponent<RectTransform>().sizeDelta = new(Character.Mp / Character.Stats.MaxMp * maxMpBarSize.x, maxMpBarSize.y);

        _characterPanel!.UpdateCharacterPanelByAgent(Character);

        // TODO: Update statuses
        
        // stats panel
        ResetStatValues();

        // equipment panel
        transform.Find("Middle/Equipment/Equipped/RightHand").GetComponent<EquipmentSlot>().Equipment = Character.RightHand;
        transform.Find("Middle/Equipment/Equipped/LeftHand").GetComponent<EquipmentSlot>().Equipment = Character.LeftHand;
        transform.Find("Middle/Equipment/Equipped/Armour").GetComponent<EquipmentSlot>().Equipment = Character.Armour;
        transform.Find("Middle/Equipment/Equipped/Footwear").GetComponent<EquipmentSlot>().Equipment = Character.Footwear;
        transform.Find("Middle/Equipment/Equipped/Accessory1").GetComponent<EquipmentSlot>().Equipment = Character.Accessory1;
        transform.Find("Middle/Equipment/Equipped/Accessory2").GetComponent<EquipmentSlot>().Equipment = Character.Accessory2;

        // description panel
        SetDescription("");
        SetElements(null);
        SetStatuses(null);
        SetCost("");

        // bottom stats
        transform.Find("Bottom/Stats/Str").gameObject.SetActive(false);
        transform.Find("Bottom/Stats/Mag").gameObject.SetActive(false);
        transform.Find("Bottom/Stats/Def").gameObject.SetActive(false);
        transform.Find("Bottom/Stats/Mdef").gameObject.SetActive(false);
        transform.Find("Bottom/Stats/Agi").gameObject.SetActive(false);
        transform.Find("Bottom/Stats/Acc").gameObject.SetActive(false);
        transform.Find("Bottom/Stats/Eva").gameObject.SetActive(false);
        transform.Find("Bottom/Stats/Luk").gameObject.SetActive(false);

        // active arbella
        UpdateActiveArbella();

        // arbellum options
        foreach (var a in ArbellumOptions!)
        {
            a.ClearArbellum();
        }

        foreach (var i in Enumerable.Range(0, Character.Arbella.Length))
        {
            ArbellumOptions[i].SetArbellum(Character.Arbella[i]);
        }

        // TODO: update panels
    }

    public void UpdateActiveArbella()
    {
        foreach (var a in ActiveArbellums!)
        {
            a.ClearArbellum();
        }

        var activeArbella = Character!.Arbella.Where(a => a.IsActive).ToArray();
        foreach (var i in Enumerable.Range(0, activeArbella.Length))
        {
            ActiveArbellums[i].SetArbellum(activeArbella[i]);
        }
    }

    public void ResetStatValues()
    {
        var maxStatsBarWidth = 300;
        var maxStats = StatLevels.MaxLevels.ToStats();

        SetStatValue("Str", Character!.Stats.Strength, maxStatsBarWidth / maxStats.Strength);
        SetStatValue("Mag", Character!.Stats.Magic, maxStatsBarWidth / maxStats.Magic);
        SetStatValue("Def", Character!.Stats.Defense, maxStatsBarWidth / maxStats.Defense);
        SetStatValue("Mdef", Character!.Stats.MagicDefense, maxStatsBarWidth / maxStats.MagicDefense);
        SetStatValue("Agi", Character!.Stats.Agility, maxStatsBarWidth / maxStats.Agility);
        SetStatValue("Eva", Character!.Stats.Evasion, maxStatsBarWidth / maxStats.Evasion);
        SetStatValue("Acc", Character!.Stats.Accuracy, maxStatsBarWidth / maxStats.Accuracy);
        SetStatValue("Luk", Character!.Stats.Luck, maxStatsBarWidth / maxStats.Luck);

        _characterPanel!.RemoveStatsComparison();
    }

    private void SetStatValue(string initials, int statValue, float barScaler)
    {
        var statNumberObject = transform.Find($"Top/Stats/Left/{initials}/Number");
        statNumberObject = statNumberObject == null ? transform.Find($"Top/Stats/Right/{initials}/Number") : statNumberObject;
        var number = statNumberObject.GetComponent<TMP_Text>()!;
        number.text = statValue.ToString();
        number.color = StatNormalColor;
        
        var statBarObject = transform.Find($"Top/Stats/Left/{initials}/Bar");
        statBarObject = statBarObject == null ? transform.Find($"Top/Stats/Right/{initials}/Bar") : statBarObject;
        statBarObject.GetComponent<LayoutElement>()!.preferredWidth = Math.Max(1, statValue * barScaler);
        statBarObject.GetComponent<Image>()!.color = StatNormalColor;
    }

    public void PreviewArbellum(Arbellum? arbellum)
    {
        
    }

    public void PreviewEquipment(Equipment? current, Equipment? potential)
    {
        var currentStatBoost = current != null ? current.StatsBoost : new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var potentialStatBoost = potential != null ? potential.StatsBoost : new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        var currentStat = Character!.Stats;
        var potentialStat = Character!.Stats.Deaugment(currentStatBoost).Augment(potentialStatBoost);

        var maxStatsBarWidth = 300;
        var maxStats = StatLevels.MaxLevels.ToStats();

        SetStatPreviewValue("Str", potentialStat.Strength, potentialStat.Strength - currentStat.Strength, maxStatsBarWidth / maxStats.Strength);
        SetStatPreviewValue("Mag", potentialStat.Magic, potentialStat.Magic - currentStat.Magic, maxStatsBarWidth / maxStats.Magic);
        SetStatPreviewValue("Def", potentialStat.Defense, potentialStat.Defense - currentStat.Defense, maxStatsBarWidth / maxStats.Defense);
        SetStatPreviewValue("Mdef", potentialStat.MagicDefense, potentialStat.MagicDefense - currentStat.MagicDefense, maxStatsBarWidth / maxStats.MagicDefense);
        SetStatPreviewValue("Agi", potentialStat.Agility, potentialStat.Agility - currentStat.Agility, maxStatsBarWidth / maxStats.Agility);
        SetStatPreviewValue("Eva", potentialStat.Evasion, potentialStat.Evasion - currentStat.Evasion, maxStatsBarWidth / maxStats.Evasion);
        SetStatPreviewValue("Acc", potentialStat.Accuracy, potentialStat.Accuracy - currentStat.Accuracy, maxStatsBarWidth / maxStats.Accuracy);
        SetStatPreviewValue("Luk", potentialStat.Luck, potentialStat.Luck - currentStat.Luck, maxStatsBarWidth / maxStats.Luck);

        _characterPanel!.CompareStats(potentialStat);
    }

    private void SetStatPreviewValue(string initials, int potentialValue, int valueChange, float barScaler)
    {
        if (valueChange != 0)
        {
            var color = valueChange < 0 ? StatDecreasedColor : StatIncreasedColor;

            var statNumberObject = transform.Find($"Top/Stats/Left/{initials}/Number");
            statNumberObject = statNumberObject == null ? transform.Find($"Top/Stats/Right/{initials}/Number") : statNumberObject;
            var number = statNumberObject.GetComponent<TMP_Text>()!;
            number.text = $"{potentialValue} ({valueChange})";
            number.color = color;
            
            var statBarObject = transform.Find($"Top/Stats/Left/{initials}/Bar");
            statBarObject = statBarObject == null ? transform.Find($"Top/Stats/Right/{initials}/Bar") : statBarObject;
            statBarObject.GetComponent<LayoutElement>()!.preferredWidth = Math.Max(1, potentialValue * barScaler);
            statBarObject.GetComponent<Image>()!.color = color;
        }
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

        _characterPanel!.RemoveStatsComparison();

        _equipmentOptionHeadIndex = 0;
    }

    public void SetArbellumOptions(int startingIndex, Arbellum[] arbellums)
    {
        for (int i = 0; i < ArbellumOptions!.Length; i++)
        {
            if (i < arbellums.Length)
            {
                var option = ArbellumOptions[i];
                option.Activate();
                option.SetArbellum(arbellums[i]);
            }
            else ArbellumOptions[i].Deactivate();
        }

        _arbellumOptionHeadIndex = startingIndex;
    }

    public void ClearArbellumOptions()
    {

    }

    public void FocusArbellumOption(ArbellumOption option)
    {
        if (_focusedArbellum != null) _focusedArbellum.Unfocus();
        _focusedArbellum = option;
        _focusedArbellum.Focus();
    }

    public void SetAvailableActions(int startingIndex, Arbellum arbellum)
    {
        ClearAvailableActions();
        var learned = arbellum.Actives.Concat(arbellum.Passives).ToArray();
        for (int i = 0; i < AvailableActions!.Length; i++)
        {
            if (i < learned.Length)
            {
                var availableAction = AvailableActions[i];
                var action = learned[i];
                availableAction.SetAction(action, false);
            }
            else if (i - learned.Length < arbellum.Learnables.Length)
            {
                var availableAction = AvailableActions[i];
                var learnable = arbellum.Learnables[i];
                availableAction.SetAction(learnable.Action, true);
            }
            else AvailableActions[i].ClearAction();
        }

        _actionsHeadIndex = startingIndex;
    }

    public void ClearAvailableActions()
    {
        foreach (var a in AvailableActions!)
        {
            a.ClearAction();
        }

        _actionsHeadIndex = 0;
    }

    public void SetDescription(string content)
    {
        var hex = $"#{ColorUtility.ToHtmlStringRGB(DescriptionHighlightFontColor)}";
        transform.Find("Bottom/Description").GetComponent<TMP_Text>().text = content.Replace("{{", $"<color={hex}>").Replace("}}", "</color>");
    }

    public void SetCost(string content)
    {
        var text = transform.Find("Bottom/Effects/Cost").GetComponent<TMP_Text>();

        if (content != "")
        {
            text.text = $"COST: {content}";
            text.gameObject.SetActive(true);
        }
        else
        {
            text.text = "";
            text.gameObject.SetActive(false);
        }
    }

    public void SetElements(ElementType[]? elements)
    {
        var text = transform.Find("Bottom/Effects/Element").GetComponent<TMP_Text>();

        if (elements != null)
        {
            var content = string.Join(", ", elements.Select(e => e.Name));
            text.text = $"ELEMENT: {content}";
            text.gameObject.SetActive(true);
        }
        else
        {
            text.text = "";
            text.gameObject.SetActive(false);
        }
    }

    public void SetStatuses(StatusType[]? statuses)
    {
        var text = transform.Find("Bottom/Effects/Status").GetComponent<TMP_Text>();

        if (statuses != null)
        {
            var content = string.Join(", ", statuses.Select(e => e.Name));
            text.text = $"STATUS: {content}";
            text.gameObject.SetActive(true);
        }
        else
        {
            text.text = "";
            text.gameObject.SetActive(false);
        }
    }
}