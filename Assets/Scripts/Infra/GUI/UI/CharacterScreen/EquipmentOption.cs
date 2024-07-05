using Battle;
using Common;
using System;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable enable

public class EquipmentOption : CharacterScreenSelectable
{
    public const string NONE_LABEL = "- None -";


    public Equipment? Equipment { get; private set; }
    public EquipmentType? Type { get; private set; }

    private bool _active = false;
    public bool Active
    {
        get => _active;
    }

    private Color? _currentColor = null;
    private Color? _currentFontColor = null;

    public override void Activate()
    {
        _active = true;
    }

    public override void Deactivate()
    {
        var name = transform.Find("Name").GetComponent<TMP_Text>();
        name.text = "";
        name.color = CharacterScreen.EquipmentOptionNormalFontColor;
        GetComponent<Image>().color = CharacterScreen.EquipmentOptionNormalColor;

        Equipment = null;
        Type = null;
        _active = false;
    }

    public override void Focus()
    {
        throw new System.NotImplementedException();
    }

    public override void Unfocus()
    {
        throw new NotImplementedException();
    }

    public void SetEquipment(Equipment? equipment, EquipmentType? type)
    {
        if (type is null && equipment is not null) throw new ArgumentException($"Type cannot be NULL when equipment is not NULL ({equipment})");

        
        Equipment = equipment;

        if (equipment != null)
        {
            var inventory = CharacterScreen.UnitOfWork!.InventoryRepository.Get();
            var amount = inventory.Amount(equipment) - inventory.Equipped(equipment);

            transform.Find("Name").GetComponent<TMP_Text>().text = $"{equipment.Value()}  x{amount}";
        }
        else
        {
            transform.Find("Name").GetComponent<TMP_Text>().text = NONE_LABEL;
        }

        Type = type;
    }

    public override void OnClick(PointerEventData eventdata)
    {
        // TODO: Update equipment slot
        if (CharacterScreen.ActivatedSelectable is EquipmentSlot slot)
        {
            if (Type != null)
            {
                slot.Equipment = Equipment;
            }
            else if (transform.Find("Name").GetComponent<TMP_Text>().text == NONE_LABEL)
            {
                slot.Equipment = null;
            }

            CharacterScreen.ResetStatValues();
        }
    }

    public override void OnEnter(PointerEventData eventdata)
    {
        if (_active)
        {
            if (Equipment is not null)
            {
                CharacterScreen.transform.Find("Bottom/Description").GetComponent<TMP_Text>().text = Equipment.Description;

                var statsBoost = Equipment.StatsBoost;

                var text = statsBoost.Strength > 0 ? $"STR: +{statsBoost.Strength}" : $"STR: {statsBoost.Strength}";
                CharacterScreen.transform.Find("Bottom/Stats/Str").GetComponent<TMP_Text>().text = text;

                text = statsBoost.Magic > 0 ? $"MAG: +{statsBoost.Magic}" : $"MAG: {statsBoost.Magic}";
                CharacterScreen.transform.Find("Bottom/Stats/Mag").GetComponent<TMP_Text>().text = text;

                text = statsBoost.Defense > 0 ? $"DEF: +{statsBoost.Defense}" : $"DEF: {statsBoost.Defense}";
                CharacterScreen.transform.Find("Bottom/Stats/Def").GetComponent<TMP_Text>().text = text;

                text = statsBoost.MagicDefense > 0 ? $"MDEF: +{statsBoost.MagicDefense}" : $"MDEF: {statsBoost.MagicDefense}";
                CharacterScreen.transform.Find("Bottom/Stats/Mdef").GetComponent<TMP_Text>().text = text;

                text = statsBoost.Agility > 0 ? $"AGI: +{statsBoost.Agility}" : $"AGI: {statsBoost.Agility}";
                CharacterScreen.transform.Find("Bottom/Stats/Agi").GetComponent<TMP_Text>().text = text;

                text = statsBoost.Accuracy > 0 ? $"ACC: +{statsBoost.Accuracy}" : $"ACC: {statsBoost.Accuracy}";
                CharacterScreen.transform.Find("Bottom/Stats/Acc").GetComponent<TMP_Text>().text = text;

                text = statsBoost.Evasion > 0 ? $"EVA: +{statsBoost.Evasion}" : $"EVA: {statsBoost.Evasion}";
                CharacterScreen.transform.Find("Bottom/Stats/Eva").GetComponent<TMP_Text>().text = text;

                text = statsBoost.Luck > 0 ? $"LUK: +{statsBoost.Luck}" : $"LUK: {statsBoost.Luck}";
                CharacterScreen.transform.Find("Bottom/Stats/Luk").GetComponent<TMP_Text>().text = text;
            }

            var image = GetComponent<Image>();
            _currentColor = image.color;
            image.color = CharacterScreen.FocusColor;

            if (CharacterScreen.ActivatedSelectable is EquipmentSlot slot)
            {
                CharacterScreen.PreviewEquipment(slot.Equipment, Equipment);
            }
        }
    }

    public override void OnExit(PointerEventData eventdata)
    {
        if (_active)
        {
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

            if (_currentColor != null)
            {
                GetComponent<Image>().color = (Color) _currentColor;
                _currentColor = null;
            }
            else GetComponent<Image>().color = CharacterScreen.EquipmentOptionNormalColor;

            if (_currentFontColor != null)
            {
                transform.Find("Name").GetComponent<TMP_Text>().color = (Color) _currentFontColor;
                _currentFontColor = null;
            }
            else transform.Find("Name").GetComponent<TMP_Text>().color = CharacterScreen.EquipmentOptionNormalFontColor;

            CharacterScreen.ResetStatValues();
        }
    }
}