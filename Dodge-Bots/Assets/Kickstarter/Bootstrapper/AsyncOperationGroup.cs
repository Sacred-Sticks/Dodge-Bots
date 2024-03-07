using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kickstarter.Bootstrapper
{
    public readonly struct AsyncOperationGroup
    {
        private List<AsyncOperation> Operations { get; }

        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(o => o.progress);
        public bool IsDone => Operations.All(o => o.isDone);

        public void AddOperation(AsyncOperation operation, string scene = "")
        {
            Operations.Add(operation);
            if (scene != "")
                operation.completed += (_) => OnSceneLoaded(scene);
        }

        private static void OnSceneLoaded(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            var gameobjects = scene.GetRootGameObjects();
            foreach (var gameobject in gameobjects)
            {
                var awakes = gameobject.GetComponentsInChildren<IAwake>();
                foreach (var awake in awakes)
                    awake.Awake_();
            }
            foreach (var gameobject in gameobjects)
            {
                var starts = gameobject.GetComponentsInChildren<IStart>();
                foreach (var start in starts)
                    start.Start_();
            }
        }

        public AsyncOperationGroup(int initialCapacity)
        {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }

    public interface IAwake
    {
        public void Awake_();
    }

    public interface IStart
    {
        public void Start_();
    }
}