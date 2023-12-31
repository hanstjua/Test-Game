using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Battle;

[CreateAssetMenu(fileName = "Events", menuName = "ScriptableObjects/Events")]
public class BattleEvents : ScriptableObject
{
    public readonly UnityEvent<AgentId, Position> characterMoved = new(); 
    public readonly UnityEvent<Position, Position> cursorSelectionChanged = new();
}