using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clotris.Puzzle
{
    public enum pieceType
    {
        i,
        j,
        l,
        o,
        s,
        t,
        z
    }

    public class Piece : MonoBehaviour
    {
        public pieceType type = pieceType.i;
        Rigidbody2D rb;

        [SerializeField]
        Transform[] blockTransforms;

        bool _isDropped = false;

        public bool IsDropped
        {
            get
            {
                return _isDropped;
            }
        }

        public void SetIsDropped(bool drop)
        {
            _isDropped = drop;
        }

        bool _canMoveDown = true;
        bool _canMoveRight = true;
        bool _canMoveLeft = true;

        public bool CanMoveDown
        {
            get
            {
                return _canMoveDown;
            }
        }
        public bool CanMoveRight
        {
            get
            {
                return _canMoveRight;
            }
        }
        public bool CanMoveLeft
        {
            get
            {
                return _canMoveLeft;
            }
        }

        [SerializeField] Transform pivot;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            if(pivot == null)
            {
                pivot = transform.Find("Pivot");
            }
            if(blockTransforms.Length != 4)
            {
                blockTransforms = new Transform[4];
                int i = 0;
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("Block"))
                    {
                        blockTransforms[i] = child;
                        i++;
                    }
                }
            }
        }

        public void MovePieceHorizontal(Vector2 direction)
        {
            if((direction.x > 0 && _canMoveRight) || (direction.x < 0 && _canMoveLeft))
            {
                rb.MovePosition(rb.position + direction);
            }
            
        }

        public void SetXPosition(float xpos)
        {
            transform.position = new Vector2(xpos, rb.position.y);
        }

        public void WallPush(Vector2 direction)
        {
            transform.Translate(direction, Space.World);
        }

        public float GetRightPosition()
        {
            float furthestRight = 0.0f;
            foreach(Transform block in blockTransforms)
            {
                if (block.position.x > furthestRight) furthestRight = block.position.x;
            }
            return furthestRight;
        }

        public float GetLeftPosition()
        {
            float furthestLeft = 0.0f;
            foreach (Transform block in blockTransforms)
            {
                if (block.position.x < furthestLeft) furthestLeft = block.position.x;
            }
            return furthestLeft;
        }

        public void SetYPosition(float ypos)
        {
            transform.position = new Vector2(rb.position.x, ypos);
        }

        public void MovePieceVertical(float y)
        {
            rb.MovePosition(new Vector2(rb.position.x, rb.position.y + y));
        }

        public void RotatePiece(float rotationDirection)
        {
            transform.RotateAround(pivot.position, Vector3.forward, 90 * rotationDirection);
        }

        public void DropPiece()
        {
            List<RaycastHit2D> validHits = new();

            foreach(Transform block in blockTransforms)
            {
                foreach (RaycastHit2D hit in Physics2D.RaycastAll(block.position, Vector2.down, 30f))
                {
                    // Verify if any Collision is with this object or any children (Blocks, Block Sides)
                    if (hit.transform.IsChildOf(transform)) continue;
                    Wall w = hit.transform.GetComponent<Wall>();
                    if (w && w.GetWallType() != WallType.floor) continue;
                    if (hit.transform.GetComponent<BlockSide>() && !hit.transform.GetComponent<BlockSide>().IsTopSide()) continue;

                    validHits.Add(hit);
                }
            }

            if (validHits.Count <= 0) return;


            RaycastHit2D closestHit = validHits[0];
            

            foreach (RaycastHit2D hit in validHits)
            {
                if (hit.distance < closestHit.distance) closestHit = hit;
            }

            if (closestHit.transform.GetComponent<BlockSide>() != null)
            {
                // closestHit is the top side of another Piece
                MovePieceVertical(-closestHit.distance + 0.5f);
            }
            else if (closestHit.transform.GetComponent<Wall>() != null)
            {
                MovePieceVertical(-closestHit.distance);
            }
        }

        public void SetMoveableDown(bool move)
        {
            _canMoveDown = move;
        }

        public void ToggleMoveableRight()
        {
            _canMoveRight = _canMoveRight ? false : true;
        }

        public void ToggleMoveableLeft()
        {
            _canMoveLeft = _canMoveLeft ? false : true;
        }

        /// <summary>
        /// Function <c>BreakUpPiece</c><br></br>
        /// Separates the Blocks From the Piece. Sets the blocks parent to this piece's parent
        /// <br></br> Destorys this piece
        /// </summary>
        public void BreakUpPiece()
        {
            foreach (Transform block in blockTransforms)
            {
                block.GetComponent<Block>().RemoveCollidersFromComposite();
                block.SetParent(this.transform.parent);
            }
            Destroy(this.gameObject);
        }

        public void EnablePiece()
        {
            rb.simulated = true;
        }

        public void DisablePieceCollider()
        {
            rb.simulated = false;
        }

        public void ResetPieceRotation()
        {
            transform.rotation = new Quaternion();
        }

        public Transform[] GetBlocks()
        {
            return blockTransforms;
        }
    }
}


