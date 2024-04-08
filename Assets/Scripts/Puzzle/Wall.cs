using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Clotris.Puzzle
{
    public enum WallType
    {
        left,
        right,
        floor
    }
    public class Wall : MonoBehaviour
    {
        GameArea gameArea;
        [SerializeField]
        WallType wallType;

        void Start()
        {
            gameArea = transform.parent.GetComponent<GameArea>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Piece p = collision.transform.GetComponent<Piece>();
            if (p)
            {
                gameArea.WallCollision(p, wallType);
            } 
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Piece p = collision.transform.GetComponent<Piece>();
            Block b = collision.GetComponent<Block>();
            if (p)
            {
                if (wallType == WallType.right)
                {
                    p.ToggleMoveableRight();
                }

                if(wallType == WallType.left)
                {
                    p.ToggleMoveableLeft();
                }
            }
            else if (b)
            {
                float moveBack = 0.0f;
                moveBack = CheckWallRotation(b);
                if (moveBack != 0.0f)
                {
                    Piece attachedPiece = b.GetPiece();
                    attachedPiece.SetXPosition(attachedPiece.transform.position.x - moveBack);
                }
            }
        }


        private float CheckWallRotation(Block b)
        {
            if ((wallType == WallType.right && b.transform.position.x < transform.position.x)
                || (wallType == WallType.left && b.transform.position.x > transform.position.x)) return 0.0f;
            return b.transform.position.x - transform.position.x;
        }

        public WallType GetWallType()
        {
            return wallType;
        }
    }
}


