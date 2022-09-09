using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public float AnimationPosition;

    private void Update()
    {
        AnimationPosition = Mathf.Clamp(AnimationPosition, 0, 1);
        _animator.SetFloat("Blend", AnimationPosition);
    }
}
