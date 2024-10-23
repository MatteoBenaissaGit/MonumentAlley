using DG.Tweening;
using UnityEngine;

public class StarVFX : MonoBehaviour
{
    [SerializeField] private float _minSize, _maxSize;
    [SerializeField] private float _animSpeedMin, _animSpeedMax;

    private bool _flipFlop;

    private void Start()
    {
        _flipFlop = UnityEngine.Random.Range(0, 10) > 5;
        LaunchAnim();
    }

    private void LaunchAnim()
    {
        float animSpeed = UnityEngine.Random.Range(_animSpeedMin, _animSpeedMax);
        transform.DOComplete();
        transform.DOScale(_flipFlop ? _minSize : _maxSize, animSpeed).OnComplete(() =>
        {
            _flipFlop = !_flipFlop;
            LaunchAnim();
        });
    }
}
