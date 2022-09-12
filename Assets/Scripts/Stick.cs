using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Stick : MonoBehaviour
{
    [SerializeField] private Transform _bone;
    [SerializeField] private Animator _animator;
    [SerializeField] private Ball _ball;
    [Header("Materials")]
    [SerializeField] private Material _default;
    [SerializeField] private Material _invisible;

    public event UnityAction FinishGame;

    private float _animationPosition;
    private Rigidbody _rigidbody;
    private float _jumpForce;
    private bool _isFlew = false;
    private float _offsetY = 1;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("Blend", _animationPosition);

        if (Input.GetMouseButton(0) && _isFlew == false)
        {
            Stand();
            GetComponentInChildren<SkinnedMeshRenderer>().material = _default;
        }
        else if (Input.GetMouseButtonUp(0) && _isFlew == false)
        {
            Fly();
            _isFlew = true;
        }
        if (Input.GetMouseButtonDown(0) && _isFlew == true)
        {
            Ray ray = new Ray(transform.position, Vector3.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent(out Block block))
                {
                    _rigidbody.isKinematic = false;
                    _rigidbody.velocity = Vector3.zero;
                    StartCoroutine(TryHook());
                }
                else if (hitInfo.collider.TryGetComponent(out Segment segment))
                {
                    _rigidbody.isKinematic = true;
                    _rigidbody.velocity = Vector3.zero;

                    _isFlew = false;
                }
                else if (hitInfo.collider.TryGetComponent(out Finish finish))
                {
                    StartCoroutine(TryHook());
                    FinishGame?.Invoke();
                }
            }
        }
    }

    private void Fly()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _jumpForce = 0;
        _animationPosition = 0;
        GetComponentInChildren<SkinnedMeshRenderer>().material = _invisible;
    }

    private void Stand()
    {
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        _animationPosition = -mousePosition.y + _offsetY;
        _jumpForce = _animationPosition;
    }

    private IEnumerator TryHook()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = _default;
        yield return new WaitForSeconds(0.1f);
        GetComponentInChildren<SkinnedMeshRenderer>().material = _invisible;
    }
}
