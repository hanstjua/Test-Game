using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using TMPro;
using System;

public class CharacterPanel : MonoBehaviour
{
    [field: SerializeField] private ColorPalette _palette;
    
    private Agent _character;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Agent Character
    {
        get => _character;
        private set
        {
            _character = value;

            // update attributes
            transform.Find("Right/Name").GetComponent<TMP_Text>().text = value.Name;

            var hpText = transform.Find("Right/Values/HP/Number").GetComponent<TMP_Text>();
            hpText.text = value.Hp.ToString();
            if (value.Hp == 0)
            {
                hpText.color = _palette.GetColor("Red4");
            }
            else
            {
                hpText.color = _palette.GetColor("Blue5");
            }
            SetHpBar((float)value.Hp / value.Stats.MaxHp);

            var mpText = transform.Find("Right/Values/MP/Number").GetComponent<TMP_Text>();
            mpText.text = value.Mp.ToString();
            if (value.Mp == 0)
            {
                mpText.color = _palette.GetColor("Red4");
            }
            else
            {
                mpText.color = _palette.GetColor("Blue5");
            }
            SetMpBar((float)value.Mp / value.Stats.MaxMp);
        }
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    public void Show()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }

    public void UpdateCharacterPanelByPosition(BattleProperties battleProperties, Position position)
    {
        var agentRepository = battleProperties.unitOfWork.AgentRepository;
        var agent = agentRepository.GetFirstBy(a => a.Position.Equals(position));

        if (agent != null)
        {
            Character = agent;
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void UpdateCharacterPanelByAgent(Agent agent)
    {
        Character = agent;
        Show();
    }

    private void SetHpBar(float percentage)
    {
        percentage = Math.Min(percentage, 1);  // cap to 100%
        var size = transform.Find("Right/Values/HP/Gauge/Line").GetComponent<RectTransform>().sizeDelta;
        transform.Find("Right/Values/HP/Gauge/BarContainer/Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(percentage * size.x, size.y);
    }

    private void SetMpBar(float percentage)
    {
        percentage = Math.Min(percentage, 1);
        var size = transform.Find("Right/Values/MP/Gauge/Line").GetComponent<RectTransform>().sizeDelta;
        transform.Find("Right/Values/MP/Gauge/BarContainer/Bar").GetComponent<RectTransform>().sizeDelta = new Vector2(percentage * size.x, size.y);
    }

    public void CompareStats(Stats stats)
    {
        var hpText = transform.Find("Right/Values/HP/Number").GetComponent<TMP_Text>();
        var deltaMaxHp = stats.MaxHp - Character.Stats.MaxHp;
        var deltaMaxHpSign = deltaMaxHp > 0 ? "+" : "";
        var deltaMaxHpString = deltaMaxHp == 0 ? $"{Character.Hp}" : $" (MAX{deltaMaxHpSign}{deltaMaxHp})";
        hpText.text = $"{deltaMaxHpString}";
        if (deltaMaxHp < 0)
        {
            hpText.color = _palette.GetColor("Red4");
        }
        else if (deltaMaxHp > 0)
        {
            hpText.color = _palette.GetColor("Green4");
        }
        else
        {
            hpText.color = _palette.GetColor("Blue5");
        }
        SetHpBar((float)Character.Hp / stats.MaxHp);

        var mpText = transform.Find("Right/Values/MP/Number").GetComponent<TMP_Text>();
        var deltaMaxMp = stats.MaxMp - Character.Stats.MaxMp;
        var deltaMaxMpSign = deltaMaxMp > 0 ? "+" : "";
        var deltaMaxMpString = deltaMaxMp == 0 ? $"{Character.Mp}" : $" (MAX{deltaMaxMpSign}{deltaMaxMp})";
        mpText.text = $"{deltaMaxMpString}";
        if (deltaMaxMp < 0)
        {
            mpText.color = _palette.GetColor("Red4");
        }
        else if (deltaMaxMp > 0)
        {
            mpText.color = _palette.GetColor("Green4");
        }
        else
        {
            hpText.color = _palette.GetColor("Blue5");
        }
        SetMpBar((float)Character.Mp / stats.MaxMp);
    }

    public void RemoveStatsComparison()
    {
        Character = Character;
    }
}
