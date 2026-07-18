using System;
using System.Collections.Generic;
using Levels.Parameters;

namespace Services {
    public class LevelDataProvider {
        private readonly LevelsParameters _parameters;
        
        private const int StartRandomLevel = 2;
        private const int LevelsCount = 3;
        
        public LevelDataProvider(LevelsParameters parameters) {
            _parameters = parameters;
        }
        
        public LevelConfig LoadLevelData(int levelId) {
            int actualLevel;
            
            if (levelId < StartRandomLevel) {
                actualLevel = levelId;
            } else {
                actualLevel = Remap(levelId);
            }
            
            return _parameters.levels[actualLevel];
        }
        
        private int Remap(int levelId) {
            int randomIndex = levelId - StartRandomLevel;
            
            int cycle = randomIndex / LevelsCount;
            int indexInCycle = randomIndex % LevelsCount;
            
            List<int> sequence = GenerateCycle(cycle);
            
            return sequence[indexInCycle];
        }
        
        private List<int> GenerateCycle(int cycle) {
            List<int> sequence = new List<int>(LevelsCount);
            
            for (int i = 0; i < LevelsCount; i++) {
                sequence.Add(StartRandomLevel + i);
            }
            
            Random random = new Random(cycle);
            
            Shuffle(sequence, random);
            
            if (cycle > 0) {
                int previousLast = GetPreviousCycleLast(cycle);
                
                if (sequence[0] == previousLast) {
                    Swap(sequence, 0, 1);
                }
            }
            
            return sequence;
        }
        
        private int GetPreviousCycleLast(int cycle) {
            List<int> previous = new List<int>(LevelsCount);
            
            for (int i = 0; i < LevelsCount; i++) {
                previous.Add(StartRandomLevel + i);
            }
            
            Random random = new Random(cycle - 1);
            Shuffle(previous, random);
            
            return previous[^1];
        }
        
        private void Shuffle<T>(List<T> list, Random random) {
            for (int i = list.Count - 1; i > 0; i--) {
                int j = random.Next(i + 1);
                
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        
        private void Swap<T>(List<T> list, int a, int b) {
            (list[a], list[b]) = (list[b], list[a]);
        }
    }
}