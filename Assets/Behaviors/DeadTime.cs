using UnityEngine;

namespace Assets.Behaviors
{
    public class DeadTime : MonoBehaviour {

        public float DeadTimer = 3f;

        public void Init(float deadTimer)
        {
            DeadTimer = deadTimer;
        }

        void Awake () {
            Destroy(gameObject, DeadTimer);
        }
    }

}
