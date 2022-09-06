using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public float _power;

    private void Update()
    {
        _power = Mathf.Clamp(_power, 0, 1);
        _animator.SetFloat("Blend", _power);
    }
}
