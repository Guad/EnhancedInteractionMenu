using System;
using System.Collections.Generic;
using GTA.Math;

namespace EnhancedInteractionMenu
{
    public static class PointsOfInterest
    {
        private static Dictionary<Type, Vector3[]> _database = new Dictionary<Type, Vector3[]>
        {
            { Type.AmmuNation,   new Vector3[]{
                new Vector3(16.1591f, -1127.586f, 28.8284f),
                new Vector3(814.7812f, -2137.877f, 29.2872f),
                new Vector3(1704.4933f, 3745.8464f, 33.8131f),
                new Vector3(234.4493f, -40.1616f, 69.7064f),
                new Vector3(843.7763f, -1012.518f, 27.8977f),
                new Vector3(-320.4078f, 6071.7251f, 31.3350f),
                new Vector3(-664.2806f, -951.3289f, 21.2968f),
                new Vector3(-1326.567f, -389.2455f, 36.5553f),
                new Vector3(-1108.508f, 2686.1399f, 18.8496f),
                new Vector3(-3155.475f, 1079.7930f, 20.6908f),
                new Vector3(2568.2351f, 309.1174f, 108.4582f),
            }},
            { Type.LosSantosCustoms,   new Vector3[]{
                new Vector3(-1137.581f, -1983.400f, 13.1600f),
                new Vector3(713.5993f, -1088.852f, 22.3695f),
                new Vector3(-369.8407f, -129.2884f, 38.6695f),
                new Vector3(1181.5710f, 2654.4570f, 37.8072f),
            }},
        };

        public enum Type
        {
            AmmuNation,
            LosSantosCustoms
        }

        public static Vector3 GetClosestPoi(Vector3 relativeTo, Type type)
        {
            var smallest = float.MaxValue;
            Vector3 output = new Vector3();
            foreach (var vector3 in _database[type])
            {
                var len = (vector3 - relativeTo).Length();
                if (len < smallest)
                {
                    smallest = len;
                    output = vector3;
                }
            }
            return output;
        }
        
    }
}