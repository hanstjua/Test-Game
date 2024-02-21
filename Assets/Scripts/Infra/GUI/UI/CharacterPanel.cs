using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] public BattleEvents battleEvents;
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
            transform.Find("Panel/Right/Name").GetComponent<TMP_Text>().text = value.Name;

            var hpText = transform.Find("Panel/Right/Values/HP/Number").GetComponent<TMP_Text>();
            hpText.text = value.Hp.ToString();
            if (value.Hp == 0)
            {
                hpText.color = _palette.GetColor("Red4");
            }
            else
            {
                hpText.color = _palette.GetColor("Blue5");
            }

            transform.Find("Panel/Right/Values/MP/Number").GetComponent<TMP_Text>().text = value.Mp.ToString();
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

    public void UpdateChracterPanelByAgent(Agent agent)
    {
        Character = agent;
        Show();
    }
}
