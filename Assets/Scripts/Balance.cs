using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonTools;
using System;
using System.IO;

namespace Swarm
{
    public class Balance
    {
        public float PlayerSpeed = 2.0f;
        public float MinEnemySpeed = 0.5f;
        public float MaxEnemySpeed = 1.5f;
        public float MinEnemyRotation = 0.5f;
        public float MaxEnemyRotation = 1.5f;
        public int MaxEnemies = 10000;

        public string[] EnemyPrefabs;
        public int[] EnemyHP;
        public float[] EnemyMinSpeed;
        public float[] EnemyMaxSpeed;
        public float[] EnemyMinRotation;
        public float[] EnemyMaxRotation;

        public void LoadBalance()
        {
            TextAsset asset = Resources.Load("swarm_balance") as TextAsset;
            LoadBalance(asset.bytes);
        }

        public void LoadBalance(byte[] array)
        {
            Stream s = new MemoryStream(array);
            using (BinaryReader br = new BinaryReader(s))
            {
                int version = br.ReadInt32();

                PlayerSpeed = br.ReadSingle();
                MaxEnemies = br.ReadInt32();

                int numEnemies = br.ReadInt32();
                EnemyPrefabs = new string[numEnemies];
                EnemyHP = new int[numEnemies];
                EnemyMinSpeed = new float[numEnemies];
                EnemyMaxSpeed = new float[numEnemies];
                EnemyMinRotation = new float[numEnemies];
                EnemyMaxRotation = new float[numEnemies];
                for(int i = 0; i < numEnemies; i++)
                {
                    EnemyPrefabs[i] = br.ReadString();
                    EnemyHP[i] = br.ReadInt32();
                    EnemyMinSpeed[i] = br.ReadSingle();
                    EnemyMaxSpeed[i] = br.ReadSingle();
                    EnemyMinRotation[i] = br.ReadSingle();
                    EnemyMaxRotation[i] = br.ReadSingle();
                }
            }
        }
    }
}