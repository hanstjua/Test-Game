using System;
using System.Collections;
using System.Collections.Generic;
using PlasticGui.WorkspaceWindow;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    [Serializable]
    private class Entry
    {
        public string name;
        public Color color;
    }

    [field: SerializeField] private List<Entry> _colors;

    public static ColorPalette Get()
    {
        var obj = Resources.Load("Prefabs/Maps/ColorPalette")  as GameObject;
        return obj.GetComponent<ColorPalette>();
    }

    public Color GetColor(string name)
    {
        var entry = _colors.Find(e => e.name == name);
        return entry != null ? entry.color : Color.magenta;
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
