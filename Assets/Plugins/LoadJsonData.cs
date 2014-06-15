using System;
using System.Xml;
using SimpleJSON;
using UnityEngine;

namespace Assets.Plugins
{
    public class LoadJsonData : MonoBehaviour
    {
        private JSONNode _pathFile;
        private JSONNode _enemiesFile;

        public JSONNode GetLevelData(int level)
        {
            // On level start, load enemies and paths
            _pathFile = JSON.Parse(Resources.Load<TextAsset>("Data/Paths").text);
            _enemiesFile = JSON.Parse(Resources.Load<TextAsset>("Data/Enemies").text);

            // Load level data
            var json = JSON.Parse(Resources.Load<TextAsset>("Data/" + level).text);
            if (json == null) Debug.LogError("Could not load '" + level + ".json'");
            return json;
        }
        
        public JSONNode GetEnemiesData()
        {
            if (_enemiesFile == null) Debug.LogError("Could not load 'Enemies.json'");
            return _enemiesFile;
        }

        public JSONNode GetPathData()
        {
            if (_pathFile == null) Debug.LogError("Could not load 'Paths.json'");
            return _pathFile;
        }

        public JSONNode GetPathData(string pathName)
        {
            if (GetPathData()[pathName] == null) Debug.LogError("Could not load element '" + pathName + "' in 'Paths.json'");
            return GetPathData()[pathName].AsArray;
        }

    }
}
