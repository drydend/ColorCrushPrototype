using Cubes;
using Levels.Parameters;
using Progression.Models;
using R3;
using Services;
using Turrels.Models;
using Turrels.Parameters;

namespace Levels.Models {
    public class LevelSessionModel {
        public readonly CubeGridModel cubeGrid;
        public readonly TurretsGridModel turretsGrid;
        public readonly ReactiveProperty<State> state;
        
        public LevelSessionModel(GameModel game, LevelDataProvider dataProvider, CubeConfigsBase cubes, TurretsParameters turretParameters) {
            LevelConfig levelConfig = dataProvider.LoadLevelData(game.currentLevelId);
            cubeGrid = new LevelGridBinder().Bind(cubes, levelConfig);
            turretsGrid = new TurretsGridModel(levelConfig.turretsGrid, turretParameters);
            state = new ReactiveProperty<State>(State.LevelEnter);
        }
        
        public enum State {
            LevelEnter,
            Action,
            Win,
            Lose,
        }
    }
}