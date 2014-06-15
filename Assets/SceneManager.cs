using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Behaviors;
using Assets.Plugins;
using SimpleJSON;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class SceneManager : MonoBehaviour
    {
        public int Level = 1, Dificulty = 1;

        private float _delayTime = 1f, _delayedTimer;
        private bool _gamePaused = false;

        private EnemyFactory _enemyFactory;
        private GameObject _level, _loadingScreen;
        private Camera _mainCamera, _menuCamera;
        private MeshRenderer _background;
        public JSONNode LevelData { get; set; }

        // Singleton  
        private static SceneManager _instance;
        public static SceneManager Instance { get { return _instance ?? (_instance = FindObjectOfType<SceneManager>()); } }

        public void Awake()
        {
            _background = GameObject.Find("Background").GetComponent<MeshRenderer>();
            _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            _menuCamera = GameObject.Find("Menu Camera").GetComponent<Camera>();
            _loadingScreen = GameObject.Find("Menu Camera");
            _level = GameObject.Find("Level");
        }

        // Use this for initialization
        public void Start()
        {
            LoadLevel(Level);
        }

        public void LoadNextLevel()
        {
            /*LeanTween.moveY(Player.Instance.gameObject, 10, 0.5f)
            .setOnComplete(c => {
                LeanTween.moveY(Player.Instance.gameObject, -41, 0f).setOnComplete(cc =>
                {
                    LoadLevel(++Level);
                });
            });*/
            LoadLevel(++Level);
        }

        public void LoadLevel(int level)
        {
            // Load level data
            if (_level.GetComponent<LoadJsonData>()) Destroy(_level.GetComponent<LoadJsonData>());
            LevelData = _level.AddComponent<LoadJsonData>().GetLevelData(level);

            // Generate stuff
            StartCoroutine(GenerateEnemies());
            StartCoroutine(GenerateLevel(level));
            StartCoroutine(GenerateMessages());
        }

        IEnumerator GenerateEnemies()
        {
            if (gameObject.GetComponent<EnemyFactory>()) Destroy(gameObject.GetComponent<EnemyFactory>());
            _enemyFactory = gameObject.AddComponent<EnemyFactory>();

            //------------ Add enemies to the queue ------------
            yield return new WaitForSeconds(3f);
            var enemiesQueue = new Queue<List<Enemy>>();

            foreach (JSONNode spawn in LevelData["spawns"].AsArray)
            {
                //var randomColor = Random.Range(0, 3);
                enemiesQueue.Enqueue(
                    Enumerable.Range(1, spawn[2].AsInt)
                        .Select(index => new Enemy(spawn[0], spawn[1].AsInt))
                        .ToList());
            }

            //------------ Send the queue to the factory ------------
            _enemyFactory.SetEnemiesQueue(enemiesQueue);
            yield return null;
        }

        IEnumerator GenerateMessages()
        {
            // Loading screen
            LeanTween.alpha(_loadingScreen, 0, 1f);
            TextManager.Show(LevelData["name"], TextEffects.Size.Big, TextEffects.Effect.Slow);

            //------------ Add tutorial messages ------------
            yield return new WaitForSeconds(1f);
            foreach (JSONNode tutorial in LevelData["tutorials"].AsArray)
            {
                StartCoroutine(ShowTutorialMessage(tutorial["text"], tutorial["waitTime"].AsFloat));
            }
        }

        IEnumerator GenerateLevel(int level)
        {
            //------------ Add background elements ------------
            _background.material.mainTexture = Resources.Load<Texture2D>(LevelData["background"]["material"].Value);

            foreach (JSONNode element in LevelData["background"]["elements"].AsArray)
            {
                AnimateBackgroundElement(element.Value);
            }
            yield return null;
        }

        IEnumerator ShowTutorialMessage(string msg, float waitSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);
            TextManager.Show(msg, TextEffects.Size.Small, TextEffects.Effect.Tutorial);
        }


        void AnimateBackgroundElement(string elementName)
        {
            Go.to(GameObject.Find(elementName).transform, 8f, new GoTweenConfig().positionPath(new GoSpline(new List<Vector3>()
            {
                new Vector3(Random.Range(-57, 57), 110, -4),
                new Vector3(Random.Range(-57, 57), -100, -4),
            })).onComplete(c => AnimateBackgroundElement(elementName)));
        }
	
        // Update is called once per frame
        void Update()
        {
            _delayedTimer += Time.deltaTime;
            if (_delayedTimer >= _delayTime)
            {
                _delayedTimer = 0;// Reset timer
            }

            // Pause Menu
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                PauseGame();
            }
        }


        public static void ShakeCamera(float magnitude)
        {
            var camera = GameObject.Find("Main Camera").transform;
            var oldPosition = camera.position;
            Go.to(camera, 0.5f, new GoTweenConfig().shake(new Vector2(magnitude, magnitude)))
                .setOnCompleteHandler(c => camera.position = oldPosition);
        }

        public void PauseGame()
        {
            _gamePaused = !_gamePaused;
            _mainCamera.enabled = !_gamePaused;
            _menuCamera.enabled = _gamePaused;
            Time.timeScale = _gamePaused ? 0f : 1f;
            AudioListener.pause = _gamePaused;
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject thisObject in allObjects)
                if (_gamePaused) LeanTween.pause(thisObject); else LeanTween.resume(thisObject);
        }
    }
}
