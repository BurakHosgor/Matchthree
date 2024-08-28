using Events;

namespace UI.MainMenu
{
    public class NewGameBTN1 : BaseNewGameButton
    {
        protected override void OnButtonClickComplete()
        {
            MainMenuEvents.NewGameBTN?.Invoke("NewGameBTN1");
        }
    }
}