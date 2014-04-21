using System;
using UnityEngine;

namespace Assets.Behaviors
{
    public class PowerUpSpeed : MonoBehaviour {

        private Player _player;
        private Shield _shield;

        public float LifeTime = 5f;
        public float ShootDelay = 0.1f;

        void Awake()
        {
        }

        void Start () {
            Destroy(this, LifeTime);
        }

        void Update()
        {
            
        }

        void OnDestroy()
        {
        }

    }
}
