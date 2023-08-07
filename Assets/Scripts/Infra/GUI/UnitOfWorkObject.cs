using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

[CreateAssetMenu(fileName = "UnitOfWorkObject", menuName = "ScriptableObjects/UnitOfWorkObject")]
public class UnitOfWorkObject : ScriptableObject
{
    public UnitOfWork obj;
}
