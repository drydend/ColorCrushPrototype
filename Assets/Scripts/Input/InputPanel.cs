using System;
using ProjectCamera;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input {
    public class InputPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public Subject<Vector2> onDown { get; private set; }
        public Subject<Vector2> onUp { get; private set; }
        
        [field: SerializeField]
        private Canvas _thisCanvas;
        
        public void Initialize(CameraView camera) {
            _thisCanvas.worldCamera = camera.uiCamera;
            onDown = new Subject<Vector2>();
            onUp = new Subject<Vector2>();
        }
        
        public void OnPointerDown(PointerEventData eventData) {
            onDown.OnNext(eventData.position);
        }
        
        public void OnPointerUp(PointerEventData eventData) {
            onUp.OnNext(eventData.position);
        }
    }
}