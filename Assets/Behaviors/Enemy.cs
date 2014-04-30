using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Behaviors
{
    public class Enemy : MonoBehaviour 
    {
        private BulletFactory _bulletFactory;

        private float _shootDelay;
        private float _shootDelayTimer;

        private bool _enemyDestroyed;

        public int Health = 100;
        public int Shield;
        public int ColorId;
        public int Credits;
        public Vector3[] Paths;

        private GameObject _enemy;
        private GoTweenChain _chain;
        private Vector2 _previousVector2;

        protected LTDescr Tween;

        void Start()
        {
            Tween = LeanTween.move(gameObject, Paths, Paths.Count());
            Tween.setRepeat(999).setOnComplete(() => Destroy(gameObject));
        }

        public void Init(Assets.Enemy enemy)
        {
            Health = enemy.Health;
            Shield = enemy.Shield;
            Credits = Health;
            ColorId = enemy.ColorId;
            _shootDelay = enemy.ShootDelay;
        }
	
        void Update()
        {
            if (Health <= 0 && !_enemyDestroyed)
            {
                StartCoroutine(Die());
                return;
            }

            //----------- Shooting -----------
            _shootDelayTimer += Time.deltaTime;
            if (_shootDelayTimer >= _shootDelay)
            {
                gameObject.GetComponent<BulletFactory>().Shoot();
                _shootDelayTimer = 0;
            }
        }


        public IEnumerator Die()
        {
            _enemyDestroyed = true;
            var animationTime = 0.5f;

            Tween.cancel();

            Go.to(transform, animationTime, new GoTweenConfig().shake(new Vector2(2, 2)));

            Instantiate(Resources.Load<GameObject>("Effects/Explosions/Destruction01"), transform.position, Quaternion.identity);

            TextManager.Show("+" + Credits, transform.position, Colors.GetColorById(ColorId));

            //SceneManager.ShakeCamera(animationTime);
            yield return new WaitForSeconds(animationTime);
            Destroy(gameObject);
        }

    }
}
