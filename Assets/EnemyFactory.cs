using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Behaviors;
using Assets.Plugins;
using SimpleJSON;
using UnityEngine;

namespace Assets
{
    public class EnemyFactory : MonoBehaviour
    {
        private float _spawnInterval = .5f;
        private Vector2 _startVector;
        private Queue<List<Enemy>> _enemiesQueue = new Queue<List<Enemy>>();
        private JSONNode _enemyData;

        void Start()
        {
            _enemyData = GameObject.Find("Level").GetComponent<LoadJsonData>().GetEnemiesData();

            StopCoroutine("SpawnNewEnemies");
            StartCoroutine(SpawnNewEnemies());
        }

        public void SetEnemiesQueue(Queue<List<Enemy>> enemiesQueue)
        {
            _enemiesQueue = enemiesQueue;
        }

        IEnumerator AddEnemies(IEnumerable<Enemy> enemies)
        {
            return enemies.Select(enemy => new WaitForSeconds(AddEnemy(enemy))).GetEnumerator();
        }

        float AddEnemy(Enemy enemy)
        {
            enemy.Init(_enemyData);
            _startVector = new Vector2(0f, 55f);

            var clone = (GameObject)Instantiate(Resources.Load("Enemies/" + enemy.Name), _startVector, Quaternion.identity);
            //AddDebugInfo(clone);

            var pathToFollow = new List<KeyValuePair<Vector3, Vector3>>()
            {
                AddBezierPath(new Vector2(70, 45)),
                AddBezierPath(new Vector2(-70, 0)),
                AddBezierPath(new Vector2(70, -23)),
            };

            // Add Behaviors
            clone.AddComponent<Behaviors.Enemy>().Init(enemy);
            clone.GetComponent<Behaviors.Enemy>().Paths = CreateBezierArrayFromList(pathToFollow);

            clone.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Colors.GetColorById(enemy.ColorId));
            clone.AddComponent<BulletFactory>().Init(enemy.Weapons, true);

            return _spawnInterval;
        }

        private void AddDebugInfo(GameObject obj)
        {
            var debug = (GameObject)Instantiate(Resources.Load("UiDebug"), obj.transform.position, Quaternion.identity);
            debug.transform.position = obj.transform.position;
            debug.transform.parent = obj.transform;
            debug.AddComponent<Scripts.DebugUnit>();
        }


        IEnumerator SpawnNewEnemies()
        {
            yield return new WaitForSeconds(0f);

            while (true)
            {
                yield return new WaitForSeconds(2f);

                //----------- If no enemies on the screen, create some ----------------
                if (_enemiesQueue.Any() && !FindObjectsOfType<Behaviors.Enemy>().Any())
                {
                    StartCoroutine(AddEnemies(_enemiesQueue.Dequeue()));
                }

                //----------- You killed them all, launch victory screen --------------
                if (!_enemiesQueue.Any() && !FindObjectsOfType<Behaviors.Enemy>().Any())
                {
                    Debug.Log("You killed em all, next level!");
                    SceneManager.Instance.LoadNextLevel();
                }
            }
        }

        #region Bezier stuff

            private Vector3[] CreateBezierArrayFromList(IEnumerable<KeyValuePair<Vector3, Vector3>> vectorList)
            {
                var vectorArray = new List<Vector3> { };
                foreach (var vector in vectorList)
                    vectorArray.AddRange(new List<Vector3> { vector.Key, Vector2.zero, Vector2.zero, vector.Value });
                return vectorArray.ToArray();
            }

            private KeyValuePair<Vector3, Vector3> AddBezierPath(Vector2 gotoVector)
            {
                var path = new KeyValuePair<Vector3, Vector3>(_startVector, gotoVector);
                _startVector = gotoVector;
                return path;
            }

        #endregion


    }
}
