using System;
using Turrels.Parameters;
using UnityEngine;

namespace Levels.Parameters {
    [Serializable]
    public class LevelConfig {
        [field: SerializeField, Min(1)]
        public int width { get; private set; } = 8;
        
        [field: SerializeField, Min(1)]
        public int height { get; private set; } = 8;
        
        [field: SerializeField]
        public LayerConfig[] layers { get; private set; } = Array.Empty<LayerConfig>();
        
        [field: SerializeField]
        public TurretsGridConfig turretsGrid { get; private set; } = new TurretsGridConfig();
        
        public void ResizeGrid(int newWidth, int newHeight) {
            width = Mathf.Max(1, newWidth);
            height = Mathf.Max(1, newHeight);
            
            for (int layerId = 0; layerId < layers.Length; layerId++) {
                layers[layerId]?.Resize(width, height);
            }
        }
        
        public void AddLayer() {
            LayerConfig[] result = new LayerConfig[layers.Length + 1];
            Array.Copy(layers, result, layers.Length);
            
            LayerConfig layer = new LayerConfig();
            layer.Resize(width, height);
            
            result[^1] = layer;
            layers = result;
        }
        
        public void RemoveLayer(int index) {
            if (index < 0 || index >= layers.Length) {
                return;
            }
            
            LayerConfig[] result = new LayerConfig[layers.Length - 1];
            
            Array.Copy(layers, 0, result, 0, index);
            Array.Copy(layers, index + 1, result, index, layers.Length - index - 1);
            
            layers = result;
        }
    }
}