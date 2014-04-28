using Assets;
using Assets.Behaviors;
using UnityEngine;

public class UiSwitchColor : MonoBehaviour {

    private Player _player;

	void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
	}

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) SwitchColor();
    }

    void OnMouseDown()
    {
        SwitchColor();
    }

    void SwitchColor()
    {
        if (_player.ColorId++ >= 3) _player.ColorId = 0;

        renderer.material.SetColor("_TintColor", Colors.GetColorById(_player.ColorId));

        _player.MeshComponent.material.SetColor("_Color", Colors.GetColorById(_player.ColorId));

        Debug.Log("Switched color to " + _player.ColorId);
    }

}
