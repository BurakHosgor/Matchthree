﻿using Events;
using UnityEngine;
using Utils;
using Zenject;

namespace Datas
{
    public class PlayerData
    {
        public float SoundVal => _soundVal;
        public bool VibrationVal => _vibrationVal;
        private const string SoundPrefKey = "Sound";
        private const string VibrationPrefKey = "Vibration";
        private float _soundVal;
        private bool _vibrationVal;

        [Inject] private SoundManager _soundManager;
        
        public PlayerData()
        {
            _soundVal = PlayerPrefs.GetFloat(SoundPrefKey);
            _vibrationVal = PlayerPrefs.GetInt(VibrationPrefKey).ToBool();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            MainMenuEvents.SoundValueChanged += OnSoundValueChanged;
            MainMenuEvents.VibrationValueChanged += OnVibrationValueChanged;
        }

        private void OnVibrationValueChanged(bool isActive)
        {
            _vibrationVal = isActive;
            PlayerPrefs.SetInt(VibrationPrefKey, isActive.ToInt());
        }

        private void OnSoundValueChanged(float soundVal)
        {
            _soundVal = soundVal;
            PlayerPrefs.SetFloat(SoundPrefKey, _soundVal);
            SoundManager.Instance.SetMusicVolume(_soundVal);
            // Debug.LogWarning(_soundVal);
        }
        
        public void SetSoundVal(float value)
        {
            _soundVal = value;
            PlayerPrefs.SetFloat(SoundPrefKey, _soundVal);
        }
    }
}