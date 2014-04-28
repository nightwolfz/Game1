using UnityEngine;
using System.Collections;

public class RotateAroundItself : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(1, 1, 1) * Time.deltaTime * 10);
	}
}
