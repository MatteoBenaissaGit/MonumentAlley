using Game;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class NeighborMatcher : EditorWindow
    {
        [MenuItem("Tools/Block Controller Detector")]
        public static void ShowWindow()
        {
            GetWindow<NeighborMatcher>("Block Controller Detector");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Detect BlockControllers"))
            {
                DetectBlockControllers();
            }
        }

        private void DetectBlockControllers()
        {
            BlockController[] blockControllers = FindObjectsOfType<BlockController>();
            foreach (BlockController block in blockControllers)
            {
                block.ClearNeighborsList();
                
                Vector3[] directions = {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
                foreach (Vector3 direction in directions)
                {
                    if (Physics.Raycast(block.transform.position, direction, out RaycastHit hit, 1))
                    {
                        if (hit.transform.gameObject.TryGetComponent(out BlockController neighborBlock)
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