using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directions : MonoBehaviour
{
    public static GameObject Create(string resourcePath, Vector3 position, Quaternion rotation)
    {
        return Instantiate((GameObject) Resources.Load(resourcePath), position + new Vector3(0, 1.5f, 0), rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
