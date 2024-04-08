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
            BlockSide bS = collision.GetComponent<BlockSide>();
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
            else if (bS)
            {
                switch (wallType){
                    case WallType.left:
                        if(bS.GetSide().x == -1 && GetDistanceBetweenPoints(bS.GetAttachedBlock().GetXPos()) <= -1)
                        {
                            // Push the piece back by one
                            bS.GetAttachedBlock().GetPiece().WallPush(Vector2.right);
                        }
                        break;
                    case WallType.right:
                        if (bS.GetSide().x == 1 && GetDistanceBetweenPoints(bS.GetAttachedBlock().GetXPos()) >= 1)
                        {
                            // Push the piece back by one
                            bS.GetAttachedBlock().GetPiece().WallPush(Vector2.left);
                        }
                        break;
                    case WallType.floor:
                        if (bS.GetSide().y == -1 && (bS.GetAttachedBlock().GetYPos() - transform.position.y >= 1))
                        {
                            // Push the piece back by one
                            bS.GetAttachedBlock().GetPiece().WallPush(Vector2.up);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private int GetDistanceBetweenPoints(float sideX)
        {
            return Mathf.RoundToInt(sideX - transform.position.x);
        }

        public WallType GetWallType()
        {
            return wallType;
        }
    }
}


