using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour {

    private const float STACK_MOVING_SPEED=5.0F;
    private const float BOUNDS_SIZE = 3.5F;
    private const float ERROR_MARGIN = 0.1F;
    private GameObject[] theStack;

    private int stackIndex;

    private int scoreCount = 0;

    private int combo = 0;

    private float tileTransition = 0.0f;

    private float tileSpeed = 2.5f;

    private bool isMovingOnX = true;

    private float secondaryPosition;

    private Vector2 stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);

    private Vector3 desiredPosition;

    private Vector3 lastTilePosition;

	// Use this for initialization
	private void Start () {

        theStack = new GameObject[transform.childCount];
        for(int i=0; i<transform.childCount; i++)
        {
            theStack[i] = transform.GetChild (i).gameObject;
        }
        stackIndex = transform.childCount -1;
		
	}
	
	// Update is called once per frame
	private void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            if (placeTile())
            {
                spawnTile();
                scoreCount++;

            }
            else
            {
                endGame();
            }
            
        }

        moveTile();

        transform.position = Vector3.Lerp(transform.position, desiredPosition,STACK_MOVING_SPEED*Time.deltaTime );
	}

    private void moveTile()
    {
        tileTransition += Time.deltaTime * tileSpeed;

        if (isMovingOnX)
        {
            theStack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTransition) * BOUNDS_SIZE, scoreCount, secondaryPosition);
        }
        else
        {
            theStack[stackIndex].transform.localPosition = new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTransition) * BOUNDS_SIZE);
        }

    }

    private void spawnTile()
    {
        lastTilePosition = theStack[stackIndex].transform.localPosition;

        stackIndex--;
        if (stackIndex <= 0)
        {
            stackIndex = transform.childCount - 1;
        }
        desiredPosition = (Vector3.down) * scoreCount;
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
    }

    private bool placeTile()
    {
        Transform t = theStack[stackIndex].transform;

        if (isMovingOnX)
        {
            float deltaX = lastTilePosition.x - t.position.x;
            if (Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                combo = 0;
                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0)
                {
                    return false;
                }
                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.localScale=new Vector3(stackBounds.x, 1, stackBounds.y);
            }
        }

        secondaryPosition = (isMovingOnX)
            ? t.localPosition.x
            : t.localPosition.z;

        isMovingOnX = !isMovingOnX;

        
        return true;
    }

    private void endGame()
    {

    }
}
