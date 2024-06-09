using UnityEngine;
using Common;
using Battle;
using TMPro;

public class CharaterScreen : MonoBehaviour
{
    // stats colors
    [field: SerializeField] private Color _statNormalColor;
    [field: SerializeField] private Color _statDecreasedColor;
    [field: SerializeField] private Color _statIncreasedColor;

    // equipment colors
    [field: SerializeField] private Color _equipmentSlotNormalColor;
    [field: SerializeField] private Color _equipmentSlotHighlightedColor;
    [field: SerializeField] private Color _equipmentSlotNormalFontColor;
    [field: SerializeField] private Color _equipmentSlotHighlightedFontColor;
    [field: SerializeField] private Color _equipmentOptionNormalColor;
    [field: SerializeField] private Color _equipmentOptionHighlightColor;
    [field: SerializeField] private Color _equipmentOptionNormalFontColor;
    [field: SerializeField] private Color _equipmentOptionHighlightFontColor;

    // Actions colors
    [field: SerializeField] private Color _activeArbellumFontColor;
    [field: SerializeField] private Color _arbellumOptionNormalColor;
    [field: SerializeField] private Color _arbellumOptionHighlightedColor;
    [field: SerializeField] private Color _arbellumOptionNormalFontColor;
    [field: SerializeField] private Color _arbellumOptionHighlightedFontColor;
    [field: SerializeField] private Color _actionNormalColor;
    [field: SerializeField] private Color _actionHighlightedColor;
    [field: SerializeField] private Color _actionNormalFontColor;
    [field: SerializeField] private Color _actionHighlightedFontColor;

    private UnitOfWork _unitOfWork;
    private Agent _character;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Show(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        enabled = true;
    }

    void Hide()
    {
        _unitOfWork = null;
        enabled = false;
    }

    void SetCharacter(Agent character)
    {
        _character = character;

        // character panel
        transform.Find("Top/Panel/Right/Name").GetComponent<TMP_Text>().text = _character.Name;

        transform.Find("Top/Panel/Right/Values/HP/Number").GetComponent<TMP_Text>().text = _character.Hp.ToString();
        var maxHpBarSize = transform.Find("Top/Panel/Right/Values/HP/Gauge/BarContainer").GetComponent<RectTransform>().sizeDelta;
        transform.Find("Top/Panel/Right/Values/HP/Gauge/BarContainer/Bar").GetComponent<RectTransform>().sizeDelta = new(_character.Hp / _character.Stats.MaxHp * maxHpBarSize.x, maxHpBarSize.y);

        transform.Find("Top/Panel/Right/Values/MP/Number").GetComponent<TMP_Text>().text = _character.Mp.ToString();
        var maxMpBarSize = transform.Find("Top/Panel/Right/Values/MP/Gauge/BarContainer").GetComponent<RectTransform>().sizeDelta;
        transform.Find("Top/Panel/Right/Values/MP/Gauge/BarContainer/Bar").GetComponent<RectTransform>().sizeDelta = new(_character.Mp / _character.Stats.MaxMp * maxMpBarSize.x, maxMpBarSize.y);

        // TODO: Update statuses

        // stats panel
        var maxStatsBarWidth = 300;
        var maxStats = StatLevels.MaxLevels.ToStats();

        SetStatValue("Str", _character.Stats.Strength / maxStats.Strength, maxStatsBarWidth);
        SetStatValue("Mag", _character.Stats.Magic / maxStats.Magic, maxStatsBarWidth);
        SetStatValue("Def", _character.Stats.Defense / maxStats.Defense, maxStatsBarWidth);
        SetStatValue("Mdef", _character.Stats.MagicDefense / maxStats.MagicDefense, maxStatsBarWidth);
        SetStatValue("Agi", _character.Stats.Agility / maxStats.Agility, maxStatsBarWidth);
        SetStatValue("Eva", _character.Stats.Evasion / maxStats.Evasion, maxStatsBarWidth);
        SetStatValue("Acc", _character.Stats.Accuracy / maxStats.Accuracy, maxStatsBarWidth);
        SetStatValue("Luk", _character.Stats.Luck / maxStats.Luck, maxStatsBarWidth);

        // equipment panel
        transform.Find("Middle/Equipment/Equipped/RightHand/Name").GetComponent<TMP_Text>().text = _character.RightHand.Type.Name;
        transform.Find("Middle/Equipment/Equipped/LeftHand/Name").GetComponent<TMP_Text>().text = _character.LeftHand.Type.Name;
        transform.Find("Middle/Equipment/Equipped/Armour/Name").GetComponent<TMP_Text>().text = _character.Armour.Type.Name;
        transform.Find("Middle/Equipment/Equipped/Footwear/Name").GetComponent<TMP_Text>().text = _character.Footwear.Type.Name;
        transform.Find("Middle/Equipment/Equipped/Accessory1/Name").GetComponent<TMP_Text>().text = _character.Accessory1.Type.Name;
        transform.Find("Middle/Equipment/Equipped/Accessory2/Name").GetComponent<TMP_Text>().text = _character.Accessory2.Type.Name;

        // description panel
        transform.Find($"Bottom/Description").GetComponent<TMP_Text>().text = "";

        // TODO: update panels
    }

    private void SetStatValue(string initials, float fraction, int maxBarWidth)
    {
        transform.Find($"Top/Stats/Left/{initials}/Number").GetComponent<TMP_Text>().text = _character.Stats.Defense.ToString();
        var rectTransform = transform.Find($"Top/Stats/Left/{initials}/Bar").GetComponent<RectTransform>();
        rectTransform.sizeDelta = new(fraction * maxBarWidth, rectTransform.sizeDelta.y);
    }
}