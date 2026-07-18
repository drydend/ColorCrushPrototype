using System.Threading;
using Cysharp.Threading.Tasks;
using Services.Audio.Configs;
using UnityEngine;
using UnityEngine.Audio;
using Utilities;

namespace Services.Audio.Players {
    public class AudioService {
        private static AudioService _instance;
        
        public static AudioService instance {
            get {
                if (_instance == null) {
                    _instance = new AudioService();
                }
                
                return _instance;
            }
        }
        
        public AudioPlayerSingle player { get; private set; }
        public AudioPlayerLoop playerLoop { get; private set; }
        
        public bool isEnableMusic { get; private set; }
        public bool isEnableSound { get; private set; }
        
        public bool isEnableMusicSettings { get; private set; }
        public bool isEnableSoundSettings { get; private set; }
        
        public bool isPause { get; private set; }
        
        public AudioParameters parameters;
        
        private CancellationTokenSource _cancellation;
        
        private const string _SNAPSHOT_ACTIVE = "Active";
        private const string _SNAPSHOT_MUTE = "Mute";
        private const float _DEFAULT_TRANSITION_TIME = 0.5f;
        
        public virtual void Init() {
            Transform pool = new GameObject("Audio").transform;
            
            parameters = AudioParameters.LoadFromResources();
            
            player = new AudioPlayerSingle(pool, parameters);
            playerLoop = new AudioPlayerLoop(pool, parameters);
            
            Object.DontDestroyOnLoad(pool);
            
            isEnableMusic = true;
            isEnableSound = true;
            
            isEnableMusicSettings = LoadMusicState();
            isEnableSoundSettings = LoadSoundState();
            
            isPause = LoadPauseState();
            
            ChangeMusicForce(isEnableMusicSettings, _DEFAULT_TRANSITION_TIME);
            ChangeSoundForce(isEnableSoundSettings, _DEFAULT_TRANSITION_TIME);
            
            CancellationUtility.Release(ref _cancellation);
            _cancellation = new CancellationTokenSource();
            UpdateActiveProcess(_cancellation.Token).Forget();
        }
        
        protected virtual void Destroy() {
            CancellationUtility.Release(ref _cancellation);
            _cancellation = new CancellationTokenSource();
        }
        
        public void ChangeMusicSettings(bool isEnable, float timeToReach = _DEFAULT_TRANSITION_TIME) {
            if (isEnableMusicSettings == isEnable) {
                return;
            }
            
            isEnableMusicSettings = isEnable;
            
            if (isEnable) {
                if (isEnableMusic) {
                    ChangeMusicForce(true, timeToReach);
                }
            } else if (isEnableMusic) {
                ChangeMusicForce(false, timeToReach);
            }
        }
        
        public void ChangeMusicState(bool isEnable, float timeToReach = _DEFAULT_TRANSITION_TIME) {
            if (isEnable == isEnableMusic) {
                return;
            }
            
            if (isEnableMusicSettings) {
                ChangeMusicForce(isEnable, timeToReach);
            }
            
            isEnableMusic = isEnable;
        }
        
        public void ChangeSoundSettings(bool isEnable, float timeToReach = _DEFAULT_TRANSITION_TIME) {
            if (isEnableSoundSettings == isEnable) {
                return;
            }
            
            isEnableSoundSettings = isEnable;
            
            if (isEnable) {
                if (isEnableSound) {
                    ChangeSoundForce(true, timeToReach);
                }
            } else if (isEnableSound) {
                ChangeSoundForce(false, timeToReach);
            }
        }
        
        public void ChangeSoundState(bool isEnable, float timeToReach = _DEFAULT_TRANSITION_TIME) {
            if (isEnable == isEnableSound) {
                return;
            }
            
            if (isEnableSoundSettings) {
                ChangeSoundForce(isEnable, timeToReach);
            }
            
            isEnableSound = isEnable;
        }
        
        public void ChangePause(bool isEnable, float timeToReach = _DEFAULT_TRANSITION_TIME) {
            if (isEnable == isPause) {
                return;
            }
            
            isPause = isEnable;
            
            if (isEnable) {
                if (isEnableSound && isEnableSoundSettings) {
                    ChangeSoundScaledForce(false, timeToReach);
                }
            } else if (isEnableSound && isEnableSoundSettings) {
                ChangeSoundScaledForce(true, timeToReach);
            }
        }
        
        public void ClearAllLoops() => playerLoop.ClearAllLoops();
        
        protected bool LoadMusicState() => true;
        
        protected bool LoadSoundState() => true;
        
        protected bool LoadPauseState() => true;
        
        private async UniTask UpdateActiveProcess(CancellationToken cancellation) {
            while (Application.isPlaying) {
                await UniTask.Delay(1000, DelayType.UnscaledDeltaTime, PlayerLoopTiming.Update, cancellation);
                player.ClearActive();
            }
        }
        
        private void ChangeMusicForce(bool isEnable, float timeToReach) {
            if (isEnable) {
                TransitionToSnapshots(parameters.mixers.music, _SNAPSHOT_ACTIVE, timeToReach);
            } else {
                TransitionToSnapshots(parameters.mixers.music, _SNAPSHOT_MUTE, timeToReach);
            }
        }
        
        private void ChangeSoundForce(bool isEnable, float timeToReach) {
            if (isEnable) {
                TransitionToSnapshots(parameters.mixers.soundUnscaled, _SNAPSHOT_ACTIVE, timeToReach);
            } else {
                TransitionToSnapshots(parameters.mixers.soundUnscaled, _SNAPSHOT_MUTE, timeToReach);
            }
            
            if (isPause == false) {
                ChangeSoundScaledForce(isEnable, timeToReach);
            }
        }
        
        private void ChangeSoundScaledForce(bool isEnable, float timeToReach) {
            if (isEnable) {
                TransitionToSnapshots(parameters.mixers.soundScaled, _SNAPSHOT_ACTIVE, timeToReach);
            } else {
                TransitionToSnapshots(parameters.mixers.soundScaled, _SNAPSHOT_MUTE, timeToReach);
            }
        }
        
        private void TransitionToSnapshots(AudioMixer mixer, string snapshotName, float timeToReach) {
            mixer.TransitionToSnapshots(new[] { mixer.FindSnapshot(snapshotName) }, new float[] { 1f }, timeToReach);
        }
    }
}