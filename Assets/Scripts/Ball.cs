using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private Stick _stick;
    [SerializeField] private float _jumpForce;

    public event UnityAction FinishGame;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _stick.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = new Ray(transform.position, Vector3.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent(out Block block))
                {
                    _rigidbody.isKinematic = false;
                    _rigidbody.velocity = Vector3.zero;
                    StartCoroutine(HideStick());
                }
                else if (hitInfo.collider.TryGetComponent(out Segment segment))
                {
                    _rigidbody.isKinematic = true;
                    _rigidbody.velocity = Vector3.zero;
                    _stick.gameObject.SetActive(true);
                }
                else if (hitInfo.collider.TryGetComponent(out Finish finish))
                {
                    FinishGame?.Invoke();
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
}
