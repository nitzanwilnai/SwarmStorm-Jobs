using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Swarm/Enemy", order = 1)]
    public class EnemySO : ScriptableObject
    {
        [HideInInspector]public int ID;

        public GameObject Prefab;

        public int HP;
        public float MinSpeed;
        public float MaxSpeed;
        public float MinRotation;
        public float MaxRotation;
    }
}