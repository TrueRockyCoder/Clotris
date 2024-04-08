using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clotris.UI
{
    public class PieceQueue : MonoBehaviour
    {
        Queue<Transform> queue;
        [SerializeField]int maxQueue = 6;
        bool queueChange;

        [SerializeField]
        Transform queueStart;

        float pieceQueueOffset = 3f;

        private void Awake()
        {
            queue = new Queue<Transform>(maxQueue);
        }

        private void Start()
        {
            if(queueStart == null)
            {
                queueStart = transform.Find("Queue Start");
            }
        }

        private void Update()
        {
            if (queueChange)
            {
                UpdateQueueDisplay();
            }
        }

        public void EnqueuePiece(Transform p)
        {
            if (queue.Count < maxQueue)
            {
                queue.Enqueue(p);
                p.SetParent(transform);
                queueChange = true;
            }
        }

        public Transform DequeuePiece()
        {
            queueChange = true;
            return queue.Dequeue();
        }

        public int GetMaxQueue()
        {
            return maxQueue;
        }

        public void UpdateQueueDisplay()
        {
            float piecePosition = pieceQueueOffset;
            
            foreach(Transform p in queue)
            {
                p.position = new Vector2(queueStart.position.x, queueStart.position.y - piecePosition);
                piecePosition += pieceQueueOffset;
            }
            queueChange = false;
        }
    }
}

