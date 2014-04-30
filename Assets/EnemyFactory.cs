using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        }

        public void SetEnemiesQueue(Queue<List<Enemy>> enemiesQueue)
        {
            _enemiesQueue = enemiesQueue;
        }

        IEnumerator AddEnemies(IEnumerable<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                enemy.Init(_enemyData);
                yield return new WaitForSeconds(AddEnemy(enemy));
            }
            //return enemies.Select(enemy => new WaitForSeconds(AddEnemy(enemy))).GetEnumerator();
        }

        float AddEnemy(Enemy enemy)
        {
            //enemy.Init(_enemyData);
            _startVector = new Vector2(0f, 55f);

            var clone = (GameObject)Instantiate(Resources.Load("Enemies/" + enemy.Name), _startVector, Quaternion.identity);
            //AddDebugInfo(clone);

            var pathToFollow = new List<KeyValuePair<Vector3, Vector3>>()
            {
                AddBezierPath(new Vector2(70, 45)),
                AddBezierPath(new Vector2(-70, 0)),
                AddBezierPath(new Vector2(70, -23)),
            };

            // Add model
            //var mesh = Resources.Load<Mesh>("Models/" + enemy.Name);
            //clone.transform.FindChild("Mesh").gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

            // Add Behaviors
            clone.AddComponent<Behaviors.Enemy>().Init(enemy);
            clone.GetComponent<Behaviors.Enemy>().Paths = CreateBezierArrayFromList(pathToFollow);

            clone.GetComponentInChildren<MeshRenderer>().material.color = Colors.GetColorById(enemy.ColorId);
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

        void Update()
        {
            // If no enemies on the screen, create some
            if (_enemiesQueue.Any() && !GameObject.FindGameObjectsWithTag("Enemy").Any())
            {
                print("adding");
                StartCoroutine(AddEnemies(_enemiesQueue.Dequeue()));
            }
            // You killed them all, launch victory screen
            else
            {

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
