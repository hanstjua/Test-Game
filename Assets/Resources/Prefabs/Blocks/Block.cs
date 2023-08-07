using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Battle;

public class Block : MonoBehaviour//, IPointerEnterHandler
{
    [field: SerializeField] public bool Traversable { get; private set; }
    [field: SerializeField] public TerrainType Type { get; private set; }
    [field: SerializeField] public bool IsStartingPosition { get; private set; }

    void Awake()
    {
        
    }
}
