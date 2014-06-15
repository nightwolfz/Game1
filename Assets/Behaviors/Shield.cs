using System.Collections;
using UnityEngine;

namespace Assets.Behaviors
{
    public class Shield : MonoBehaviour {

        private MeshRenderer _shield;
        private Player _player;

        void Start ()
        {
            _player = Player.Instance;
            _shield = Player.Instance.MeshComponent;
        }

        IEnumerator ReplenishShields(int restartDelay, float delay)
        {
            if (_player.TimeSinceTakingDamage < restartDelay)
            {
                yield return new WaitForSeconds(1f);
                _player.TimeSinceTakingDamage++;
            }
            else
            {
                yield return new WaitForSeconds(delay);
                if (_player.Shield == 0) PlayAnimation("up");
                if (_player.Shield < _player.MaxShield) _player.Shield += _player.ShieldRechargeAmmount;
                if (_player.Shield > _player.MaxShield) _player.Shield = _player.MaxShield;
            }
        }

        public IEnumerator ReplenishShields()
        {
            while (_player)
            {
                if (gameObject.GetComponent<PowerUpShield>())
                    yield return ReplenishShields(gameObject.GetComponent<PowerUpShield>().ShieldRestartDelay, gameObject.GetComponent<PowerUpShield>().ShieldDelay);
                else
                    yield return ReplenishShields(_player.ShieldRestartDelay, _player.ShieldDelay);
            }
        }

        public void PlayAnimation(string anim)
        {
            switch (anim)
            {
                case "down":
                    SceneManager.ShakeCamera(2f);
                    if (gameObject.GetComponent<PowerUpShield>()) return; // Shields dont go down when boosted
                    _shield.renderer.enabled = false;
                break;
                case "up":
                    _shield.renderer.enabled = true;
                    LeanTween.value(gameObject, AnimateShieldShader, 0, 0.2f, 0.2f).setOnComplete(c => AnimateShieldShader(0.2f));
                break;
                case "hit":
                    if (gameObject.GetComponent<PowerUpShield>()) return; // Shields dont flicker when boosted
                    LeanTween.value(gameObject, AnimateShieldShader, 1f, 4f, 0.2f).setOnComplete(c => AnimateShieldShader(0.2f));
                break;
                case "boost":
                    Debug.Log("boosted!");
                    LeanTween.value(gameObject, AnimateShieldShader, 2f, 10f, 0.2f);
                break;
                case "immortality":
                    LeanTween.value(gameObject, AnimateShieldShader, 2f, 10f, 0.2f);
                break;
            }
        }

        private void AnimateShieldShader(float strength)
        {
            _shield.material.SetFloat("_Strength", strength);
        }

    }
}
