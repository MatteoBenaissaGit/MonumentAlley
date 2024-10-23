using System;
using System.Collections.Generic;
using DG.Tweening;
using Game;
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
            _moveSequence.Join(transform.DOLookAt(path[i].WalkPoint, 0.1f, AxisConstraint.Y, Vector3.up).SetEase(Ease.Linear));
        }
        _moveSequence.AppendCallback(() => _animator.SetBool(Walking, false));
        
        _moveSequence.Play();
    }

    private List<BlockController> GetPathTo(BlockController from, BlockController to)
    {
        List<BlockController> previousBlocks = new();
        List<BlockController> path = new();

        List<BlockController> pathTo = RecursivePathSearch(previousBlocks, from, to, path);
        return pathTo;
    }

    
    private List<BlockController> RecursivePathSearch(List<BlockController> previous, BlockController current, BlockController destination, List<BlockController> path)
    {
        List<BlockController> localPath = new List<BlockController>(path) { current };

        if (current == destination)
        {
            return localPath;
        }

        List<BlockController> localPrevious = new (previous) { current };

        foreach (BlockPath neighbor in current.Paths)
        {
            if (neighbor.IsActive == false || previous.Contains(neighbor.Block)) continue;
            List<BlockController> neighborPath = RecursivePathSearch(localPrevious, neighbor.Block, destination, localPath);
            if (neighborPath != null) return neighborPath;
        }

        return null;
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
