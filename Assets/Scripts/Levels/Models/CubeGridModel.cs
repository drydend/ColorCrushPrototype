using Cubes.Models;
using R3;

namespace Levels.Models {
    public class CubeGridModel {
        public ReactiveProperty<int> aliveCubesLeft;
        public int width;
        public int height;
        public int depth;
        
        public CubeModel[,,] grid;
        
        public CubeModel[] all;
    }
}