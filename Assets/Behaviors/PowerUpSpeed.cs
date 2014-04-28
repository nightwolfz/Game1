using System;
using Assets.Plugins;
using UnityEngine;

namespace Assets.Behaviors
{
    public class PowerUpSpeed : MonoBehaviour {

        private Player _player;
        private Shield _shield;

        public float LifeTime = 5f;
        public float ShootDelay = 0.1f;

        private TextEffects _flashText;

        void Start() {
            Destroy(this, LifeTime);
            TextManager.Show("Power Up!", TextEffects.Size.Big, TextEffects.Effect.Slow);
        }

        void Update()
        {
            
        }

        void OnDestroy()
        {
        }

    }
}
