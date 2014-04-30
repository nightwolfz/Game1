using UnityEngine;

namespace Assets.Scripts
{
    [AddComponentMenu("Gameplay/Rotate Unit")]
    public class RotateUnit : MonoBehaviour
    {
        private GameObject _mesh;

        void Start ()
        {
            _mesh = transform.FindChild("Mesh").gameObject;
        }
	
        void Update () {
            _mesh.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 100);
        }
    }
}
