using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clotris.Puzzle
{
    public class GameArea : MonoBehaviour
    {
        // Start is called before the first frame update
        float maxLeftPos = 0.0f;
        float maxRightPos = 0.0f;
        BoxCollider2D bc2D;
        float floorPosition = 0.0f;
        [SerializeField] float topPosition = 10.0f;

        [SerializeField] 
        int areaWidth = 10;
        [SerializeField]
        int areaHeight = 22;

        [SerializeField]
        GameObject leftWall;
        [SerializeField]
        GameObject rightWall;
        [SerializeField]
        GameObject floor;

        private void Awake()
        {
            bc2D = GetComponent<BoxCollider2D>();
            if(leftWall == null)
            {
                leftWall = GameObject.Find("Left Wall");       
            }
            maxLeftPos = leftWall.transform.position.x;
            if (rightWall == null)
            {
                rightWall = GameObject.Find("Right Wall");
                
            }
            maxRightPos = rightWall.transform.position.x;
            if (floor == null)
            {
                floor = GameObject.Find("Floor");
            }
            floorPosition = floor.transform.position.y;
            topPosition = floorPosition + 21;
        }

        public void WallCollision(Piece puzzlePiece, WallType wallType)
        {
            if(wallType == WallType.left)
            {
                puzzlePiece.ToggleMoveableLeft();
            }
            else if (wallType == WallType.right)
            {
                puzzlePiece.ToggleMoveableRight();
            }
            else if (wallType == WallType.floor)
            {
                puzzlePiece.SetIsDropped(true);
                puzzlePiece.SetMoveableDown(false);
            }
        }

        public float GetFloorPosition()
        {
            return floorPosition;
        }

        public float GetFloorLocalPosition()
        {
            return floor.transform.localPosition.y;
        }

        public float GetTopPosition()
        {
            return topPosition;
        }

        public int GetAreaWidth()
        {
            return areaWidth;
        }

        public float GetMaxLeftLocalPos()
        {
            return leftWall.transform.localPosition.x;
        }

        public int GetAreaHeight()
        {
            return areaHeight;
        }
    }
}