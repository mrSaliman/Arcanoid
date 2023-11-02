using Cysharp.Threading.Tasks;

namespace App.Scripts.Libs.Architecture.Scenes
{
    public class Scene
    {
        private InteractorsBase _interactorsBase;
        private RepositoriesBase _repositoriesBase;
        private SceneConfig _sceneConfig;

        public Scene(SceneConfig sceneConfig)
        {
            _sceneConfig = sceneConfig;
            _interactorsBase = new InteractorsBase(sceneConfig);
            _repositoriesBase = new RepositoriesBase(sceneConfig);
        }

        public async UniTask InitializeAsync()
        {
            _interactorsBase.CreateAllInteractors();
            _repositoriesBase.CreateAllRepositories();
            await UniTask.Yield();
            
            _interactorsBase.SendOnCreateToAllInteractors();
            _repositoriesBase.SendOnCreateToAllRepositories();
            await UniTask.Yield();

            _interactorsBase.InitializeAllInteractors();
            _repositoriesBase.InitializeAllRepositories();
            await UniTask.Yield();
            
            _interactorsBase.SendOnStartToAllInteractors();
            _repositoriesBase.SendOnStartToAllRepositories();
        }
        
        public T GetInteractor<T>() where T : Interactor
        {
            return _interactorsBase.GetInteractor<T>();
        }

        public T GetRepository<T>() where T : Repository
        {
            return _repositoriesBase.GetRepository<T>();
        }
    }
}