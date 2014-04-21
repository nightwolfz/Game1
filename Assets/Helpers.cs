using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Assets
{
    static class Helpers
    {
        public static int SetDefaultValue(XmlNode node, string attribute, int defaultValue = 0)
        {
            if (node.Attributes != null && node.Attributes[attribute] != null)
            {
                return int.Parse(node.Attributes[attribute].Value);
            }
            return defaultValue;
        }
        public static string SetDefaultValue(XmlNode node, string attribute, string defaultValue = "")
        {
            if (node.Attributes != null && node.Attributes[attribute] != null)
            {
                return node.Attributes[attribute].Value;
            }
            return defaultValue;
        }
    }

    public class Weapon
    {
        public string Name;
        public string Behavior;

        public Weapon(string name, string behavior)
        {
            Name = name;
            Behavior = behavior;
        }
    }

    public class Enemy
    {
        public string Name = "Enemy";
        public int ColorId = 0;
        public int Health = 30;
        public int Shield = 0;

        public Enemy(string name, int colorId)
        {
            ColorId = colorId;
            Name = name;
        }
    }

    static class Colors
    {
        public static Color Red = new Color32(255, 11, 11, 255);
        public static Color Green = new Color32(106, 232, 32, 255);
        public static Color Blue = new Color32(56, 154, 255, 255);
        public static Color White = new Color32(255, 255, 255, 255);
        public static Color Orange = new Color32(255, 216, 0, 255);

        public static Color GetRandomColor()
        {
            return GetColorById(Random.Range(0, 3));
        }

        public static Color GetColorById(int id)
        {
            switch (id)
            {
                case 1: return Red;
                case 2: return Green;
                case 3: return Blue;
                case 4: return Orange;
                default: return White;
            }
        }
    }

}
