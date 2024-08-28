using UnityEngine.Events;
using System;

namespace Events
{
    public static class MainMenuEvents
    {
        public static UnityAction SettingsBTN;
        public static UnityAction SettingsExitBTN;
        public static UnityAction<String> NewGameBTN;
        public static UnityAction<float> SoundValueChanged;
        public static UnityAction<bool> VibrationValueChanged;
    }
}