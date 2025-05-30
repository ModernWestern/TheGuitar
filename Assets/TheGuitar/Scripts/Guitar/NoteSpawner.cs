using Sago.Guitar;
using UnityEngine;
using System.Collections.Generic;

namespace Sago
{
    public class NoteSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject notePrefab;

        [SerializeField]
        private Transform spawnOrigin;

        [SerializeField]
        private int noteCount = 15;

        private readonly Queue<GameObject> _notes = new();

        private void Awake()
        {
            for (var i = 0; i < noteCount; i++)
            {
                var note = Instantiate(notePrefab, transform);

                note.SetActive(false);

                _notes.Enqueue(note);
            }
        }

        public void Spawn()
        {
            if (_notes.Count <= 0)
            {
                return;
            }

            var note = _notes.Dequeue().GetComponent<StringNote>();

            note.Init(transform.position, _notes.Enqueue);
        }
    }
}
