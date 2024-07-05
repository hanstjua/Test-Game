using Battle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveArbellum : MonoBehaviour
{
    public CharacterScreen CharacterScreen { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetArbellum(Arbellum arbellum)
    {
        transform.Find("Name").GetComponent<TMP_Text>().text = arbellum.Type.Name;
        transform.Find("Icon").GetComponent<Image>().color = new(1, 1, 1, 1);
    }

    public void ClearArbellum()
    {
        transform.Find("Name").GetComponent<TMP_Text>().text = "";
        transform.Find("Icon").GetComponent<Image>().color = new(0, 0, 0, 0);
    }
}
