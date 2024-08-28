using Events;
using Zenject;
using Utils;
using Datas;

namespace UI.MainMenu.SettingsPanel
{
    public class SoundSlider : UISlider
    {
        [Inject] private PlayerData _playerData{get;set;}
        [Inject] private SoundManager _soundManager { get; set; }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            // Reset slider value after close-reopen game.
            _slider.value = 1;
            _slider.value = _playerData.SoundVal;
        }

        protected override void OnValueChanged(float val)
        {
            _playerData.SetSoundVal(val); 
            _soundManager.SetMusicVolume(val);
            MainMenuEvents.SoundValueChanged?.Invoke(val);
        }
    }
}