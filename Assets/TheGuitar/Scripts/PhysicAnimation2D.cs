using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PhysicAnimation2D : MonoBehaviour
{
    private readonly static int Stretch = Shader.PropertyToID("_Stretch");
    
    private readonly static int Squash = Shader.PropertyToID("_Squash");

    [SerializeField]
    private float stretchFactor = 0.2f;

    [SerializeField]
    private float squashFactor = 0.2f;

    [SerializeField]
    private float lerpSpeed = 8f;

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

        var speed = _velocity.magnitude;

        var stretch = 1 + Mathf.Clamp(speed * stretchFactor, 0, 0.5f);

        var squash = 1 - Mathf.Clamp(speed * squashFactor, 0, 0.3f);

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
        _mpb.SetFloat(Stretch, Mathf.Lerp(_mpb.GetFloat(Stretch), stretch, Time.deltaTime * lerpSpeed));

        _mpb.SetFloat(Squash, Mathf.Lerp(_mpb.GetFloat(Squash), squash, Time.deltaTime * lerpSpeed));

        _renderer.SetPropertyBlock(_mpb);
    }
}
