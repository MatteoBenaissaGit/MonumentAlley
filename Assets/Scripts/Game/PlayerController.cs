using System;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using IshLib.Pathfinding;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BlockController _startBlock;
    [SerializeField] private Animator _animator;
    
    private static readonly int Walking = Animator.StringToHash("walking");
    private Sequence _moveSequence;
    
    private void Start()
    {
        transform.position = _startBlock.WalkPoint;
    }
    
    public void GoToBlock(BlockController block)
    {
        var currentBlock = GetBlockDown();
        if (currentBlock == null) return;

        List<BlockController> path = GetPathTo(currentBlock, block);
        if (path == null) return;

        _moveSequence?.Kill();
        _moveSequence = DOTween.Sequence();
        _moveSequence.AppendCallback(() => _animator.SetBool(Walking, true));
        for (int i = 1; i < path.Count; i++)
        {
            _moveSequence.Append(transform.DOMove(path[i].WalkPoint, 0.35f).SetEase(Ease.Linear));

            Vector3 pointToLook = path[i].WalkPoint;
            var pathToNewBlock = path[i-1].Paths.Find(x => x.Block == path[i]);
            if (pathToNewBlock.ForceDirection)
            {
                 pointToLook = path[i].WalkPoint+ new Vector3(pathToNewBlock.Direction.x, 0, pathToNewBlock.Direction.y);
            }
            _moveSequence.Join(transform.DOLookAt(pointToLook, 0.1f, AxisConstraint.Y, Vector3.up).SetEase(Ease.Linear));
        }
        _moveSequence.AppendCallback(() => _animator.SetBool(Walking, false));
        
        _moveSequence.Play();
    }

    private List<BlockController> GetPathTo(BlockController from, BlockController to)
    {
        List<BlockController> path = Dijkstra.GetClosestNode(from,
            GetDistanceBetweenTiles,
            (currentNode) => GetNeighbours(currentNode.Node),
            (currentNode) => currentNode.Node == to);

        return path;
    }

    private BlockController[] GetNeighbours(BlockController currentBlock)
    {
        List<BlockController> neighbors = new List<BlockController>();
        
        foreach (BlockPath path in currentBlock.Paths)
        {
            if (path.IsActive == false) continue;
            neighbors.Add(path.Block);
        }
        
        return neighbors.ToArray();
    }

    private float GetDistanceBetweenTiles(BlockController origin, BlockController destination)
    {
        return Vector3.Distance(origin.transform.position, destination.transform.position);
    }
    
    private BlockController GetBlockDown()
    {
        RaycastHit[] hits = new RaycastHit[8];
        Ray ray = new Ray() { origin = transform.position + Vector3.up, direction = Vector3.down };
        int cast = Physics.RaycastNonAlloc(ray, hits, 10);
        for (int i = 0; i < cast; i++)
        {
            if (hits[i].transform.gameObject == null || hits[i].transform.gameObject.TryGetComponent(out BlockController block) == false)
            {
                continue;
            }
            return block;
        }
        return null;
    }
    
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down);
    }

#endif
}
