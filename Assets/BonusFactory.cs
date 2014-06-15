using Assets.Behaviors;
using UnityEngine;

namespace Assets
{
    public class BonusFactory : MonoBehaviour
    {
        private float _spawnInterval = 2f;
        private float _spawnTimer;

        void Start () {
	
        }

        void Spawn()
        {
            var startVector = new Vector2(Random.Range(-30f, 30f), 50f);
            var o = (GameObject)Instantiate(Resources.Load("PowerUp"), startVector, Quaternion.identity);
            //o.transform.FindChild("Cube").gameObject.AddComponent<PowerUp>();
            //o.AddComponent<PowerUp>();
        }

        void Move()
        {
            //transform.position += transform.TransformDirection(new Vector2(0, 2)) * Time.deltaTime;
        }
	

        void Update () {
            //----------- Shooting -----------
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnInterval)
            {
                Spawn();
                _spawnTimer = 0;
            }
        }
    }
}
