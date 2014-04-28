using System;
using System.Globalization;
using Assets.Behaviors;
using UnityEngine;

namespace Assets
{
    public class Gui : MonoBehaviour
    {
        private Player _player;
        
        private TextMesh _uiHealth;
        private TextMesh _uiShield;
        private TextMesh _uiCredits;

        public GUISkin UiSkin;

        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();            
            _uiHealth = GameObject.Find("UiHealth").GetComponent<TextMesh>();
            _uiShield = GameObject.Find("UiShield").GetComponent<TextMesh>();
            _uiCredits = GameObject.Find("UiCredits").GetComponent<TextMesh>();

            UiSkin = Resources.Load<GUISkin>("UiSkin");

            InvokeRepeating("RefreshPlayerStats", 0, 0.5f);
        }

        // Singleton  
        private static Gui _instance;   
        public static Gui Instance
        {
            get { return _instance ?? (_instance = FindObjectOfType(typeof (Gui)) as Gui); }
        }

        void RefreshPlayerStats()
        {
            _uiHealth.text = _player.Health.ToString();
            _uiShield.text = _player.Shield.ToString();
            _uiCredits.text = String.Format("{0:0,0}", _player.Credits);
        }

        /*void OnGUI()
        {
            //GUI.skin = UiSkin;

            // Show health bar
            //GUI.Box(new Rect(0, Screen.height - 105, 40, -_healthBarLength), "HP" + _player.Health + "/" + _player.MaxHealth, _healthTexture2D);

            // Show shield bar
            //GUI.Box(new Rect(40, Screen.height - 105, 40, -_shieldBarLength), "SP" + _player.Shield + "/" + _player.MaxShield, _shieldTexture2D);
        */

    }
}
