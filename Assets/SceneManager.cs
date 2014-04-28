using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Plugins;
using SimpleJSON;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class SceneManager : MonoBehaviour
    {
        public int Dificulty = 1;
        public float SpawnDelay = 1f;
        private float _spawnTimer;

        private EnemyFactory _enemyFactory;
        private GameObject _level;

        private MeshRenderer _background;

        public void Awake()
        {
            _background = GameObject.Find("Background").GetComponent<MeshRenderer>();
        }

        // Use this for initialization
        public void Start()
        {
            _enemyFactory = gameObject.AddComponent<EnemyFactory>();
            GenerateLevelJson(1);
        }

        public void GenerateLevelJson(int level)
        {
            _level = GameObject.Find("Level");
            Destroy(_level.GetComponent<LoadJsonData>());

            var levelData = _level.AddComponent<LoadJsonData>().GetLevelJson(1);
            var enemiesQueue = new Queue<List<Enemy>>();

            foreach (JSONNode spawn in levelData["spawns"].AsArray)
            {
                //------------ Add enemies to the queue ------------
                //var randomColor = Random.Range(0, 3);
                enemiesQueue.Enqueue(
                    Enumerable.Range(1, spawn[2].AsInt)
                        .Select(index => new Enemy(spawn[0], spawn[1].AsInt))
                        .ToList());
            }
            foreach (JSONNode element in levelData["background"]["elements"].AsArray)
            {
                AnimateBackgroundElement(element.Value);
            }

            //------------ Add background elements ------------
            _background.material.mainTexture = Resources.Load<Texture2D>(levelData["background"]["material"].Value);

            //------------ Send the queue to the factory ------------
            _enemyFactory.SetEnemiesQueue(enemiesQueue);

            //------------ Add tutorial messages ------------
            foreach (JSONNode tutorial in levelData["tutorials"].AsArray)
            {
                StartCoroutine(ShowTutorialMessage(tutorial["text"], tutorial["waitTime"].AsFloat));
            }
        }

        IEnumerator ShowTutorialMessage(string msg, float waitSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);
            TextManager.Show(msg, TextEffects.Size.Small, TextEffects.Effect.Tutorial);
        }


        public void AnimateBackgroundElement(string elementName)
        {
            Go.to(GameObject.Find(elementName).transform, 8f, new GoTweenConfig().positionPath(new GoSpline(new List<Vector3>()
            {
                new Vector3(Random.Range(-57, 57), 110, -4),
                new Vector3(Random.Range(-57, 57), -100, -4),
            })).onComplete(c => AnimateBackgroundElement(elementName)));
        }
	
        // Update is called once per frame
        public void Update()
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= SpawnDelay)
            {
                _spawnTimer = 0;// Reset timer
            }
        }


        public static void ShakeCamera(float magnitude)
        {
            var camera = GameObject.Find("Main Camera").transform;
            var oldPosition = camera.position;
            Go.to(camera, 0.5f, new GoTweenConfig().shake(new Vector2(magnitude, magnitude)))
                .setOnCompleteHandler(c => camera.position = oldPosition);
        }

    }
}
