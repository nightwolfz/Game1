using UnityEngine;

namespace Assets.Behaviors
{
    public class AnimateUv : MonoBehaviour {

        public Vector2 UvAnimationRate = new Vector2(0f, 1f);
        private Vector2 _offset = Vector2.zero;

        void Update()
        {
            _offset += (UvAnimationRate * Time.deltaTime);
            if (renderer.enabled)
            {
                renderer.material.SetTextureOffset("_MainTex", _offset);
            }
        }
    }
}
