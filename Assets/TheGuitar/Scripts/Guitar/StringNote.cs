using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sago.Guitar
{
    public class StringNote : MonoBehaviour
    {
        private event Action<GameObject> Dispose;

        private Vector2 _startPosition;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _startPosition = transform.position;

            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Init(Vector2 origin, Action<GameObject> onDispose)
        {
            Invoke(nameof(OnDispose), 4);

            transform.position = origin;

            gameObject.SetActive(true);

            Dispose = onDispose;

            Animation();
        }

        public void Animation()
        {
            var randomDir = Random.insideUnitCircle.normalized;

            float randomForce = Random.Range(500, 1000);

            _rigidbody.AddForce(randomDir * randomForce);
        }

        private void OnDispose()
        {
            Dispose?.Invoke(gameObject);

            gameObject.SetActive(false);
        }
    }
}
