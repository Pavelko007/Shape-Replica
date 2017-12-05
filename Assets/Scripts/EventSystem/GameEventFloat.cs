using System.Collections.Generic;
using UnityEngine;

namespace ShapeReplica.EventSystem
{
    [CreateAssetMenu(fileName = "NewGameEventFloat.asset", menuName = "GameEventFloat", order = 1)]
    public class GameEventFloat : EventBase<float>
    {
    }
}