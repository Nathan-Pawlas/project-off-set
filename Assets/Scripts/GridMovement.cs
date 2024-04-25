using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    // Time in seconds to move between one grid position and the next.
    [SerializeField] private float moveDuration = 0.1f;
    // The size of the grid
    [SerializeField] private float gridSize = 1f;
    // Bool Determines if Movement is Mirored
    [SerializeField] private bool mirored_Player = false;
    //Grid Controlller for Determining whether a move is valid
    private GridController gridController;

    private bool m_IsMoving = false;



    //Bool Flag To Determine if Player is on End Goal
    [SerializeField] public bool onGoal = false;


    // Update is called once per frame
    void Update()
    {
        if (!m_IsMoving)
        {
            System.Func<KeyCode, bool> inputFunction = Input.GetKeyDown;

            if (inputFunction(KeyCode.W))
            {
                StartCoroutine(Move(Vector2.up));
            }
            else if (inputFunction(KeyCode.S))
            {
                StartCoroutine(Move(Vector2.down));
            }
            else if (inputFunction(KeyCode.D))
            {
                if (mirored_Player)
                {
                    StartCoroutine(Move(Vector2.left));
                }
                else
                {
                    StartCoroutine(Move(Vector2.right));
                }
            }
            else if (inputFunction(KeyCode.A))
            {
                if (mirored_Player)
                {
                    StartCoroutine(Move(Vector2.right));
                }
                else
                {
                    StartCoroutine(Move(Vector2.left));
                }
            }
        }
    }

    private IEnumerator Move(Vector2 direction)
    {
        // Make a note of where we are and where we are going.
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

        //Maintain a list of interactables on start and end tiles
        List<Interactable> toBeInteractedWith = new List<Interactable>();

        //Consult Grid Controller to make sure this move is valid
        if (gridController.GetComponent<GridController>().playerCanMove(startPosition, endPosition))
        {
            //Check the destination tile for interactables, if there are add them to the interactables to be proccessed
            foreach (Interactable go in gridController.GetComponent<GridController>().tileContainsInteractable(endPosition))
            {
                if (go.gameObject.GetComponent<EndGoal>() != null)
                    enterGoal();
                toBeInteractedWith.Add(go);
            }

            foreach (Interactable go in gridController.GetComponent<GridController>().tileContainsInteractable(startPosition))
            {
                if (go.gameObject.GetComponent<EndGoal>() != null)
                    exitGoal();
                toBeInteractedWith.Add(go);
            }

            // Record that we're moving so we don't accept more input.
            m_IsMoving = true;

            // Smoothly move in the desired direction taking the required time.
            float elapsedTime = 0;
            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / moveDuration;
                transform.position = Vector2.Lerp(startPosition, endPosition, percent);
                yield return null;
            }

            // Make sure we end up exactly where we want.
            transform.position = endPosition;

            //Placed this here so that level ends after the players have moved fully into the end goal
            if (gridController.CheckEndConditions())
            {
                gridController.TransitionScreen();
            }

            // We're no longer moving so we can accept another move input.
            m_IsMoving = false;
        }
        //Proccess all interactions after both players have moved
        gridController.GetComponent<GridController>().processMoveInteractables(toBeInteractedWith);

        gridController.moveSound.Play();
    }

    public void setIsMirror(bool isMirror)
    {
        mirored_Player = isMirror;
    }

    public void setGridController(GridController gridController)
    {
        this.gridController= gridController;
    }

    public Tile getTileOn()
    {
        return gridController.GetTileAtPosition(transform.localPosition);
    }

    public void enterGoal()
    {
        onGoal = true;
    }

    public void exitGoal() 
    { 
        onGoal = false; 
    }

}
