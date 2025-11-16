using System;
using UnityEngine;

namespace _Game.Scripts.Units.Skins
{
    [Serializable]
    public class UnitSkin
    {
        [SerializeField] private Material main;
        [SerializeField] private Material tail;

        public Material Main => main;
        public Material Tail => tail;
    }
}