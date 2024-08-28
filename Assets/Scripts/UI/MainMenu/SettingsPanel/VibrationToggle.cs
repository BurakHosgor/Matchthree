using Datas;
using Events;
using Utils;
using Zenject;


namespace UI.MainMenu.SettingsPanel
{
    public class VibrationToggle : UIToggle
    {
        [Inject] private PlayerData _playerData{get;set;}
        protected override void OnEnable()
        {
            base.OnEnable();
            _toggle.isOn = _playerData.VibrationVal;
        }

        protected override void OnValueChanged(bool isActive)
        {
            MainMenuEvents.VibrationValueChanged?.Invoke(isActive);
        }
    }
}