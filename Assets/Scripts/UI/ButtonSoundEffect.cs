using Services.Audio.Players;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ButtonSoundEffect : MonoBehaviour {
        [field: SerializeField]
        private Button _button;
        
        private void OnEnable() => _button.onClick.AddListener(PlaySound);
        
        private void OnDisable() => _button.onClick.RemoveListener(PlaySound);
        
        private void PlaySound() => AudioService.instance.player.Play(AudioService.instance.parameters.buttonClick, Vector3.zero);
    }
}