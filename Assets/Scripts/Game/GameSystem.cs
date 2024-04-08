using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clotris.Control;
using Clotris.Puzzle;
using Clotris.UI;

using Random = UnityEngine.Random;

namespace Clotris.Game
{
    public class GameSystem : MonoBehaviour
    {
        PlayerController controller;

        [SerializeField]
        GameObject[] pieces;
        GameArea gameArea;
        float y_offset;
        float x_offset;

        Transform[,] gridPieces;
        bool[] lineCleared;

        [SerializeField] 
        PieceQueue pieceQueue;
        [SerializeField]
        SavedPiece savedPiece;
        [SerializeField]
        Score score;
        
        Vector3 pieceInitPosition;

        float totalGameTime = 0.0f;

        int level = 1;
        [SerializeField] float dropSpeed = 1.0f;
        float timeToNextDrop = 0.0f;
        bool gameOver = false;

        [SerializeField] int dropScore = 10;
        [SerializeField] int setScore = 10;
        [SerializeField] int clearScore = 100;



        // Start is called before the first frame update
        void Start()
        {
            Random.InitState((int)DateTime.Now.Ticks);

            if (pieceQueue == null)
            {
                pieceQueue = GameObject.FindWithTag("Queue").GetComponent<PieceQueue>();
            }
            if(pieceQueue != null)
            {
                for(int i = 0; i < pieceQueue.GetMaxQueue(); i++)
                {
                    Transform tempPiece = CreateNewPiece();
                    tempPiece.GetComponent<Piece>().DisablePieceCollider();
                    pieceQueue.EnqueuePiece(tempPiece);
                }
                pieceQueue.UpdateQueueDisplay();
            }

            if(savedPiece == null)
            {
                savedPiece = GameObject.FindWithTag("Saved Piece").GetComponent<SavedPiece>();
            }

            if(score == null)
            {
                score = GameObject.FindWithTag("Score").GetComponent<Score>();
            }


            gameArea = transform.Find("Game Area").GetComponent<GameArea>();
            y_offset = gameArea.GetFloorLocalPosition() + 0.5f;
            x_offset = gameArea.GetMaxLeftLocalPos();
            gridPieces = new Transform[(int)gameArea.GetAreaHeight(), (int)gameArea.GetAreaWidth()];
            lineCleared = new bool[(int)gameArea.GetAreaHeight()];
            pieceInitPosition = new Vector3(gameArea.transform.position.x, gameArea.transform.position.y + 11, gameArea.transform.position.z);
            controller = GetComponent<PlayerController>();

            Transform newPiece = CreateNewPiece();
            newPiece.SetParent(gameArea.transform);
            newPiece.position = pieceInitPosition;
            controller.SetNewPiece(newPiece);
            timeToNextDrop = 0.0f;

            controller.OnSavePiece += SavePiece;
        }

        // Update is called once per frame
        void Update()
        {
            if (gameOver)
            {
                return;
            }
            if (!controller.CheckCurrentPiece())
            {
                AssignPiece();
            }
            else if (controller.GetCurrentPiece().IsDropped)
            {
                bool cleared = SetGridLocation();
                if (cleared)
                {
                   ClearLines();
                }
                else if (gameOver)
                {
                    // Game is over! return and do not update further
                    return;
                }

                score.UpdateScore(dropScore + setScore);
                AssignPiece();
            }
            else if(timeToNextDrop >= dropSpeed)
            {
                if (controller.GetCurrentPiece().CanMoveDown)
                {
                    DropCurrentPiece();
                }
                timeToNextDrop = 0.0f;
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                controller.DestroyPieceFromGame();
                AssignPiece();
            }

            timeToNextDrop += Time.deltaTime;
            totalGameTime += Time.deltaTime;
        }

        bool SetGridLocation()
        {
            Piece p = controller.GetCurrentPiece();
            Transform[] blocks = p.GetBlocks();

            foreach (Transform block in blocks)
            {
                Vector2 localPosition = gameArea.transform.InverseTransformPoint(block.position);
                int y_pos = Mathf.RoundToInt(localPosition.y - y_offset);
                int x_pos = Mathf.RoundToInt(localPosition.x - x_offset);

                if(y_pos >= gameArea.GetAreaHeight())
                {
                    // There's no room for the piece to move downward on the puzzle board. The game is over
                    gameOver = true;
                }

                gridPieces[y_pos, x_pos] = block;
            }
           return CheckGridForClears();
        }

        /// <summary>
        /// Checks if there's a row that is full on the dpuzzle board.
        /// </summary>
        /// <returns>
        /// Index of array, which represents the y position on the puzzle board.
        /// Returns -1 if there are no full rows on the puzzle board
        /// </returns>
        bool CheckGridForClears()
        {
            bool hasClear = false;
            for(int i = 0; i < gameArea.GetAreaHeight(); i++)
            {
                int j;
                for(j = 0; j < gameArea.GetAreaWidth(); j++)
                {
                    if (gridPieces[i, j] == null) break;
                }
                if (j == gameArea.GetAreaWidth())
                {
                    hasClear = true;
                    lineCleared[i] = true;
                }
            }
            return hasClear;
        }

        Transform CreateNewPiece()
        {
            int i = (int)(int)(Random.value * 100000) % pieces.Length;
            return Instantiate(pieces[i], new Vector2(100, -100), new Quaternion()).transform;
        }

        void AssignPiece()
        {
            controller.RemovePiece();
            Transform newPiece = pieceQueue.DequeuePiece();
            ConfigureNewPiece(newPiece);
            controller.SetNewPiece(newPiece);

            newPiece = CreateNewPiece();
            newPiece.GetComponent<Piece>().DisablePieceCollider();

            pieceQueue.EnqueuePiece(newPiece);
           
            timeToNextDrop = 0.0f;
        }

        void ConfigureNewPiece(Transform p)
        {
            p.SetParent(gameArea.transform);
            p.position = pieceInitPosition;
        }



        void DropCurrentPiece()
        {
            controller.GetCurrentPieceTransform().Translate(Vector3.down, gameArea.transform);
        }

        void SavePiece()
        {
            if (!savedPiece.IsEmpty())
            {
                controller.GetCurrentPiece().ResetPieceRotation();
                Transform currentPiece = controller.GetCurrentPieceTransform();
                Transform newPiece = savedPiece.GetSavedPiece();
                ConfigureNewPiece(newPiece);
                controller.SetNewPiece(newPiece);
                savedPiece.SetSavedPiece(currentPiece);
            }
            else
            {
                // Saved Piece slot is empty
                controller.GetCurrentPiece().ResetPieceRotation();
                savedPiece.SetSavedPiece(controller.GetCurrentPieceTransform());
                Transform newPiece = CreateNewPiece();
                ConfigureNewPiece(newPiece);
                controller.SetNewPiece(newPiece);
            }
        }

        void ClearLines()
        {
            int firstLineCleared = -1;
            for (int i = 0; i < lineCleared.Length; i++)
            {
                if (lineCleared[i])
                {
                    for(int j = 0; j < gameArea.GetAreaWidth(); j++)
                    {
                        // Destroy this piecedd
                        Destroy(gridPieces[i, j].gameObject);
                        gridPieces[i, j] = null;
                    }
                    score.UpdateScore(clearScore);
                    if(firstLineCleared < 0)
                    {
                        firstLineCleared = i;
                    }
                }
            }

            if (firstLineCleared < 0) return;
            lineCleared[firstLineCleared] = false;
            int linesToMove = 1;
            for (int k = firstLineCleared + 1; k < gameArea.GetAreaHeight(); k++)
            {
                if (lineCleared[k])
                {
                    linesToMove++;
                    lineCleared[k] = false;
                    continue;
                }
                for (int j = 0; j < gameArea.GetAreaWidth(); j++) 
                {
                    // Check if the piece above must be moved down
                    if (gridPieces[k, j] != null)
                    {
                        gridPieces[k, j].Translate(Vector3.down*linesToMove, gameArea.transform);
                        gridPieces[k - linesToMove, j] = gridPieces[k, j];
                        gridPieces[k, j] = null;
                    }
                }
            }
        }
    }
}


