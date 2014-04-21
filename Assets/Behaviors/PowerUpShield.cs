using System;
using UnityEngine;

namespace Assets.Behaviors
{
    public class PowerUpShield : MonoBehaviour {

        private Player _player;
        private Shield _shield;

        public float LifeTime = 5f;
        public float ShieldDelay = 0.05f;
        public int ShieldRestartDelay = 1;

        void Awake()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        public void Start () {
            _player.ShieldComponent.PlayAnimation("boost");
            _player.Shield = _player.MaxShield;
            Destroy(this, LifeTime);
        }

        void OnDestroy()
        {
            _player.ShieldComponent.PlayAnimation("up");
        }

    }
}
