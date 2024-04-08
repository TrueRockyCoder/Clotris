using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Clotris.Puzzle
{
    public class Block : MonoBehaviour
    {
        [SerializeField]
        BoxCollider2D[] sideColliders;
        Piece p;
        private void Start()
        {
            p = transform.parent.GetComponent<Piece>();
            if (sideColliders.Length != 4)
            {
                sideColliders = new BoxCollider2D[4];
                int i = 0;
                foreach(BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>())
                {
                    sideColliders[i] = box;
                    i++;
                }
            }
        }

        public void RemoveCollidersFromComposite()
        {
            foreach(BoxCollider2D box in sideColliders)
            {
                box.usedByComposite = false;
                box.isTrigger = true;
            }
            Destroy(GetComponent<CompositeCollider2D>());
        }

        public Piece GetPiece()
        {
            return p;
        }

        public float GetXPos()
        {
            return transform.position.x;
        }

        public float GetYPos()
        {
            return transform.position.y;
        }

    }
}
