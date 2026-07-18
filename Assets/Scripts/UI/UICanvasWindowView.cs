using System;
using ProjectCamera;
using UnityEngine;

namespace UI {
    public class UICanvasWindowView : MonoBehaviour {
        [field: SerializeField]
        private Canvas _thisCanvas;
        
        public void SetCamera(CameraView camera) {
            _thisCanvas.worldCamera = camera.uiCamera;
        }
        
        public virtual void Open() {
            Debug.Log("Open");
            gameObject.SetActive(true);
        }
        
        public virtual void Close() {
            gameObject.SetActive(false);
        }
        
    #if UNITY_EDITOR
        [ContextMenu("Soft reset")]
        public virtual void Reset() {
            _thisCanvas = GetComponent<Canvas>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    #endif
    }
}