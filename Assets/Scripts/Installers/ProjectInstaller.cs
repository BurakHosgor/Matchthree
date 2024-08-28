using Events;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Datas;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        private ProjectEvents _projectEvents;
        private InputEvents _inputEvents;
        private GridEvents _gridEvents;
        private ProjectSettings _projectSettings;
        private PlayerData _playerData;
        
        
        [SerializeField] SoundManager _soundManagerPrefab;
        public override void InstallBindings()
        {
            InstallEvents();
            InstallSettings();
            InstallData();
            InstallSoundManager(); // Add this line
        }

        private void InstallEvents()
        {
            _projectEvents = new ProjectEvents();
            Container.BindInstance(_projectEvents).AsSingle();
            
            _inputEvents = new InputEvents();
            Container.BindInstance(_inputEvents).AsSingle();

            _gridEvents = new GridEvents();
            Container.BindInstance(_gridEvents).AsSingle();
        }

        private void InstallSettings()
        {
            _projectSettings = Resources.Load<ProjectSettings>(EnvVar.ProjectSettingsPath);
            Container.BindInstance(_projectSettings).AsSingle();
        }
        private void InstallData()
        {
            _playerData = new PlayerData();
            Container.BindInstance(_playerData).AsSingle();
        }
        private void InstallSoundManager()
        {
            Container.Bind<SoundManager>().FromComponentInNewPrefab(_soundManagerPrefab).AsSingle().NonLazy();
        }
        
        private void Awake()
        {
            RegisterEvents();
        }

        public override void Start()
        {
            _projectEvents.ProjectStarted?.Invoke();
        }

        private static void LoadScene(string sceneName) {SceneManager.LoadScene(sceneName);}

        private void RegisterEvents()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            MainMenuEvents.NewGameBTN += OnNewGameBTN;
            MainUIEvents.ExitBTN += OnExitBTN;
        }

        private void OnSceneLoaded(Scene loadedScene, LoadSceneMode arg1)
        {
            if(loadedScene.name == EnvVar.LoginSceneName)
            {
                LoadScene(EnvVar.MainSceneName);
            }
        }
        
        private void OnExitBTN()
        {
            LoadScene("MainMenu");
        }
        
        private void OnNewGameBTN(string buttonName)
        {
            switch (buttonName)
            {
                case "NewGameBTN":
                    LoadScene("Main");
                    break;
                case "NewGameBTN1":
                    LoadScene("Main 1");
                    break;
            }
        }
    }
}