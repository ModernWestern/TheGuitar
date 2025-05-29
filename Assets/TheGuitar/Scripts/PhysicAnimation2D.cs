using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PhysicAnimation2D : MonoBehaviour
{
    private readonly static int Squash = Shader.PropertyToID("_Squash");
    private readonly static int Stretch = Shader.PropertyToID("_Stretch");
    
    public float stretchFactor = 0.2f;
    public float squashFactor = 0.2f;
    public float lerpSpeed = 8f;

    private MaterialPropertyBlock _mpb;
    private SpriteRenderer _renderer;
    private Vector3 _lastPosition;
    private Vector3 _velocity;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _mpb = new MaterialPropertyBlock();
        _lastPosition = transform.position;

        _renderer.GetPropertyBlock(_mpb);
    }

    private void Update()
    {
        _velocity = (transform.position - _lastPosition) / Time.deltaTime;

        float speed = _velocity.magnitude;
        float stretch = 1 + Mathf.Clamp(speed * stretchFactor, 0, 0.5f);
        float squash = 1 - Mathf.Clamp(speed * squashFactor, 0, 0.3f);

        // Si el movimiento es mayor en Y, estira verticalmente
        if (Mathf.Abs(_velocity.y) > Mathf.Abs(_velocity.x))
        {
            ApplyDeformation(squash, stretch);
        }
        else
        {
            ApplyDeformation(stretch, squash);
        }

        _lastPosition = transform.position;
    }

    private void ApplyDeformation(float squash, float stretch)
    {
        // _renderer.GetPropertyBlock(_mpb);

        Vector2 dir = _velocity.normalized;
        if (dir.sqrMagnitude < 0.001f) dir = Vector2.right; // Dirección por defecto si está casi quieto

        _mpb.SetFloat(Squash, Mathf.Lerp(_mpb.GetFloat(Squash), squash, Time.deltaTime * lerpSpeed));
        _mpb.SetFloat(Stretch, Mathf.Lerp(_mpb.GetFloat(Stretch), stretch, Time.deltaTime * lerpSpeed));
        // _mpb.SetVector("_Direction", new Vector4(dir.x, dir.y, 0, 0));

        _renderer.SetPropertyBlock(_mpb);
    }
}
