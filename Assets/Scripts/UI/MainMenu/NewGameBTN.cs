using Events;

namespace UI.MainMenu
{
    public class NewGameBTN : BaseNewGameButton
    {
        protected override void OnButtonClickComplete()
        {
            MainMenuEvents.NewGameBTN?.Invoke("NewGameBTN");
        }
    }
}