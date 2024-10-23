using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class BlockPath
    {
        public bool IsActive = true;
        public BlockController Block;
    }

    public class BlockController : MonoBehaviour
    {
        public Vector3 WalkPoint => transform.position + _walkPoint;
        public List<BlockPath> Paths => _paths;
    
        [SerializeField] private Vector3 _walkPoint;
        [SerializeField] private List<BlockPath> _paths;
        [SerializeField] private bool _isStair;

        public void AddNeighbors(BlockController block)
        {
            if (_paths.Exists(x => x.Block == block)) return;
        
            _paths.Add(new BlockPath(){ IsActive = true, Block = block});
        }

        public void RemoveNeighbors(BlockController block)
        {
            if (_paths.Exists(x => x.Block == block) == false) return;

            _paths.Remove(_paths.Find(x => x.Block == block));
        }

        public void ClearNeighborsList()
        {
            List<int> indexToRemove = new();
            for (var i = 0; i < _paths.Count; i++)
            {
                var path = _paths[i];
                if (path.Block == null) indexToRemove.Add(i);
            }
            indexToRemove.ForEach(index => _paths.RemoveAt(index));
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            DrawWalkPoint();
            DrawNeighborsConnections();
        }

        private void DrawNeighborsConnections()
        {
            if (_paths == null) return;
            foreach (BlockPath neighbor in _paths)
            {
                if (neighbor.Block == null) continue;
            
                Gizmos.color = neighbor.IsActive ? Color.green : Color.red;
                Gizmos.DrawLine(WalkPoint, neighbor.Block.WalkPoint);
            }
        }

        private void DrawWalkPoint()
        {
            Vector3 position = transform.position + _walkPoint;
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(position, 0.1f);
        }

#endif
    }
}