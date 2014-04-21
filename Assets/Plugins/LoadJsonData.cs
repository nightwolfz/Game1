using System;
using System.Xml;
using SimpleJSON;
using UnityEngine;

namespace Assets.Plugins
{
    public class LoadJsonData : MonoBehaviour 
    {
        public JSONNode GetLevelJson(int level)
        {
            var levelXml = Resources.Load<TextAsset>("Levels/" + level);
            var json = JSON.Parse(levelXml.text);
            if (json == null) Debug.LogError("Could not load '" + level + ".json'");
            return json;
        }
    }
}
