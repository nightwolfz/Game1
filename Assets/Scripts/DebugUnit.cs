using UnityEngine;

namespace Assets.Scripts
{
    [AddComponentMenu("Gameplay/DebugUnit")]
    class DebugUnit : MonoBehaviour
    {
        // Use this for initialization
        public void Start()
        {
            guiText.text = "eeeeeee";
        }


        // Update is called once per frame
        public void Update()
        {
            //gameObject.guiText.text = gameObject.GetComponent<Enemy>().Health.ToString();
            //gameObject.guiText.text = "eeeeeee";
        }
    }
}
