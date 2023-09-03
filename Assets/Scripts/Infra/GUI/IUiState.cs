using Battle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// to enable declaring struct while compiling against older .NET Framework (<5.0)
namespace System.Runtime.CompilerServices
{
        internal static class IsExternalInit {}
}

public record BattleProperties(
    UnitOfWork unitOfWork,
    Dictionary<AgentId, GameObject> characters,
    Map map,
    BattleId battleId,
    Cursor cursor,
    GameObject uiObjects,
    BattleEvents battleEvents
);

public interface IUiState
{
    public IUiState Update(BattleProperties battleProperties);
}