using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Units.Skins.Enums;
using UnityEngine;

namespace _Game.Scripts.Units.Skins
{
    public class UnitSkinLoader : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private List<SnakePart> skinParts = new();

        #endregion

        #region FIELDS

        private UnitSkin _currentSkin;

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public void RemoveSkinPart(SnakePart part)
        {
            if (skinParts.Contains(part))
                skinParts.Remove(part);
        }

        public void AddSkinPart(SnakePart part)
        {
            skinParts.Add(part);

            if (_currentSkin != null)
                SetPartMaterial(part, _currentSkin);
        }

        public void LoadSkin(UnitSkin unitSkin)
        {
            _currentSkin = unitSkin;

            foreach (var part in skinParts)
            {
                SetPartMaterial(part, unitSkin);
            }
        }

        private void SetPartMaterial(SnakePart part, UnitSkin unitSkin)
        {
            switch (part.Type)
            {
                case SnakePartType.Tail:
                    part.SetMaterial(unitSkin.Tail);
                    break;
                case SnakePartType.Head:
                case SnakePartType.Detail:
                    part.SetMaterial(unitSkin.Main);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}