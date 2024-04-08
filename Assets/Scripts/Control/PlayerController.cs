using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Clotris.Puzzle;
using System;

namespace Clotris.Control
{
    public delegate void SavePiece();

    public class PlayerController : MonoBehaviour
    {
        Transform currentPieceTransform;
        Piece currentPiece;

        KeyCode rightButton = KeyCode.D;
        KeyCode leftButton = KeyCode.A;
        KeyCode rightRotation = KeyCode.E;
        KeyCode leftRotation = KeyCode.Q;
        KeyCode dropKey = KeyCode.Space;
        KeyCode savePiece = KeyCode.X;

        public event SavePiece OnSavePiece;

        // Update is called once per frame
        private void Update()
        {
            if (currentPiece == null) return;

            if(Input.GetKeyDown(leftButton))
            {
                MovePiece(leftButton);
            }
            if (Input.GetKeyDown(rightButton))
            {
                MovePiece(rightButton);
            }
            if (Input.GetKeyDown(rightRotation))
            {
                RotatePiece(rightRotation);
            }
            if (Input.GetKeyDown(leftRotation))
            {
                RotatePiece(leftRotation);
            }
            if (Input.GetKeyDown(dropKey))
            {
                DropPiece();
            }
            if (Input.GetKeyDown(savePiece))
            {
                OnSavePiece?.Invoke();
            }
        }

        void MovePiece(KeyCode direction)
        {
            if (direction == rightButton)
                currentPiece.MovePieceHorizontal(Vector2.right);
            else if (direction == leftButton)
                currentPiece.MovePieceHorizontal(Vector2.left);
        }

        void RotatePiece(KeyCode direction)
        {
            if(direction == rightRotation)
            {
                currentPiece.RotatePiece(-1.0f);
            }
            else if (direction == leftRotation)
            {
                currentPiece.RotatePiece(1.0f);
            }
        }

        void DropPiece()
        {
            currentPiece.DropPiece();
        }

        public bool CheckCurrentPiece()
        {
            if (currentPiece == null)
            {
                return false;
            }
            return true;
        }

       
        public void SetNewPiece(Transform p)
        {
            currentPiece = p.GetComponent<Piece>();
            currentPiece.EnablePiece();
            currentPieceTransform = p;
        }

        public void SetNewPiece(Piece p)
        {
            currentPiece = p;
            currentPiece.EnablePiece();
            currentPieceTransform = p.transform;
        }

        public Transform GetCurrentPieceTransform()
        {
            return currentPieceTransform;
        }

        public Piece GetCurrentPiece()
        {
            return currentPiece;
        }

        public void RemovePiece()
        {
            if (currentPiece)
            {
                currentPiece.BreakUpPiece();
                currentPiece = null;
            }
        }

        public void DestroyPieceFromGame()
        {
            Destroy(currentPieceTransform.gameObject);
            currentPiece = null;
        }
    }
}


