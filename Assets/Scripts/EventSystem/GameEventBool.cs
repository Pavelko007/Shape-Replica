using UnityEngine;

namespace ShapeReplica.EventSystem
{
    [CreateAssetMenu(fileName = "NewGameEventBool.asset", menuName = "GameEventBool", order = 1)]
    public class GameEventBool : EventBase<bool>
    {
    }
}