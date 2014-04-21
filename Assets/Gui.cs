using System;
using System.Globalization;
using Assets.Behaviors;
using UnityEngine;

namespace Assets
{
    public class Gui : MonoBehaviour
    {
        private Player _player;
        private GameObject _colorIndicator;

        private int _boxWidth = 200;
        private int _boxHeight = 48;
        //private int _boxPadding = 10;

        private int _healthBarLength;
        private int _shieldBarLength;

        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            _colorIndicator = GameObject.Find("ColorIndicator");

            RefreshHealthBar();
        }

        void RefreshHealthBar()
        {
            _healthBarLength = (_boxWidth) * _player.Health / _player.MaxHealth;
            _shieldBarLength = (_boxWidth) * _player.Shield / _player.MaxShield;
        }

        public GUISkin UiSkin;
        private Texture2D _healthTexture2D;
        private Texture2D _shieldTexture2D ;

        void Awake()
        {
            UiSkin = Resources.Load<GUISkin>("UiSkin");
            _healthTexture2D = Resources.Load<Texture2D>("Materials/Textures/healthBar");
            _shieldTexture2D = Resources.Load<Texture2D>("Materials/Textures/shieldBar");
        }

        void OnGUI()
        {
            GUI.skin = UiSkin;

            /*GUILayout.BeginVertical("box");
            GUILayout.Box(new Rect(0, Screen.height + 5, 20, -_healthBarLength), "HP" + _player.Health + "/" + _player.MaxHealth);
            GUILayout.Box(new Rect(20, Screen.height + 5, 20, -_shieldBarLength), "SP" + _player.Shield + "/" + _player.MaxShield);
            GUILayout.EndVertical();*/

            GUI.skin.box.normal.background = _healthTexture2D;
            GUI.Box(new Rect(0, Screen.height - 105, 20, -_healthBarLength), "HP" + _player.Health + "/" + _player.MaxHealth);

            GUI.skin.box.normal.background = _shieldTexture2D;
            GUI.Box(new Rect(20, Screen.height - 105, 20, -_shieldBarLength), "SP" + _player.Shield + "/" + _player.MaxShield);

            GUI.skin.button.normal.background = _healthTexture2D;
            if (GUI.Button(new Rect(0, Screen.height-100, 100, 100), _player.ColorId.ToString()))
            {
                _player.SwitchColor();
            }

            GUI.Label(new Rect(5, 2, _boxWidth, _boxHeight), "Credits: <b>" + _player.Credits + "</b>");
        }

        void Update()
        {
            RefreshHealthBar();

            //if (_colorIndicator.transform.)
        }

    }
}
