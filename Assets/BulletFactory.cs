using System.Collections.Generic;
using Assets.Behaviors;
using Assets.Plugins;
using UnityEngine;

namespace Assets
{
    class BulletFactory : MonoBehaviour {

        private bool _reverse;
        private List<Weapon> _weapons;
        private Vector2 _moveDirection;
        private int _colorId = 0;
        private Color _color;
        private GameObject _bulletContainer;

        public void Init(List<Weapon> weapons, bool reverse)
        {
            _reverse = reverse;
            _weapons = weapons;
            _bulletContainer = GameObject.Find("BulletContainer");
        }

        // Return a list of all created entities so we can add them to the scene
        public void Shoot(int colorId = 0)
        {
            _colorId = colorId;
            foreach (var weapon in _weapons) AddBullet(weapon);
        }

        private void AddBullet(Weapon weapon)
        {
            var direction = _reverse ? -1 : 1;

            //var clone = ObjectPool.instance.GetObjectForType(weapon.Name, false, transform.position);
            //var clone = PoolManager.Spawn(weapon.Name);
            //if (clone == null) return;

            var clone = (GameObject)Instantiate(Resources.Load<GameObject>(weapon.Name), gameObject.transform.position, Quaternion.identity);
            clone.tag = _reverse ? "EnemyBullet" : "PlayerBullet";
            clone.transform.parent = _bulletContainer.transform;

            // Adapt Colors
            _colorId = _reverse ? 4 : _colorId; // Enemies get orange bullets
            _color = Colors.GetColorById(_colorId);

            clone.GetComponent<SpriteRenderer>().color = Colors.GetColorById(_colorId);
            clone.GetComponent<TimedTrailRenderer>().Sizes = new[] { 2f, 1f, 0.5f };
            clone.GetComponent<TimedTrailRenderer>().Colors = new[]
            {
                new Color(_color.r, _color.g, _color.b, 0.8f), 
                new Color(_color.r, _color.g, _color.b, 0.4f),
                new Color(_color.r, _color.g, _color.b, 0.2f),
            };

            // L=left, R=right
            switch (weapon.Behavior)
            {
                case "L":
                    _moveDirection = new Vector2(-5, 20 * direction);
                    break;
                case "LL":
                    _moveDirection = new Vector2(-20, 20 * direction);
                    break;
                case "R":
                    _moveDirection = new Vector2(5, 20 * direction);
                    break;
                case "RR":
                    _moveDirection = new Vector2(20, 20 * direction);
                    break;
                default:
                    _moveDirection = new Vector2(0, 20 * direction);
                    break;
            }

            clone.GetComponent<Bullet>().Velocity = _moveDirection;
        }

    }
}
