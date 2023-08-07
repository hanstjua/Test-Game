using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
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
        set
        {
            _character = value;

            // update attributes
            transform.Find("Name").GetComponent<TMP_Text>().text = value.Name;
            transform.Find("HpValue").GetComponent<TMP_Text>().text = value.Hp.ToString();
            transform.Find("MpValue").GetComponent<TMP_Text>().text = value.Mp.ToString();
        }
    }
}
