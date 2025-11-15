
using UnityEngine;

namespace _Game.Scripts.Multiplayer.Schemas
{
    public static class SchemaExtensions
    {
        public static Vector3 ToVector3(this Vector3Schema vector3)
        {
            return new Vector3(vector3.x, vector3.y, vector3.z);
        }
        public static Vector2 ToVector2(this Vector2Schema vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
    }
}