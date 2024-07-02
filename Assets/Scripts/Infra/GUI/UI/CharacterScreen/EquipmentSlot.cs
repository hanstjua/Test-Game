using System;
using System.Linq;
using Battle;
using Common;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable enable

public abstract class EquipmentSlot : CharacterScreenSelectable
{
    private Equipment? _equipment;

    public abstract EquipmentType Type { get; }
    public abstract EquipmentSlot TriggerEquipService(AgentId id, Equipment? equipment, UnitOfWork unitOfWork);

    public Equipment? Equipment 
    { 
        get => _equipment; 
        set
        {
            TriggerEquipService((AgentId) CharacterScreen.Character!.Id(), value, CharacterScreen.UnitOfWork!);

            CharacterScreen.ClearEquipmentOptions();
            CharacterScreen.ActivatedSelectable = null;

            _equipment = value;
            transform.Find("Name").GetComponent<TMP_Text>().text = _equipment != null ? _equipment.Value() : "-";

            var image = GetComponent<Image>();
            image.color = CharacterScreen.EquipmentSlotNormalColor;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public override void OnClick(PointerEventData eventdata)
    {
        // TODO: update equipment options
        if (CharacterScreen.ActivatedSelectable != this)
        {
            CharacterScreen.ActivatedSelectable = this;
        }
    }

    public override void OnEnter(PointerEventData eventdata)
    {
        // TODO: Update description
        if (Equipment != null)
        {
            CharacterScreen.transform.Find("Bottom/Description").GetComponent<TMP_Text>().text = Equipment.Description;

            var statsBoost = Equipment.StatsBoost;
            
            if (statsBoost.Strength != 0)
            {
                var text = statsBoost.Strength > 0 ? $"STR: +{statsBoost.Strength}" : $"STR: {statsBoost.Strength}";
                CharacterScreen.transform.Find("Bottom/Stats/Str").GetComponent<TMP_Text>().text = text;
            }

            if (statsBoost.Magic != 0)
            {
                var text = statsBoost.Magic > 0 ? $"MAG: +{statsBoost.Magic}" : $"MAG: {statsBoost.Magic}";
                CharacterScreen.transform.Find("Bottom/Stats/Mag").GetComponent<TMP_Text>().text = text;
            }

            if (statsBoost.Defense != 0)
            {
                var text = statsBoost.Defense > 0 ? $"DEF: +{statsBoost.Defense}" : $"DEF: {statsBoost.Defense}";
                CharacterScreen.transform.Find("Bottom/Stats/Def").GetComponent<TMP_Text>().text = text;
            }

            if (statsBoost.MagicDefense != 0)
            {
                var text = statsBoost.MagicDefense > 0 ? $"MDEF: +{statsBoost.MagicDefense}" : $"MDEF: {statsBoost.MagicDefense}";
                CharacterScreen.transform.Find("Bottom/Stats/Mdef").GetComponent<TMP_Text>().text = text;
            }

            if (statsBoost.Agility != 0)
            {
                var text = statsBoost.Agility > 0 ? $"AGI: +{statsBoost.Agility}" : $"AGI: {statsBoost.Agility}";
                CharacterScreen.transform.Find("Bottom/Stats/Agi").GetComponent<TMP_Text>().text = text;
            }

            if (statsBoost.Accuracy != 0)
            {
                var text = statsBoost.Accuracy > 0 ? $"ACC: +{statsBoost.Accuracy}" : $"ACC: {statsBoost.Accuracy}";
                CharacterScreen.transform.Find("Bottom/Stats/Acc").GetComponent<TMP_Text>().text = text;
            }

            if (statsBoost.Evasion != 0)
            {
                var text = statsBoost.Evasion > 0 ? $"EVA: +{statsBoost.Evasion}" : $"EVA: {statsBoost.Evasion}";
                CharacterScreen.transform.Find("Bottom/Stats/Eva").GetComponent<TMP_Text>().text = text;
            }

            if (statsBoost.Luck != 0)
            {
                var text = statsBoost.Luck > 0 ? $"LUK: +{statsBoost.Luck}" : $"LUK: {statsBoost.Luck}";
                CharacterScreen.transform.Find("Bottom/Stats/Luk").GetComponent<TMP_Text>().text = text;
            }
        }
    }

    public override void OnExit(PointerEventData eventdata)
    {
        // TODO: Clear description
        if (Equipment != null)
        {
            CharacterScreen.transform.Find("Bottom/Description").GetComponent<TMP_Text>().text = "";

            CharacterScreen.transform.Find("Bottom/Stats/Str").GetComponent<TMP_Text>().text = "";
            CharacterScreen.transform.Find("Bottom/Stats/Mag").GetComponent<TMP_Text>().text = "";
            CharacterScreen.transform.Find("Bottom/Stats/Def").GetComponent<TMP_Text>().text = "";
            CharacterScreen.transform.Find("Bottom/Stats/Mdef").GetComponent<TMP_Text>().text = "";
            CharacterScreen.transform.Find("Bottom/Stats/Agi").GetComponent<TMP_Text>().text = "";
            CharacterScreen.transform.Find("Bottom/Stats/Acc").GetComponent<TMP_Text>().text = "";
            CharacterScreen.transform.Find("Bottom/Stats/Eva").GetComponent<TMP_Text>().text = "";
            CharacterScreen.transform.Find("Bottom/Stats/Luk").GetComponent<TMP_Text>().text = "";
        }
    }

    public override void Activate()
    {
        var inventory = CharacterScreen
        .UnitOfWork!
        .InventoryRepository
        .Get();

        var equipment = inventory
        .AllEquipment
        .Where(
            e => 
            inventory.Equipped(e) < inventory.Amount(e) &&
            (e.Type.GetType() == Type.GetType() || 
            e.Type.GetType().IsSubclassOf(Type.GetType()))
        )
        .Append(null)
        .ToArray();

        var equipmentIndex = Array.IndexOf(equipment, Equipment);
        var lengthDelta = equipment.Length - CharacterScreen.EquipmentOptions!.Length;
        var startingIndex = Equipment != null ? Math.Max(Math.Min(equipmentIndex, lengthDelta), 0) : 0;

        CharacterScreen.SetEquipmentOptions(startingIndex, equipment!);

        GetComponent<Image>().color = CharacterScreen.EquipmentSlotHighlightedColor;
        GetComponentInChildren<TMP_Text>().color = CharacterScreen.EquipmentSlotHighlightedFontColor;
    }

    public override void Deactivate()
    {
        GetComponent<Image>().color = CharacterScreen.EquipmentSlotNormalColor;
        GetComponentInChildren<TMP_Text>().color = CharacterScreen.EquipmentSlotNormalFontColor;
    }

    public override void Focus()
    {
        
    }

    public override void Unfocus()
    {
        
    }
}