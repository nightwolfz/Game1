using UnityEngine;

namespace Assets.Behaviors
{
    public class DeadTime : MonoBehaviour {

        public float DeadTimer = 3f;

        void Awake () {
            Destroy(gameObject, DeadTimer);
        }
    }

}
