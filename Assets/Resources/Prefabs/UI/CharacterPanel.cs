using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] public BattleEvents battleEvents;
    
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
            transform.Find("Name").GetComponent<TMP_Text>().text = value.Name;
            transform.Find("HpValue").GetComponent<TMP_Text>().text = value.Hp.ToString();
            transform.Find("MpValue").GetComponent<TMP_Text>().text = value.Mp.ToString();
        }
    }

    public void UpdateCharacterPanel(BattleProperties battleProperties, Position position)
    {
        var agentRepository = battleProperties.unitOfWork.AgentRepository;
        var agent = agentRepository.GetFirstBy(a => a.Position.Equals(position));

        if (agent != null)
        {
            Character = agent;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
