using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sago.Guitar
{
    public class StringNote : MonoBehaviour
    {
        private readonly static int MainColor = Shader.PropertyToID("_Color");

        private event Action<GameObject> Dispose;

        private SpriteRenderer _renderer;

        private Rigidbody2D _rigidbody;

        public void Init()
        {
            _renderer = GetComponent<SpriteRenderer>();

            _rigidbody = GetComponent<Rigidbody2D>();

            gameObject.SetActive(false);
        }

        public void Prepare(Vector2 origin, Action<GameObject> onDispose)
        {
            Invoke(nameof(OnDispose), 4);

            transform.position = origin;

            gameObject.SetActive(true);

            Dispose = onDispose;

            SetColor(Color.HSVToRGB(Random.value, .9f, 1));

            Animation();
        }

        private void Animation()
        {
            var randomDir = Random.insideUnitCircle.normalized;

            float randomForce = Random.Range(500, 1000);

            _rigidbody.AddForce(randomDir * randomForce);
        }

        private void SetColor(Color color)
        {
            _renderer.material.SetColor(MainColor, color);
        }

        private void OnDispose()
        {
            Dispose?.Invoke(gameObject);

            gameObject.SetActive(false);
        }
    }
}
