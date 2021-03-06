﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Behaviors
{
    public class Player : MonoBehaviour {

        private float _direction;
        private bool _shooting;

        private BulletFactory _bulletFactory;

        public float ShootDelay = 0.4f;
        public float ShieldDelay = 0.3f;
        public int ShieldRestartDelay = 5;
        public int ShieldRechargeAmmount = 1;
	
        public int Health = 100;
        public int MaxHealth = 100;
        public int Shield = 20;
        public int MaxShield = 20;
        public int Credits = 0;
        public int TimeSinceTakingDamage;
        public int ColorId = 0;
        public int DiedTimes = 0;
        public bool Dead = false;
        public bool PoweredUp = false;

        public Shield ShieldComponent;
        public MeshRenderer MeshComponent;

        public int HealthPerc
        {
            get { return Health / MaxHealth; }
        }
        public Color GetColor
        {
            get { return Colors.GetColorById(ColorId); }
        }

        // Singleton  
        private static Player _instance;
        public static Player Instance { get { return _instance ?? (_instance = FindObjectOfType<Player>()); } }

        void Start ()
        {
            ShieldComponent = gameObject.GetComponent<Shield>();
            MeshComponent = gameObject.transform.FindChild("Mesh").GetComponent<MeshRenderer>();

            gameObject.AddComponent<BulletFactory>().Init(
                new List<Weapon>()
                {
                    new Weapon("Bullet", "L"),
                    new Weapon("Gattling", "U"),
                    new Weapon("Bullet", "R"),
                }, false);
            StartCoroutine(ShieldComponent.ReplenishShields());
        }

        IEnumerator ShootNow()
        {
            _shooting = true;
            gameObject.GetComponent<BulletFactory>().Shoot(ColorId);

            yield return new WaitForSeconds(gameObject.GetComponent<PowerUpSpeed>()
                ? gameObject.GetComponent<PowerUpSpeed>().ShootDelay
                : ShootDelay);

            _shooting = false;
        }

        void SetSpriteAlpha(float val)
        {
            MeshComponent.material.color = new Color(GetColor.r, GetColor.g, GetColor.b, val);
        }

        IEnumerator Respawn()
        {
            var animTime = 0.5f;
            Health = 0;

            for (int i = 0; i < 5; i++)
            {
                LeanTween.value(gameObject, SetSpriteAlpha, 0.8f, 0.2f, animTime).setOnComplete(() =>
                LeanTween.value(gameObject, SetSpriteAlpha, 0.2f, 0.8f, animTime));
                yield return new WaitForSeconds(animTime * 2);
            }
            Dead = false;
            Health = MaxHealth;
            Shield = MaxShield;
            Debug.Log("Respawning...");
        }

        void Update ()
        {
            if (Dead == false)
            {
                if (_shooting == false) StartCoroutine(ShootNow());
                if (Health <= 0) StartCoroutine(Respawn());
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                Health -= 5;
                other.gameObject.GetComponent<Enemy>().Die();
            }
        }

    }
}
