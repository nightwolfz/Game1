using System;
using UnityEngine;

namespace Assets.Behaviors
{
    public class Bullet : MonoBehaviour {

        public int Damage = 5, Speed = 10, ColorId = 0;
        public int Velocity = 20;
        public float Rotation = 0f;

        private float _timeLeft = 3;
        protected bool DamageDone = false; // So we only damage once
        private Player _player;
        private Shield _shield;

        void Start()
        {
            _player = Player.Instance;
            _shield = Player.Instance.ShieldComponent;

            if (gameObject.tag == "EnemyBullet"){
                Speed /= (int)2; // Slow enemy bullets
                Damage /= (int)2; // Weak enemy bullets
            }

            //transform.GetComponent<SpriteRenderer>().sprite.texture.
            //transform.rotation = new Quaternion(0f,0f,0f,344f);
            transform.Rotate(0, 0, Rotation);

            SoundManager.Instance.Play(SoundManager.Instance.BulletSound);
        }
	
        void Update()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0 || DamageDone)
            {
                DestroyBullet();
            }
            //transform.Rotate(new Vector3(0, 0, 344f)); //344, 24 //cool effect
            transform.position += transform.TransformDirection(new Vector2(0, Velocity)) * (Time.deltaTime * Speed);
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
