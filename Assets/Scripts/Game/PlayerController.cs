using System;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using Game.Blocks;
using IshLib.Pathfinding;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BlockController _startBlock;
    [SerializeField] private Animator _animator;
    
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int Climbing = Animator.StringToHash("climbing");
    private static readonly int ClimbValue = Animator.StringToHash("climbValue");
    private Sequence _moveSequence;
    
    private void Start()
    {
        transform.position = _startBlock.WalkPoint;
    }
    
    public void GoToBlock(BlockController block)
    {
        BlockController currentBlock = GetBlockDown();
        if (currentBlock == null) return;

        MovingPart movingPart = currentBlock.MovingPart;
        
        List<BlockController> path = GetPathTo(currentBlock, block);
        if (path == null) return;

        _moveSequence?.Kill();
        _moveSequence = DOTween.Sequence();
        _moveSequence.AppendCallback(() => _animator.SetBool(Walking, true));
        for (int i = 1; i < path.Count; i++)
        {
            BlockController pathBlock = path[i];

            //move
            bool nextBlockIsLadder = i != path.Count - 1 && path[i + 1].Type == BlockType.Ladder;
            if (pathBlock.Type == BlockType.Ladder)
            {
                if (i == 1 && path[i-1].LadderTop)
                {
                    _moveSequence.Append(transform.DOMove(path[i-1].WalkPoint, 0.35f).SetEase(Ease.Linear));
                }
                
                bool previousBlockWasLadder = path[i-1].Type == BlockType.Ladder;
                bool endLadder = i == path.Count - 1 || path[i + 1].Type != BlockType.Ladder;
                bool endLadderOnLadderTop = (i == path.Count - 1 && pathBlock.LadderTop) || (path[i + 1].Type != BlockType.Ladder && path[i + 1].LadderTop);
                if (endLadder)
                {
                    if (previousBlockWasLadder)
                    {
                        _moveSequence.Append(transform.DOMove(path[i].WalkPoint, 0.35f).SetEase(Ease.Linear));
                        if (pathBlock.LadderTop)
                        {
                            _moveSequence.Append(transform.DOMove(path[i].LadderTopWalkPoint, 0.35f).SetEase(Ease.Linear));
                        }
                    }
                    if (endLadderOnLadderTop)
                    {
                        _moveSequence.Append(transform.DOMove(path[i].LadderTopWalkPoint, 0.35f).SetEase(Ease.Linear));
                    }
                    _moveSequence.JoinCallback(() => _animator.SetBool(Climbing, false));
                }
                else if (previousBlockWasLadder == false || pathBlock.LadderTop)
                {
                    if (pathBlock.LadderTop)
                    {
                        _moveSequence.Append(transform.DOMove(path[i].LadderTopWalkPoint, 0.35f).SetEase(Ease.Linear));
                        _moveSequence.Append(transform.DOMove(path[i].WalkPoint, 0.35f).SetEase(Ease.Linear));
                    }
                    else
                    {
                        _moveSequence.Append(transform.DOMove(path[i].WalkPoint, 0.35f).SetEase(Ease.Linear));
                    }
                    _moveSequence.JoinCallback(() => _animator.SetBool(Climbing, true));
                }
                else
                {
                    _moveSequence.Append(transform.DOMove(path[i].WalkPoint, 0.35f).SetEase(Ease.Linear));
                    _moveSequence.JoinCallback(() => _animator.SetBool(Climbing, true));
                }
            }
            else
            {
                Vector3 offset = Vector3.zero;
                if (nextBlockIsLadder && path[i+1].LadderTop == false)
                {
                    Vector3 directionToNextBlock = (path[i+1].WalkPoint - path[i].WalkPoint);
                    directionToNextBlock.y = 0;
                    directionToNextBlock = directionToNextBlock.normalized;
                    offset = directionToNextBlock * 0.25f;
                }
                _moveSequence.Append(transform.DOMove(path[i].WalkPoint + offset, 0.35f).SetEase(Ease.Linear));
            }
            if (nextBlockIsLadder && path[i+1].LadderTop == false)
            {
                _moveSequence.JoinCallback(() => _animator.SetBool(Climbing, true));
            }
            
            //look
            if (pathBlock.Type != BlockType.Ladder || pathBlock.LadderTop == false)
            {
                Vector3 pointToLook = pathBlock.Type == BlockType.Ladder ? pathBlock.LadderTopWalkPoint : pathBlock.WalkPoint;
                BlockPath pathToNewBlock = path[i-1].Paths.Find(x => x.Block == pathBlock);
                if (pathToNewBlock.ForceDirection)
                {
                    Vector3 direction = new Vector3(pathToNewBlock.Direction.x, 0, pathToNewBlock.Direction.y);
                    Debug.DrawLine(path[i-1].WalkPoint, path[i-1].WalkPoint + direction, Color.cyan, 5f);
                    pointToLook = path[i-1].WalkPoint + direction;
                }
                _moveSequence.Join(transform.DOLookAt(pointToLook, 0.1f, AxisConstraint.Y, Vector3.up).SetEase(Ease.Linear));
            }

            //moving part
            if (pathBlock.MovingPart != null && pathBlock.MovingPart != movingPart)
            {
                int step = pathBlock.MovingPart.CurrentStep;
                _moveSequence.JoinCallback(() => CheckMovingPartAtPlayerPath(pathBlock.MovingPart, _moveSequence, step));
                _moveSequence.JoinCallback(() => pathBlock.MovingPart.PlayerIsOnMovingPart(true));
                movingPart = pathBlock.MovingPart;
            }
            if (pathBlock.MovingPart != movingPart && movingPart != null)
            {
                MovingPart part = movingPart;
                _moveSequence.JoinCallback(() => part.PlayerIsOnMovingPart(false));
                movingPart = null;
            }

            //parent
            Transform currentBlockTransform = path[i].transform;
            _moveSequence.JoinCallback(() => transform.parent = currentBlockTransform);
            
            //end level
            if (pathBlock == GameManager.Instance.EndBlock)
            {
                _moveSequence.JoinCallback(() => GameManager.Instance.EndLevel());
            }
            
        }
        _moveSequence.AppendCallback(() => _animator.SetBool(Walking, false));
        
        _moveSequence.Play();
    }

    private void CheckMovingPartAtPlayerPath(MovingPart part, Sequence pathSequence, int step)
    {
        if (part.IsPressed == false && step == part.CurrentStep) return;
        pathSequence.Kill();
        _animator.SetBool(Walking, false);
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
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = Vector3.down;
        if (Physics.Raycast(origin,direction, out RaycastHit hit, 1))
        {
            if (hit.transform.gameObject.TryGetComponent(out BlockController block) == false)
            {
                return null;
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
