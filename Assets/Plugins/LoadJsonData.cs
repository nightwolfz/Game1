using System;
using System.Xml;
using SimpleJSON;
using UnityEngine;

namespace Assets.Plugins
{
    public class LoadJsonData : MonoBehaviour 
    {
        public JSONNode GetLevelData(int level)
        {
            var res = Resources.Load<TextAsset>("Data/" + level);
            var json = JSON.Parse(res.text);
            if (json == null) Debug.LogError("Could not load '" + level + ".json'");
            return json;
        }
        
        public JSONNode GetEnemiesData()
        {
            var res = Resources.Load<TextAsset>("Data/Enemies");
            var json = JSON.Parse(res.text);
            if (json == null) Debug.LogError("Could not load 'Enemies.json'");
            return json;
        }

    }
}
