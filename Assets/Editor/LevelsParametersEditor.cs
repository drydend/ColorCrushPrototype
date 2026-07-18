using System.Collections.Generic;
using Cubes;
using Levels.Parameters;
using UnityEditor;
using UnityEngine;

namespace ProjectEditor {
    [CustomEditor(typeof(LevelsParameters))]
    public class LevelsParametersEditor : UnityEditor.Editor {
        private const int VisibleGridSize = 15;
        private const float CellSize = 42f;
        private const int PresetsInRow = 7;
        
        private enum Tool {
            Brush,
            Rectangle,
            Fill
        }
        
        private Tool _activeTool;
        
        private int _selectedLevelIndex;
        private int _selectedLayerIndex;
        private int _selectedPresetIndex = -1;
        
        private int _gridOffsetX;
        private int _gridOffsetY;
        
        private LevelConfig _sizeTarget;
        private int _pendingWidth;
        private int _pendingHeight;
        
        private bool _isBrushPainting;
        private bool _isErasing;
        
        private readonly HashSet<Vector2Int> _paintedCells = new();
        
        private bool _isRectangleSelecting;
        private Vector2Int _rectangleStart;
        private Vector2Int _rectangleEnd;
        private CubeConfigsBase _cubesBase;
        
        public override void OnInspectorGUI() {
            var parameters = (LevelsParameters)target;
            _cubesBase = CubeConfigsBase.LoadFromResources();
            
            serializedObject.Update();
            
            var paletteProperty = serializedObject.FindProperty("<cubePalette>k__BackingField");
            
            if (paletteProperty != null) {
                EditorGUILayout.PropertyField(paletteProperty, new GUIContent("Cube Palette"));
            }
            
            serializedObject.ApplyModifiedProperties();
            
            DrawLevelControls(parameters);
            
            if (parameters.levels == null || parameters.levels.Length == 0) {
                EditorGUILayout.HelpBox("Добавьте LevelConfig для начала редактирования.", MessageType.Info);
                
                return;
            }
            
            _selectedLevelIndex = Mathf.Clamp(_selectedLevelIndex, 0, parameters.levels.Length - 1);
            
            var levelConfig = parameters.levels[_selectedLevelIndex];
            
            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField($"Level {_selectedLevelIndex + 1}", EditorStyles.boldLabel);
            
            DrawLevelSize(parameters, levelConfig);
            
            serializedObject.Update();
            
            var levelsProperty = serializedObject.FindProperty("<levels>k__BackingField");
            
            if (levelsProperty == null)
                return;
            
            var levelProperty = levelsProperty.GetArrayElementAtIndex(_selectedLevelIndex);
            
            DrawBrushPalette(parameters);
            DrawLayerControls(parameters, levelConfig, levelProperty);
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawBrushPalette(LevelsParameters parameters) {
            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Brush presets", EditorStyles.boldLabel);
            
            var palette = _cubesBase;
            
            if (palette == null) {
                EditorGUILayout.HelpBox("Назначьте Cube Palette.", MessageType.Warning);
                
                return;
            }
            
            var presets = palette.presets;
            
            if (presets == null || presets.Length == 0) {
                EditorGUILayout.HelpBox("В Cube Palette нет presets.", MessageType.Info);
                
                return;
            }
            
            _selectedPresetIndex = Mathf.Clamp(_selectedPresetIndex, 0, presets.Length - 1);
            
            for (var index = 0; index < presets.Length; index++) {
                if (index % PresetsInRow == 0)
                    EditorGUILayout.BeginHorizontal();
                
                var preset = presets[index];
                
                var rect = GUILayoutUtility.GetRect(CellSize, CellSize, GUILayout.Width(CellSize), GUILayout.Height(CellSize));
                
                GUI.Box(rect, GUIContent.none, index == _selectedPresetIndex ? EditorStyles.helpBox : GUIStyle.none);
                
                EditorGUI.DrawRect(rect, preset.previewColor);
                
                var labelRect = new Rect(rect.x, rect.y + rect.height - 17, rect.width, 17);
                
                GUI.Label(labelRect, $"ID: {preset.id}", EditorStyles.whiteMiniLabel);
                
                if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
                    _selectedPresetIndex = index;
                
                if (index % PresetsInRow == PresetsInRow - 1 || index == presets.Length - 1) {
                    EditorGUILayout.EndHorizontal();
                }
            }
            
            var selectedPreset = presets[_selectedPresetIndex];
            
            EditorGUILayout.LabelField($"Selected: ID {selectedPreset.id}, Health: {selectedPreset.health}");
        }
        
        private void DrawLevelControls(LevelsParameters parameters) {
            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Levels", EditorStyles.boldLabel);
            
            using (new EditorGUILayout.HorizontalScope()) {
                if (GUILayout.Button("+ Add Level")) {
                    Undo.RecordObject(parameters, "Add level");
                    
                    parameters.AddLevel();
                    
                    _selectedLevelIndex = parameters.levels.Length - 1;
                    _selectedLayerIndex = 0;
                    
                    ResetGridNavigation();
                    EditorUtility.SetDirty(parameters);
                }
                
                using (new EditorGUI.DisabledScope(parameters.levels == null || parameters.levels.Length == 0)) {
                    if (GUILayout.Button("- Delete selected Level")) {
                        Undo.RecordObject(parameters, "Delete level");
                        
                        parameters.RemoveLevel(_selectedLevelIndex);
                        
                        _selectedLevelIndex = Mathf.Max(0, _selectedLevelIndex - 1);
                        
                        _selectedLayerIndex = 0;
                        
                        ResetGridNavigation();
                        EditorUtility.SetDirty(parameters);
                    }
                }
            }
            
            if (parameters.levels == null || parameters.levels.Length == 0)
                return;
            
            var levelNames = new string[parameters.levels.Length];
            
            for (var index = 0; index < levelNames.Length; index++)
                levelNames[index] = $"Level {index + 1}";
            
            EditorGUI.BeginChangeCheck();
            
            _selectedLevelIndex = EditorGUILayout.Popup("Edit Level", _selectedLevelIndex, levelNames);
            
            if (EditorGUI.EndChangeCheck()) {
                _selectedLayerIndex = 0;
                ResetGridNavigation();
            }
        }
        
        private void DrawLevelSize(LevelsParameters parameters, LevelConfig levelConfig) {
            if (_sizeTarget != levelConfig) {
                _sizeTarget = levelConfig;
                _pendingWidth = levelConfig.width;
                _pendingHeight = levelConfig.height;
            }
            
            _pendingWidth = Mathf.Max(1, EditorGUILayout.DelayedIntField("Width", _pendingWidth));
            
            _pendingHeight = Mathf.Max(1, EditorGUILayout.DelayedIntField("Height", _pendingHeight));
            
            var hasChanges = _pendingWidth != levelConfig.width || _pendingHeight != levelConfig.height;
            
            using (new EditorGUI.DisabledScope(!hasChanges)) {
                if (GUILayout.Button("Apply grid size")) {
                    Undo.RecordObject(parameters, "Resize level grid");
                    
                    levelConfig.ResizeGrid(_pendingWidth, _pendingHeight);
                    
                    ResetGridNavigation();
                    EditorUtility.SetDirty(parameters);
                }
            }
        }
        
        private void DrawLayerControls(LevelsParameters parameters, LevelConfig levelConfig, SerializedProperty levelProperty) {
            var layersProperty = FindField(levelProperty, "layers");
            
            if (layersProperty == null)
                return;
            
            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);
            
            if (layersProperty.arraySize == 0) {
                if (GUILayout.Button("+ Add Layer")) {
                    Undo.RecordObject(parameters, "Add layer");
                    
                    levelConfig.AddLayer();
                    
                    _selectedLayerIndex = 0;
                    ResetGridNavigation();
                    
                    EditorUtility.SetDirty(parameters);
                }
                
                return;
            }
            
            _selectedLayerIndex = Mathf.Clamp(_selectedLayerIndex, 0, layersProperty.arraySize - 1);
            
            var layerNames = new string[layersProperty.arraySize];
            
            for (var index = 0; index < layerNames.Length; index++)
                layerNames[index] = $"Layer {index + 1}";
            
            EditorGUI.BeginChangeCheck();
            
            _selectedLayerIndex = EditorGUILayout.Popup("Edit Layer", _selectedLayerIndex, layerNames);
            
            if (EditorGUI.EndChangeCheck())
                ResetGridNavigation();
            
            using (new EditorGUILayout.HorizontalScope()) {
                if (GUILayout.Button("+ Add Layer")) {
                    Undo.RecordObject(parameters, "Add layer");
                    
                    levelConfig.AddLayer();
                    
                    _selectedLayerIndex = levelConfig.layers.Length - 1;
                    
                    ResetGridNavigation();
                    EditorUtility.SetDirty(parameters);
                    
                    return;
                }
                
                if (GUILayout.Button("- Delete selected Layer")) {
                    Undo.RecordObject(parameters, "Delete layer");
                    
                    levelConfig.RemoveLayer(_selectedLayerIndex);
                    
                    _selectedLayerIndex = Mathf.Max(0, _selectedLayerIndex - 1);
                    
                    ResetGridNavigation();
                    EditorUtility.SetDirty(parameters);
                    
                    return;
                }
            }
            
            DrawGridNavigation(levelConfig);
            
            var selectedLayerProperty = layersProperty.GetArrayElementAtIndex(_selectedLayerIndex);
            
            DrawLayer(parameters, selectedLayerProperty);
            
            DrawTurretsConfig(levelProperty, levelConfig, _cubesBase);
        }
        
        private void DrawGridNavigation(LevelConfig levelConfig) {
            var maxOffsetX = ((levelConfig.width - 1) / VisibleGridSize) * VisibleGridSize;
            
            var maxOffsetY = ((levelConfig.height - 1) / VisibleGridSize) * VisibleGridSize;
            
            _gridOffsetX = Mathf.Clamp(_gridOffsetX, 0, maxOffsetX);
            _gridOffsetY = Mathf.Clamp(_gridOffsetY, 0, maxOffsetY);
            
            using (new EditorGUILayout.HorizontalScope()) {
                DrawToolSelectionInline();
                
                GUILayout.Space(8);
                
                using (new EditorGUI.DisabledScope(_gridOffsetX == 0)) {
                    if (GUILayout.Button("← 15", GUILayout.Width(55)))
                        _gridOffsetX -= VisibleGridSize;
                }
                
                using (new EditorGUI.DisabledScope(_gridOffsetX >= maxOffsetX)) {
                    if (GUILayout.Button("15 →", GUILayout.Width(55)))
                        _gridOffsetX += VisibleGridSize;
                }
                
                GUILayout.Label($"X: {_gridOffsetX + 1}-{Mathf.Min(_gridOffsetX + VisibleGridSize, levelConfig.width)}");
            }
            
            using (new EditorGUILayout.HorizontalScope()) {
                GUILayout.Space(228);
                
                using (new EditorGUI.DisabledScope(_gridOffsetY == 0)) {
                    if (GUILayout.Button("↑ 15", GUILayout.Width(55)))
                        _gridOffsetY -= VisibleGridSize;
                }
                
                using (new EditorGUI.DisabledScope(_gridOffsetY >= maxOffsetY)) {
                    if (GUILayout.Button("15 ↓", GUILayout.Width(55)))
                        _gridOffsetY += VisibleGridSize;
                }
                
                GUILayout.Label($"Y: {_gridOffsetY + 1}-{Mathf.Min(_gridOffsetY + VisibleGridSize, levelConfig.height)}");
            }
        }
        
        private void DrawToolSelectionInline() {
            var newTool = (Tool)GUILayout.Toolbar((int)_activeTool, new[] { "Brush", "Rect", "Fill" }, GUILayout.Width(220));
            
            if (newTool == _activeTool)
                return;
            
            _activeTool = newTool;
            CancelRectangle();
            StopBrushPainting();
        }
        
        private void DrawLayer(LevelsParameters parameters, SerializedProperty layerProperty) {
            var rowsProperty = FindField(layerProperty, "rows");
            
            if (rowsProperty == null)
                return;
            
            var firstY = _gridOffsetY;
            var lastY = Mathf.Min(_gridOffsetY + VisibleGridSize, rowsProperty.arraySize);
            
            // Отрисовка сверху вниз: height - 1 находится сверху.
            for (var y = lastY - 1; y >= firstY; y--) {
                var rowProperty = rowsProperty.GetArrayElementAtIndex(y);
                var cubesProperty = FindField(rowProperty, "cubes");
                
                if (cubesProperty == null)
                    continue;
                
                var firstX = _gridOffsetX;
                var lastX = Mathf.Min(_gridOffsetX + VisibleGridSize, cubesProperty.arraySize);
                
                using (new EditorGUILayout.HorizontalScope()) {
                    for (var x = firstX; x < lastX; x++) {
                        var cubeProperty = cubesProperty.GetArrayElementAtIndex(x);
                        
                        DrawCube(parameters, layerProperty, cubeProperty, x, y);
                    }
                }
            }
            
            if (Event.current.type == EventType.MouseUp) {
                if (_activeTool == Tool.Rectangle && _isRectangleSelecting) {
                    ApplyRectangle(parameters, layerProperty);
                    CancelRectangle();
                }
                
                StopBrushPainting();
            }
        }
        
        private void DrawCube(LevelsParameters parameters, SerializedProperty layerProperty, SerializedProperty cubeProperty, int x, int y) {
            var rect = GUILayoutUtility.GetRect(CellSize, CellSize, GUILayout.Width(CellSize), GUILayout.Height(CellSize));
            
            GUI.Box(rect, GUIContent.none, EditorStyles.helpBox);
            
            var idProperty = FindField(cubeProperty, "id");
            
            if (idProperty != null && _cubesBase != null && _cubesBase.TryGetPreset(idProperty.intValue, out var preset)) {
                EditorGUI.DrawRect(rect, preset.previewColor);
            }
            
            DrawRectanglePreview(rect, x, y);
            
            HandleCellInput(parameters, layerProperty, cubeProperty, x, y, rect);
        }
        
        private void HandleCellInput(LevelsParameters parameters, SerializedProperty layerProperty, SerializedProperty cubeProperty, int x, int y, Rect rect) {
            var currentEvent = Event.current;
            
            if (!rect.Contains(currentEvent.mousePosition))
                return;
            
            if (currentEvent.type == EventType.MouseDown) {
                HandleMouseDown(parameters, layerProperty, cubeProperty, x, y, currentEvent);
                
                return;
            }
            
            if (currentEvent.type != EventType.MouseDrag)
                return;
            
            if (_activeTool == Tool.Brush && _isBrushPainting) {
                PaintBrushCell(parameters, cubeProperty, x, y);
                currentEvent.Use();
            } else if (_activeTool == Tool.Rectangle && _isRectangleSelecting) {
                _rectangleEnd = new Vector2Int(x, y);
                Repaint();
                
                currentEvent.Use();
            }
        }
        
        private void HandleMouseDown(LevelsParameters parameters, SerializedProperty layerProperty, SerializedProperty cubeProperty, int x, int y, Event currentEvent) {
            if (_activeTool == Tool.Brush) {
                if (currentEvent.button != 0 && currentEvent.button != 1)
                    return;
                
                Undo.RecordObject(parameters, "Paint cubes");
                
                _isBrushPainting = true;
                _isErasing = currentEvent.button == 1;
                
                _paintedCells.Clear();
                
                PaintBrushCell(parameters, cubeProperty, x, y);
                
                currentEvent.Use();
                return;
            }
            
            if (_activeTool == Tool.Rectangle) {
                if (currentEvent.button == 1) {
                    CancelRectangle();
                    currentEvent.Use();
                    
                    return;
                }
                
                if (currentEvent.button != 0)
                    return;
                
                Undo.RecordObject(parameters, "Paint rectangle");
                
                _isRectangleSelecting = true;
                _rectangleStart = new Vector2Int(x, y);
                _rectangleEnd = _rectangleStart;
                
                currentEvent.Use();
                
                return;
            }
            
            if (_activeTool == Tool.Fill) {
                if (currentEvent.button != 0)
                    return;
                
                Undo.RecordObject(parameters, "Fill cubes");
                
                FloodFill(parameters, layerProperty, x, y);
                
                currentEvent.Use();
            }
        }
        
        private void PaintBrushCell(LevelsParameters parameters, SerializedProperty cubeProperty, int x, int y) {
            var position = new Vector2Int(x, y);
            
            if (!_paintedCells.Add(position))
                return;
            
            if (_isErasing)
                SetCubeId(cubeProperty, 0);
            else
                ApplySelectedPreset(parameters, cubeProperty);
        }
        
        private void ApplyRectangle(LevelsParameters parameters, SerializedProperty layerProperty) {
            var minX = Mathf.Min(_rectangleStart.x, _rectangleEnd.x);
            var maxX = Mathf.Max(_rectangleStart.x, _rectangleEnd.x);
            
            var minY = Mathf.Min(_rectangleStart.y, _rectangleEnd.y);
            var maxY = Mathf.Max(_rectangleStart.y, _rectangleEnd.y);
            
            for (var y = minY; y <= maxY; y++) {
                for (var x = minX; x <= maxX; x++) {
                    var cubeProperty = GetCubeProperty(layerProperty, x, y);
                    
                    if (cubeProperty != null)
                        ApplySelectedPreset(parameters, cubeProperty);
                }
            }
        }
        
        private void FloodFill(LevelsParameters parameters, SerializedProperty layerProperty, int startX, int startY) {
            if (!TryGetSelectedPreset(parameters, out var selectedPreset))
                return;
            
            var startCube = GetCubeProperty(layerProperty, startX, startY);
            
            if (startCube == null)
                return;
            
            var sourceId = GetCubeId(startCube);
            
            if (sourceId == selectedPreset.id)
                return;
            
            var queue = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();
            
            var startPosition = new Vector2Int(startX, startY);
            
            queue.Enqueue(startPosition);
            visited.Add(startPosition);
            
            while (queue.Count > 0) {
                var position = queue.Dequeue();
                
                var cubeProperty = GetCubeProperty(layerProperty, position.x, position.y);
                
                if (cubeProperty == null || GetCubeId(cubeProperty) != sourceId) {
                    continue;
                }
                
                SetCubeId(cubeProperty, selectedPreset.id);
                
                AddFillNeighbour(queue, visited, layerProperty, position.x - 1, position.y, sourceId);
                
                AddFillNeighbour(queue, visited, layerProperty, position.x + 1, position.y, sourceId);
                
                AddFillNeighbour(queue, visited, layerProperty, position.x, position.y - 1, sourceId);
                
                AddFillNeighbour(queue, visited, layerProperty, position.x, position.y + 1, sourceId);
            }
        }
        
        private static void AddFillNeighbour(Queue<Vector2Int> queue, HashSet<Vector2Int> visited, SerializedProperty layerProperty, int x, int y, int sourceId) {
            var position = new Vector2Int(x, y);
            
            if (!visited.Add(position))
                return;
            
            var cubeProperty = GetCubeProperty(layerProperty, x, y);
            
            if (cubeProperty != null && GetCubeId(cubeProperty) == sourceId) {
                queue.Enqueue(position);
            }
        }
        
        private void ApplySelectedPreset(LevelsParameters parameters, SerializedProperty cubeProperty) {
            if (!TryGetSelectedPreset(parameters, out var preset))
                return;
            
            SetCubeId(cubeProperty, preset.id);
        }
        
        private bool TryGetSelectedPreset(LevelsParameters parameters, out CubeConfigsBase.CubePreset preset) {
            preset = null;
            
            var palette = _cubesBase;
            
            if (palette == null || palette.presets == null || palette.presets.Length == 0 || _selectedPresetIndex < 0 || _selectedPresetIndex >= palette.presets.Length) {
                return false;
            }
            
            preset = palette.presets[_selectedPresetIndex];
            
            return preset != null;
        }
        
        private void DrawTurretsConfig(SerializedProperty levelProperty, LevelConfig levelConfig, CubeConfigsBase cubeConfigsBase) {
            var turretsProperty = FindField(levelProperty, "turretsGrid");
            
            if (turretsProperty == null)
                return;
            
            EditorGUILayout.Space(12);
            EditorGUILayout.LabelField("Current level turrets", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(turretsProperty, includeChildren: true);
            
            EditorGUILayout.Space(6);
        }
        
        
        private void DrawRectanglePreview(Rect rect, int x, int y) {
            if (!_isRectangleSelecting)
                return;
            
            var minX = Mathf.Min(_rectangleStart.x, _rectangleEnd.x);
            var maxX = Mathf.Max(_rectangleStart.x, _rectangleEnd.x);
            
            var minY = Mathf.Min(_rectangleStart.y, _rectangleEnd.y);
            var maxY = Mathf.Max(_rectangleStart.y, _rectangleEnd.y);
            
            if (x < minX || x > maxX || y < minY || y > maxY)
                return;
            
            EditorGUI.DrawRect(rect, new Color(0.2f, 0.65f, 1f, 0.35f));
        }
        
        private static int GetCubeId(SerializedProperty cubeProperty) {
            var idProperty = FindField(cubeProperty, "id");
            
            return idProperty != null ? idProperty.intValue : 0;
        }
        
        private static void SetCubeId(SerializedProperty cubeProperty, int id) {
            var idProperty = FindField(cubeProperty, "id");
            
            if (idProperty != null)
                idProperty.intValue = id;
        }
        
        private static SerializedProperty GetCubeProperty(SerializedProperty layerProperty, int x, int y) {
            var rowsProperty = FindField(layerProperty, "rows");
            
            if (rowsProperty == null || y < 0 || y >= rowsProperty.arraySize) {
                return null;
            }
            
            var rowProperty = rowsProperty.GetArrayElementAtIndex(y);
            var cubesProperty = FindField(rowProperty, "cubes");
            
            if (cubesProperty == null || x < 0 || x >= cubesProperty.arraySize) {
                return null;
            }
            
            return cubesProperty.GetArrayElementAtIndex(x);
        }
        
        private static SerializedProperty FindField(SerializedProperty parent, string fieldName) {
            return parent.FindPropertyRelative(fieldName) ?? parent.FindPropertyRelative($"_{fieldName}") ?? parent.FindPropertyRelative($"<{fieldName}>k__BackingField");
        }
        
        private void CancelRectangle() {
            _isRectangleSelecting = false;
        }
        
        private void StopBrushPainting() {
            _isBrushPainting = false;
            _isErasing = false;
            
            _paintedCells.Clear();
        }
        
        private void ResetGridNavigation() {
            _gridOffsetX = 0;
            _gridOffsetY = 0;
            
            CancelRectangle();
            StopBrushPainting();
        }
    }
}