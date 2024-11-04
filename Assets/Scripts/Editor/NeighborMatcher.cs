using System.Collections.Generic;
using Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class NeighborMatcher : EditorWindow
    {
        [MenuItem("Tools/Block Neighbor Matcher")]
        public static void ShowWindow()
        {
            GetWindow<NeighborMatcher>("Block Neighbor Matcher");
        }

        private BlockController _createPathBlockLeft;
        private BlockController _createPathBlockRight;
        private bool _createPathActive = true;

        private void OnGUI()
        {
            if (GUILayout.Button($"Detect BlockControllers from {Selection.gameObjects.Length} selected objects") && Selection.gameObjects.Length > 0)
            {
                DetectBlockControllers();
            }
            
            GUILayout.Space(15);

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            _createPathBlockLeft = EditorGUILayout.ObjectField(_createPathBlockLeft, typeof(BlockController), true) as BlockController;
            _createPathBlockRight = EditorGUILayout.ObjectField(_createPathBlockRight, typeof(BlockController), true) as BlockController;
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Create Path between blocks") && _createPathBlockLeft != null && _createPathBlockRight != null)
            {
                CreatePathBetween(_createPathBlockLeft, _createPathBlockRight);
            }
            _createPathActive = EditorGUILayout.Toggle("Path is active", _createPathActive);
            GUILayout.EndVertical();
            
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button($"Clear paths from {Selection.gameObjects.Length} object"))
            {
                List<BlockController> blockControllers = GetBlockControllersFromSelection();
                foreach (BlockController block in blockControllers)
                {
                    block.ClearNeighborsList(true);
                    EditorUtility.SetDirty(block);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void CreatePathBetween(BlockController leftBlock, BlockController rightBlock)
        {
            if (leftBlock.Paths.Exists(x => x.Block == rightBlock) == false)
            {
                leftBlock.AddNeighbors(rightBlock, _createPathActive);
                EditorUtility.SetDirty(rightBlock);
            }
            if (rightBlock.Paths.Exists(x => x.Block == leftBlock) == false)
            {
                rightBlock.AddNeighbors(leftBlock, _createPathActive);
                EditorUtility.SetDirty(leftBlock);
            }
        }

        public static List<BlockController> GetBlockControllersFromSelection()
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;

            List<BlockController> blockControllers = new List<BlockController>();
            foreach (GameObject selectedGameObject in selectedGameObjects)
            {
                if (selectedGameObject.TryGetComponent(out BlockController blockController))
                {
                    blockControllers.Add(blockController);
                }
            }

            return blockControllers;
        }

        private void DetectBlockControllers()
        {
            List<BlockController> blockControllers = GetBlockControllersFromSelection();
            foreach (BlockController block in blockControllers)
            {
                block.ClearNeighborsList();
                
                Vector3[] directions = {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
                foreach (Vector3 direction in directions)
                {
                    if (Physics.Raycast(block.transform.position, direction, out RaycastHit hit, 1))
                    {
                        if (hit.transform.gameObject != block.gameObject 
                            && hit.transform.gameObject.TryGetComponent(out BlockController neighborBlock)
                            && block.Paths.Exists(x => x.Block == neighborBlock) == false)
                        {
                            block.AddNeighbors(neighborBlock);
                        }
                    }
                }
                
                
                block.Paths.ForEach(x => x.Block.AddNeighbors(block));
                EditorUtility.SetDirty(block);
            }
        }
    }
}