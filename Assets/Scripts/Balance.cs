using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonTools;
using System;

namespace Swarm
{
    public class Balance
    {
        public float PlayerSpeed = 2.0f;
        public float MinEnemySpeed = 0.5f;
        public float MaxEnemySpeed = 1.5f;
        public float MinEnemyRotation = 0.5f;
        public float MaxEnemyRotation = 1.5f;
        public int NumEnemies = 10000;
    }
}