using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "Swarm/GameBalance", order = 1)]
    public class GameBalanceSO : ScriptableObject
    {
        public float PlayerSpeed;
        public int MaxEnemies;
    }
}