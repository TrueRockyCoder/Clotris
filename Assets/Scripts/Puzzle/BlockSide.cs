using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clotris.Puzzle
{
    public class BlockSide : MonoBehaviour
    {
        Block b;
        private void Start()
        {
            b = transform.parent.GetComponent<Block>();
        }

        public bool IsTopSide()
        {
            return Mathf.RoundToInt(GetSide().y) == 1;
        }


        public Vector2 GetSide()
        {
            return (transform.position - transform.parent.position).normalized;
        }

        public Block GetAttachedBlock()
        {
            return b;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Piece p = collision.GetComponent<Piece>();

            if (p == null) return;

            Vector2 side = GetSide();
            if(side.x == 1)
            {
                // Points right; Prevent piece from moving right
                p.ToggleMoveableRight();
            }
            else if(side.x == -1)
            {
                // Points left
                p.ToggleMoveableLeft();
            }
            else if(side.y == 1)
            {
                // Points up
                p.SetIsDropped(true);
                p.SetMoveableDown(false);
            }
            else if(side.y == -1)
            {
                // Points down (Do Nothing)
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Piece p = collision.GetComponent<Piece>();

            if (p == null) return;

            Vector2 side = GetSide();
            if (side.x == 1)
            {
                // Points right; Prevent piece from moving left, into the right side
                p.ToggleMoveableLeft();
            }
            else if (side.x == -1)
            {
                // Points left; Prevent piece from moving right, into the left side
                p.ToggleMoveableRight();
            }
            else if (side.y == 1)
            {
                // Points up
                p.SetIsDropped(false);
                p.SetMoveableDown(true);
            }
            else if (side.y == -1)
            {
                // Points down (Do Nothing)
            }
        }
    }
}


