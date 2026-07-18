using Boot.Controllers;
using Bullets.Parameters;
using Cubes;
using Levels.Parameters;
using Loader;
using Music;
using Progression.Models;
using ProjectCamera;
using Services;
using Services.Audio.Configs;
using Turrels.Parameters;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace Boot {
    public class ProjectScope : LifetimeScope {
        [field: SerializeField]
        private CameraView _camera;
        
        [field: SerializeField]
        private LoaderCurtain _loader;
        
        [field: SerializeField]
        private EventSystem _eventSystem;
        
        protected override void Awake() {
            base.Awake();
            
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_camera);
            DontDestroyOnLoad(_loader);
            DontDestroyOnLoad(_eventSystem);
        }
        
        protected override void Configure(IContainerBuilder builder) {
            builder.RegisterInstance(BulletsParameters.LoadFromResources());
            builder.RegisterInstance(LevelsParameters.LoadFromResources());
            builder.RegisterInstance(CubeConfigsBase.LoadFromResources());
            builder.RegisterInstance(TurretsParameters.LoadFromResources());
            builder.RegisterInstance(AudioParameters.LoadFromResources());
            
            builder.RegisterInstance(_camera);
            builder.RegisterInstance(_loader);
            builder.RegisterInstance(_eventSystem);
            
            builder.Register<GameModel>(Lifetime.Singleton);
            
            builder.Register<SceneLoaderService>(Lifetime.Singleton);
            builder.Register<LevelDataProvider>(Lifetime.Singleton);
            builder.Register<LevelLoader>(Lifetime.Singleton);
            
            
            builder.RegisterEntryPoint<CurtainController>();
            
            builder.RegisterEntryPoint<GameEntryPoint>();
            
            builder.RegisterEntryPoint<MusicPlayer>();
        }
    }
}