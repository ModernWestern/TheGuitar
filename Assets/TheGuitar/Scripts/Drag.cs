using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Drag : MonoBehaviour
{
    public bool IsDragging { get; private set; }

    [SerializeField]
    private float throwForceMultiplier = 1f;

    private Camera _cam;
    private Rigidbody2D _rb;

    private Vector3 _mouseVelocity;
    private Vector3 _lastMouseWorldPos;
    private Vector3 _currentMouseWorldPos;

    private void Awake()
    {
        _cam = Camera.main;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        _lastMouseWorldPos = MouseToWorldPosition();
        
        IsDragging = true;
    }

    private void OnMouseUp()
    {
        _rb.velocity = _mouseVelocity * throwForceMultiplier;
        
        IsDragging = false;
    }

    private void OnMouseDrag()
    {
        _currentMouseWorldPos = MouseToWorldPosition();

        transform.position = _currentMouseWorldPos;

        _mouseVelocity = (_currentMouseWorldPos - _lastMouseWorldPos) / Time.deltaTime;

        _lastMouseWorldPos = _currentMouseWorldPos;
    }

    private Vector3 MouseToWorldPosition()
    {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = 0;

        return mousePos;
    }
}
