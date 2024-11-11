using System.Collections;
using Game;
using Game.Blocks;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectSetBlockedPath : Effect
    {
        public BlockController StartBlock;
        public BlockController EndBlock;
        public bool Blocked;
        
        public override Color EffectColorInEditor { get; } = new Color(1f, 0.58f, 0.92f);

        public override string ToString()
        {
            string startBlock = StartBlock == null ? "___" : $"{StartBlock.name}";
            string endBlock = EndBlock == null ? "___" : $"{EndBlock.name}";
            return $"{(Blocked ? "Block" : "Release")} {startBlock} to {endBlock} path";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            if (Blocked)
            {
                GameManager.Instance.BlockedPath.Add(new BlockToBlock(StartBlock, EndBlock));
                yield break;
            }
            
            for (int i = 0; i < GameManager.Instance.BlockedPath.Count; i++)
            {
                BlockToBlock path = GameManager.Instance.BlockedPath[i];
                if ((path.StartBlock == StartBlock && path.EndBlock == EndBlock) || (path.StartBlock == EndBlock && path.EndBlock == StartBlock))
                {
                    GameManager.Instance.BlockedPath.RemoveAt(i);
                    yield break;
                }
            }
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}