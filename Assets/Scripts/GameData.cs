using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonTools;

namespace Swarm
{
    [Serializable]
    public class GameData
    {
        public Vec2 PlayerPos;
        public Vec2 PlayerDir;
        public int PlayerIdx;

        public int EnemyCount;
        public Vec2[] EnemyPos;
        public Vec2[] EnemyCurrentDir;
        public Vec2[] EnemyTargetDir;
        public float[] EnemySpeed;
        public float[] EnemyRotation;
    }
}