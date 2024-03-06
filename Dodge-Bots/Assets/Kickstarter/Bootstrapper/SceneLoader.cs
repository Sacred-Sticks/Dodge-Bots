using UnityEngine;
using System.Threading.Tasks;
using System;

namespace Kickstarter.Bootstrapper
{
    public class SceneLoader : MonoBehaviour
    {
        [DependencyInjection.Inject] private SceneLoader _sceneLoader => this;

        [SerializeField] private SceneGroup[] _sceneGroups;

        private LoadingProgress _progress;
        private static bool _isLoading;
        
        // Components
        private Camera _loadingCamera;
        private LoadingBar _loadingBar;

        public SceneGroupManager manager { get; } = new SceneGroupManager();

        private void Awake()
        {
            _loadingCamera = Camera.main;
            _loadingBar = GetComponentInChildren<LoadingBar>();
        }

        private async void Start()
        {
            await LoadSceneGroup(0);
        }

        private async Task LoadSceneGroup(int index)
        {
            if (index < 0 || index >= _sceneGroups.Length)
            {
                Debug.LogError($"Invalid scene group index: {index}");
                return;
            }

            _progress = new LoadingProgress(_loadingBar);

            EnableLoadingCanvas();
            await manager.LoadScenes(_sceneGroups[index], _progress);
            EnableLoadingCanvas(false);
        }

        private void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            _loadingCamera.gameObject.SetActive(_isLoading);
            _loadingBar.Enable(_isLoading);
        }

        public async void LoadSceneGroup(string groupName)
        {
            var index = Array.FindIndex(_sceneGroups, group => group.GroupName == groupName);
            await LoadSceneGroup(index);
        }

        public static bool IsLoadingComplete()
        {
            return !_isLoading;
        }
    }
}
