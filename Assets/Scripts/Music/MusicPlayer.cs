using Services.Audio.Players;
using UnityEngine;
using VContainer.Unity;

namespace Music {
    public class MusicPlayer : IStartable {
        public void Start() {
            AudioService.instance.playerLoop.PlayLoop("Music", Vector3.zero, AudioService.instance.parameters.music);
        }
    }
}