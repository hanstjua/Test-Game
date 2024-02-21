using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SampleButton : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        Debug.Log("enter!");
    }

    void OnMouseExit()
    {
        Debug.Log("exit!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter!");
    }
}
