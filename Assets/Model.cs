using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;

namespace Assets
{
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
        public float ShootDelay = 2f;

        public List<Weapon> Weapons;

        public Enemy(string name, int colorId)
        {
            Name = name;
            ColorId = colorId;
        }

        public void Init(JSONNode enemyData)
        {
            var data = enemyData[Name];
            if (data["hp"] != null) Health = data["hp"].AsInt;
            if (data["sp"] != null) Shield = data["sp"].AsInt;
            if (data["delay"] != null) ShootDelay = data["delay"].AsFloat;
            Weapons = (from JSONNode weapon in data["weapons"].AsArray select new Weapon(weapon[1], weapon[0])).ToList();
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
