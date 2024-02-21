using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

[CreateAssetMenu(fileName = "ColorsObject", menuName = "ScriptableObjects/ColorsObject")]
public class ColorsObject : ScriptableObject
{
    private class Entry
    {
        public readonly string name;
        public readonly Color color;
    }

    [field: SerializeField] private List<Entry> _entries;
    [field: SerializeField] private Dictionary<string, Color> _colors;

    public Color GetColor(string name)
    {
        var ret = _entries.Find(e => e.name == name).color;
        return ret == null ? Color.magenta : ret;
    }
}
