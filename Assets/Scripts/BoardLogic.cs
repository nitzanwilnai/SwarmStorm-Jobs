using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonTools;
using System;

namespace Swarm
{
    public static class BoardLogic
    {
        public static uint IncrementSeed(uint seed)
        {
            return (214013 * seed + 2531011);
        }

        public static uint RandInt(ref uint seed)
        {
            uint value = (seed >> 16) & 0x7FFF;
            seed = IncrementSeed(seed);
            return value;
        }

        public static uint RandIntRange(uint min, uint max, ref uint seed)
        {
            uint value = (seed >> 16) & 0x7FFF;
            uint range = max - min + 1;
            value = value % range + min;
            seed = IncrementSeed(seed);
            return value;
        }

        public static float RandFloat(ref uint seed)
        {
            return RandInt(ref seed) / (float)0x7FFF;
        }

        public static int GetSeedForCurrentTime()
        {
            long currentTime = GetCurrentTimeSec();
            return (int)(currentTime % int.MaxValue);
        }

        public static long GetCurrentTimeSec()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
        }

        public static long GetCurrentTimeMS()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static void Allocate(GameData gameData, Balance balance)
        {
            gameData.EnemyCount = balance.MaxEnemies;
            gameData.EnemyPos = new Vec2[gameData.EnemyCount];
            gameData.EnemyCurrentDir = new Vec2[gameData.EnemyCount];
            gameData.EnemyTargetDir = new Vec2[gameData.EnemyCount];
            gameData.EnemySpeed = new float[gameData.EnemyCount];
            gameData.EnemyRotation = new float[gameData.EnemyCount];
        }

        public static void StartGame(GameData gameData, Balance balance, ref uint seed)
        {
            gameData.PlayerPos = Vec2.Zero();
            for (int i = 0; i < gameData.EnemyCount; i++)
            {
                gameData.EnemyPos[i] = new Vec2(RandFloat(ref seed) * 4.0f - 2.0f, RandFloat(ref seed) * 4.0f - 2.0f);
                gameData.EnemyCurrentDir[i] = gameData.EnemyTargetDir[i] = (gameData.PlayerPos - gameData.EnemyPos[i]).Normal();
                gameData.EnemySpeed[i] = RandFloat(ref seed) * (balance.MaxEnemySpeed - balance.MinEnemySpeed) + balance.MinEnemySpeed;
                gameData.EnemyRotation[i] = RandFloat(ref seed) * (balance.MaxEnemyRotation - balance.MinEnemyRotation) + balance.MinEnemyRotation;
            }
        }

        public static void SetPlayerMoveDirection(GameData gameData, Vec2 direction)
        {
            gameData.PlayerDir = direction;
            if (gameData.PlayerDir.Magnitude() > 1.0f)
                gameData.PlayerDir.Normalize();
        }

        static void movePlayer(GameData gameData, Balance balance, float dt)
        {
            Vec2 pos = gameData.PlayerPos + gameData.PlayerDir * dt * balance.PlayerSpeed;
            Vec2 newPos = gameData.PlayerPos;

            // try X
            if (TryMove(gameData, balance, new Vec2(pos.x, gameData.PlayerPos.y)))
                newPos.x = pos.x;
            if (TryMove(gameData, balance, new Vec2(gameData.PlayerPos.x, pos.y)))
                newPos.y = pos.y;

            gameData.PlayerPos = newPos;
        }

        private static bool TryMove(GameData gameData, Balance balance, Vec2 pos)
        {
            return true;
        }

        public static void Tick(GameData gameData, Balance balance, float dt)
        {
            movePlayer(gameData, balance, dt);

            for (int i = 0; i < gameData.EnemyCount; i++)
            {
                gameData.EnemyTargetDir[i] = (gameData.PlayerPos - gameData.EnemyPos[i]).Normal();

                double angle = Vec2.Angle(gameData.EnemyCurrentDir[i], gameData.EnemyTargetDir[i]);
                if (angle > 1.0d)
                    gameData.EnemyCurrentDir[i].Rotate(1.0d * gameData.EnemyRotation[i]);
                else if (angle < 1.0d)
                    gameData.EnemyCurrentDir[i].Rotate(-1.0d * gameData.EnemyRotation[i]);
                else
                    gameData.EnemyCurrentDir[i].Rotate(angle);
                gameData.EnemyCurrentDir[i].Normalize();

                gameData.EnemyPos[i] += gameData.EnemyCurrentDir[i] * dt * gameData.EnemySpeed[i];
            }
        }
    }
}