using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clotris.UI
{
    public class SavedPiece : MonoBehaviour
    {
        Transform savedPiece;

        public bool IsEmpty()
        {
            return savedPiece == null;
        }

        public void SetSavedPiece(Transform p)
        {
            savedPiece = p;

            DisplayPiece();
        }

        public Transform GetSavedPiece()
        {
            return savedPiece;
        }

        void DisplayPiece()
        {
            savedPiece.transform.SetParent(transform);
            savedPiece.transform.position = transform.position;
        }
    }
}
