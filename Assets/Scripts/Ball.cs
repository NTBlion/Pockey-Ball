using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private Stick _stick;
    [SerializeField] private Transform _bone;
    [SerializeField] private Ball _ball;

    public event UnityAction FinishGame;

    private Rigidbody _rigidbody;
    private float _jumpForce = 1;
    private bool _isFlew = false;
    private float _offsetY = 1;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && _isFlew == false)
        {
            _stick.transform.SetParent(null);
            _ball.transform.SetParent(_bone.transform);
            Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            _stick.AnimationPosition = Mathf.Clamp(_stick.AnimationPosition, 0, 1);
            _stick.AnimationPosition = -mousePosition.y +_offsetY;
            _jumpForce = _stick.AnimationPosition;
        }
        else if(Input.GetMouseButtonUp(0) && _isFlew == false)
        {
            _ball.transform.SetParent(null);
            _stick.transform.SetParent(_ball.transform, true);
            Fly();
            _isFlew=true;
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
                    StartCoroutine(HideStick());
                    _isFlew = true;
                }
                else if (hitInfo.collider.TryGetComponent(out Segment segment))
                {
                    _rigidbody.isKinematic = true;
                    _rigidbody.velocity = Vector3.zero;
                    _stick.gameObject.SetActive(true);
                    _isFlew = false;
                }
                else if (hitInfo.collider.TryGetComponent(out Finish finish))
                {
                    FinishGame?.Invoke();
                    StartCoroutine(HideStick());
                }
            }
        }
    }

    private IEnumerator HideStick()
    {
        _stick.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _stick.gameObject.SetActive(false);
    }

    private void Fly()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _stick.gameObject.SetActive(false);
        _jumpForce = 0;
        _stick.AnimationPosition = Mathf.Clamp(_stick.AnimationPosition,0,1);
    }
}
