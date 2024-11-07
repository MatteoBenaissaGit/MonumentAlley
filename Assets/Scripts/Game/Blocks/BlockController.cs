using System;
using System.Collections.Generic;
using Game.Blocks;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class BlockPath
    {
        public bool IsActive = true;
        public BlockController Block;
        public bool ForceDirection;
        public Vector2Int Direction;
    }
    
    public enum BlockType
    {
        Normal = 0,
        Stair = 1,
        Ladder = 2
    }

    public class BlockController : MonoBehaviour
    {
        public Vector3 WalkPoint
        {
            get
            {
                if (_localRelative)
                {
                    Vector3 up = transform.up * _walkPoint.y;
                    Vector3 right = transform.right * _walkPoint.x;
                    Vector3 forward = transform.forward * _walkPoint.z;
                    return transform.position + up + right + forward;
                }
                return transform.position + _walkPoint;
            }
        }
        public List<BlockPath> Paths => _paths;
        public BlockType Type => _type;
        public MovingPart MovingPart { get; set; }
    
        [SerializeField] private Vector3 _walkPoint;
        [SerializeField] private bool _localRelative;
        [SerializeField] private List<BlockPath> _paths;
        [SerializeField] private BlockType _type;

        private Dictionary<BlockController, BlockPath> _pathsToBlocks;

        private void Awake()
        {
            _pathsToBlocks = new Dictionary<BlockController, BlockPath>();
            foreach (BlockPath path in _paths)
            {
                _pathsToBlocks.Add(path.Block, path);
            }
        }

        public void AddNeighbors(BlockController block, bool isActive = true)
        {
            if (_paths.Exists(x => x.Block == block)) return;
        
            _paths.Add(new BlockPath(){ IsActive = isActive, Block = block});
        }

        public void RemoveNeighbors(BlockController block)
        {
            if (_paths.Exists(x => x.Block == block) == false) return;

            _paths.Remove(_paths.Find(x => x.Block == block));
        }

        public void ClearNeighborsList(bool clearAll = false)
        {
            if (clearAll)
            {
                _paths = new();
            }
            
            List<int> indexToRemove = new();
            for (var i = 0; i < _paths.Count; i++)
            {
                var path = _paths[i];
                if (path.Block == null) indexToRemove.Add(i);
            }
            indexToRemove.ForEach(index => _paths.RemoveAt(index));
        }

        public void SetPathToActive(BlockController to, bool isActive, bool callback = true)
        {
            if (_pathsToBlocks.TryGetValue(to, out BlockPath path) == false)
            {
                return;
            }
            path.IsActive = isActive;
            if (callback)
            {
                to.SetPathToActive(this, isActive, false);
            }
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
                Vector3 startPoint = WalkPoint;
                Vector3 endPoint = neighbor.Block.WalkPoint;
                
                Vector3 direction = (endPoint - startPoint).normalized;
                Vector3 normal = new Vector3(-direction.z, 0, direction.x);
                Vector3 offset = normal * 0.1f;
                Vector3 middlePoint = (startPoint + endPoint) / 2 + offset;
                
                Gizmos.DrawLine(startPoint, middlePoint);
                Gizmos.DrawLine(middlePoint, endPoint);
            }
        }

        private void DrawWalkPoint()
        {
            Vector3 position = WalkPoint;
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(position, 0.1f);
        }

#endif
    }
}