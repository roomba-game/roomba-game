using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static public Player P;
    public bool playerControl = true;
    float playerDirection = -1; //-1 = No Direction, 0 = N, 1 = W, 2 = S, 3 = E
    int[] playerPos = new int[] { 0, 0 };
    int[] toCheck = new int[] { 0, 0 };
    Vector3 target = new Vector3(0, 0, 0);
    bool moved = false;
    public (int x, int z) exitDestination;
    List<Vector3> TargetDestinations = new List<Vector3>();
    int count = 0;
    

    void Awake()
    {
        P = this;
        this.GetComponent<CapsuleCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControl)
        {
            return;
        }

        //Set direction
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            playerDirection = 0; //North
            playerControl = false;
            TargetDestinations.Clear();
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            playerDirection = 1; //West
            playerControl = false;
            TargetDestinations.Clear();
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            playerDirection = 2; //South
            playerControl = false;
            TargetDestinations.Clear();
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            playerDirection = 3; //East
            playerControl = false;
            TargetDestinations.Clear();
            moved = true;
        }
        else
        {
            playerControl = true;
        }
    }

    //Update loop that looks along the path the player is traveling
    void FixedUpdate()
    {
        if (playerDirection == 0)
        {
            toCheck[0] = playerPos[0] - 1;
            toCheck[1] = playerPos[1];
        }
        else if (playerDirection == 1)
        {
            toCheck[1] = playerPos[1] - 1;
            toCheck[0] = playerPos[0];
        }
        else if (playerDirection == 2)
        {
            toCheck[0] = playerPos[0] + 1;
            toCheck[1] = playerPos[1];
        }
        else if (playerDirection == 3)
        {
            toCheck[1] = playerPos[1] + 1;
            toCheck[0] = playerPos[0];
        }
        else return;

        checkGridSpace(playerDirection, toCheck);
    }

    //checks the next tile and determines what to do from there
    void checkGridSpace(float dir, int[] spaceToCheck)
    {
        MapCell cell = Main.S.grid[spaceToCheck[0], spaceToCheck[1]];

        if (cell.gameObject == null)
        {
            setPlayerPosition(dir);

            if (moved)
            {
                GameData.GD.setMoves();

                moved = false;
            }
        }
        else
        {
            if (!cell.gridObject.isSolid && cell.gridObject.tag != "Portal")
            {
                setPlayerPosition(dir);

                if (moved)
                {
                    GameData.GD.setMoves();

                    moved = false;
                }
            }
            else if (!cell.gridObject.isSolid && cell.gridObject.tag == "Portal")
            {
                if (moved)
                {
                    GameData.GD.setMoves();

                    moved = false;
                }

                setPlayerPosition(dir);

                cell.gridObject.OnCollision();

                setTeleportDestination(cell.gridObject.transform.position, spaceToCheck[0], spaceToCheck[1]);
            }
            else if (cell.gridObject.isSolid && !playerControl)
            {
                //Once it finds a wall set the target
                setTarget(dir, cell.gameObject.transform.position);

                //start movement
                StartCoroutine(movePlayer());
            }
        }
    }

    void setTeleportDestination(Vector3 entryPortal, int entryPortalZ, int entryPortalX)
    {
        (int x, int z) difference;

        TargetDestinations.Add(entryPortal);

        difference.x = entryPortalX - exitDestination.x;
        difference.z = entryPortalZ - exitDestination.z;

        TargetDestinations.Add(convertGridToGameTile(entryPortal, difference));

        playerPos[0] = exitDestination.z;
        playerPos[1] = exitDestination.x;
    }

    Vector3 convertGridToGameTile(Vector3 entryPortal, (int x, int z) difference)
    {
        Vector3 newTarget = entryPortal;

        //Determine if left or right (x)
        if (difference.x < 0)
        {
            //teleporting right
            difference.x = Mathf.Abs(difference.x);

            newTarget.x = entryPortal.x + difference.x;
        }
        else if (difference.x > 0)
        {
            //teleporting left
            newTarget.x = entryPortal.x - difference.x;
        }

        //Determine if up or down (z)
        if (difference.z < 0)
        {
            //teleporting down
            difference.z = Mathf.Abs(difference.z);

            newTarget.z = entryPortal.z - difference.z;
        }
        else if (difference.z > 0)
        {
            //teleporting up
            newTarget.z = entryPortal.z + difference.z;
        }

        return newTarget;
    }

    //sets the target to the new position
    void setTarget(float dir, Vector3 wallHit)
    {
        target = wallHit;

        if (!playerControl)
        {
            if (dir == 0)
            {
                target.z -= 1;
            }
            else if (dir == 1)
            {
                target.x += 1;
            }
            else if (dir == 2)
            {
                target.z += 1;
            }
            else if (dir == 3)
            {
                target.x -= 1;
            }

            TargetDestinations.Add(target);
        }
    }

    //set up the player for a new level
    public void setPlayerStart(int row, int col)
    {
        playerPos[0] = row;
        playerPos[1] = col;

        target = transform.position;

        resetValues();
    }

    //Reset all player values at the start of a level
    void resetValues()
    {
        playerControl = true;
        playerDirection = -1; //-1 = No Direction, 0 = N, 1 = W, 2 = S, 3 = E
        toCheck = new int[] { 0, 0 };

        exitDestination = (0, 0);
        TargetDestinations = new List<Vector3>();
        count = 0;
}

    //Sets the player position in the 2d array
    void setPlayerPosition(float dir)
    {
        if (dir == 0)
        {
            playerPos[0]--;
        }
        else if (dir == 1)
        {
            playerPos[1]--;
        }
        else if (dir == 2)
        {
            playerPos[0]++;
        }
        else if (dir == 3)
        {
            playerPos[1]++;
        }
    }

    //Smoothly moves the player until it gets to the new target position
    IEnumerator movePlayer()
    {
        if (count % 2 == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetDestinations[count], GameData.GD.getPlayerSpeed() * Time.deltaTime);
        }
        else
        {
            SFX.S.Play(3);
            transform.position = TargetDestinations[count];
        }

        if (transform.position == TargetDestinations[count])
        {
            count++;
        }

        yield return new WaitUntil(() => transform.position == TargetDestinations[TargetDestinations.Count - 1]);

        count = 0;
        playerControl = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Plant")
        {
            other.GetComponent<Obstacle>().OnCollision();
        }
        else if (other.tag == "Spill")
        {
            other.GetComponent<Mess>().OnCollision();
        }
        else if (other.tag == "Portal")
        {
            SFX.S.Play(3);
        }
    }

    public void LockMovement() { playerDirection = -1; }
}
