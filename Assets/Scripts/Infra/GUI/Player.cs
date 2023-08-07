using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameObject.transform.Translate(0, 0, 1);
            Debug.Log(Utils.Add(1, 2));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameObject.transform.Translate(0, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.transform.Translate(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.transform.Translate(1, 0, 0);
        }
    }
}
