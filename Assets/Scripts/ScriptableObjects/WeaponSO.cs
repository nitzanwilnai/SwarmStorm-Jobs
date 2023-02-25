using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    public enum WEAPON_TYPE { PROJECTILE, BEAM };

    [CreateAssetMenu(fileName = "Weapon", menuName = "Swarm/Weapon", order = 1)]
    public class WeaponSO : ScriptableObject
    {
        [HideInInspector]public int ID;

        public WEAPON_TYPE WeaponType;
        public float Speed;
        public float ExplosionRadius;
        public bool Homing;
        public float Damage;
        public bool Freeze;
    }
}