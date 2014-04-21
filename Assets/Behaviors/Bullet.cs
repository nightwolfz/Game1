using System;
using Assets.Plugins;
using UnityEngine;

namespace Assets.Behaviors
{
    public class Bullet : MonoBehaviour {

        private float _timeLeft = 3;

        public int Damage = 5;
        public int Speed = 10;
        public Vector2 Velocity = Vector2.zero;
        protected int ColorId = 0;
        protected bool DamageDone = false; // So we only damage once
        private Player _player;
        private Shield _shield;

        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            _shield = _player.ShieldComponent;

            if (gameObject.tag == "EnemyBullet"){
                Speed /= (int)2; // Slow enemy bullets
                Damage /= (int)2; // Weak enemy bullets
            }
        }
	
        void Update()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0 || DamageDone)
            {
                DestroyBullet();
            }

            transform.position += transform.TransformDirection(Velocity) * (Time.deltaTime * Speed);
        }

        int GetDamageWithMultiplier(int colorId2)
        {
            if (ColorId == colorId2) return Damage * 4; // Same color x4
            return ColorId == 0 ? Damage * 2 : Damage; // If neutral x2 else x1
        }

        void DestroyBullet()
        {
            DamageDone = true;
            //ObjectPool.instance.PoolObject(gameObject);
            //PoolManager.Despawn(gameObject);
            Destroy(gameObject);
        }

        void DamagePlayer()
        {
            if (_player.Dead)
            {
                DestroyBullet(); 
                return;
            }

            if (_player.Shield > 0)
            {
                _player.Shield -= Damage;

                if (_player.Shield <= 0)
                {
                    Damage = Math.Abs(_player.Shield);
                    _player.Shield = 0;
                    _player.Health -= Damage;
                    _shield.PlayAnimation("down");
                }
                else
                {
                    _shield.PlayAnimation("hit");
                }
            }
            else _player.Health -= Damage;
            _player.TimeSinceTakingDamage = 0;
        }

        void DamageEnemy(Enemy enemy)
        {
            if (enemy)
            {
                enemy.Health -= GetDamageWithMultiplier(enemy.ColorId);
                _player.Credits += enemy.Credits;
            }
            DestroyBullet();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (gameObject.tag == "PlayerBullet" && other.gameObject.tag == "Enemy") DamageEnemy(other.gameObject.GetComponent<Enemy>());
            if (gameObject.tag == "EnemyBullet" && other.gameObject.tag == "Player") DamagePlayer();
        }

    }
}
