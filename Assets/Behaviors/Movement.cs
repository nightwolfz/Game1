using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Behaviors
{
    public class Movement : MonoBehaviour {
	
        public int SPEED = 100;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector2 _eulerAngle;
        private Vector2 _input;

        private float _clampToScreen; // How far can the player move

        private MeshRenderer _mesh;

        void Awake()
        {
            //if (MoveJoystick == null) throw new UnassignedReferenceException("Please assign MoveJoystick object");
            _clampToScreen = Screen.width / 9f;
        }

        void Start ()
        {
            _mesh = gameObject.GetComponentInChildren<MeshRenderer>();
        }

        void Move()
        {
            //----------- Touch or keyboard -------
            if (Input.touchCount != 0 && Input.GetTouch(0).deltaPosition.sqrMagnitude > 10)
                _input = Input.GetTouch(0).deltaPosition;
            else
                _input = new Vector2(Input.GetAxis("Horizontal"), 0) * 10;

            //----------- Move the ship -----------
            _moveDirection = transform.TransformDirection(new Vector2(_input.x, 0)) * SPEED * Time.deltaTime;


            if ((transform.position.x > _clampToScreen && _moveDirection.x > 0) || (transform.position.x < -_clampToScreen && _moveDirection.x < 0))
            {
                _moveDirection.x = 0;
            }
            else
            {
                Rotate();
            }

            transform.position += _moveDirection;
        }

        void Rotate()
        {
            if (_moveDirection.x > 0) _eulerAngle = new Vector2(90f, -40f);
            else if (_moveDirection.x < 0) _eulerAngle = new Vector2(90f, 40f);
            else _eulerAngle = new Vector2(90f, 0f);

            LeanTween.rotate(_mesh.gameObject, _eulerAngle, 0.1f);
        }
	
        void Update()
        {
            //if (MoveJoystick.JSK_TouchForce > 10f) Move(MoveJoystick.JSK_DirectionNormalized);
            Move();
        }

    }
}
