using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    public GameObject wall;
    private GameObject Walls;

    public float wallLength = 2.0f;
    public int xSize = 5;
    public int ySize = 5;

    private Vector3 initialPos;

    private Cell[] cells;

    private int currCell = 0;
    private int totalCells;

    private int visitedCells;
    private bool startedBuilding;
    private int currNeighbor;

    private List<int> lastCells;
    private int backUp = 0;

    private int wallToBreak = 0;

    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north;
        public GameObject south;
        public GameObject east;
        public GameObject west;

    }

    // Use this for initialization
    void Start () {
        createWalls();
	}

    void createWalls()
    {
        //creates a parent for wall objects
        Walls = new GameObject();
        Walls.name = "Maze";

        //initial wall position
        initialPos = new Vector3((-xSize/2) + (wallLength/2), 0.0f, (-ySize/2) + (wallLength/2));
        Vector3 currPos = initialPos;

        //builds x walls
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                currPos = new Vector3(initialPos.x + ((j * wallLength) - wallLength / 2), 0.0f, initialPos.z + ((i * wallLength) - wallLength / 2));
                GameObject tempwal = Instantiate(wall, currPos, Quaternion.identity) as GameObject;
                tempwal.transform.parent = Walls.transform;
            }
        }

        //builds y walls
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                currPos = new Vector3(initialPos.x + (j * wallLength), 0.0f, initialPos.z + ((i * wallLength) - wallLength));
                GameObject tempwall = Instantiate(wall, currPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
                tempwall.transform.parent = Walls.transform;
            }
        }
        createCells();
    }

    void createCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();

        totalCells = xSize * ySize;
        //get current number of walls
        int wallCount = Walls.transform.childCount;
        GameObject[] allWalls = new GameObject[wallCount];
        cells = new Cell[xSize * ySize];

        int eastWest = 0;
        int child = 0;
        int term = 0;

        //get all walls
        for (int i = 0; i < wallCount; i++)
        {
            allWalls[i] = Walls.transform.GetChild(i).gameObject;
        }

        //assign walls to cells
        for (int j = 0; j < cells.Length; j++)
        {
            if (term == xSize)
            {
                eastWest++;
                term = 0;
            }

            cells[j] = new Cell();
            cells[j].east = allWalls[eastWest];
            cells[j].south = allWalls[child + (xSize + 1) * ySize];

            eastWest++;

            term++;
            child++;
            cells[j].west = allWalls[eastWest];
            cells[j].north = allWalls[(child + (xSize + 1) * ySize) + (xSize - 1)];
        }
        createMaze();
    }

    void createMaze()
    {
        while (visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                giveNeighbor();
                if (cells[currNeighbor].visited == false && cells[currCell].visited == true)
                {
                    breakWall();
                    cells[currNeighbor].visited = true;
                    visitedCells++;
                    lastCells.Add(currCell);
                    currCell = currNeighbor;
                    if (lastCells.Count > 0)
                    {
                        backUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currCell = Random.Range(0, totalCells);
                cells[currCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }
        }
    }

    void breakWall()
    {
        //north = 1     south = 2       east = 3        west = 4
        switch (wallToBreak)
        {
            case 1: Destroy(cells[currCell].north); break;
            case 2: Destroy(cells[currCell].south); break;
            case 3: Destroy(cells[currCell].east); break;
            case 4: Destroy(cells[currCell].west); break;
        }
    }

    void giveNeighbor()
    {
        int length = 0;
        int[] neighbors = new int[4];
        int[] connectingWall = new int[4];

        int check = 0;
        check = ((currCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;

        //north
        if (currCell + xSize < totalCells)
        {
            if (cells[currCell + xSize].visited == false)
            {
                neighbors[length] = currCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }

        //south 
        if (currCell - xSize >= 0)
        {
            if (cells[currCell - xSize].visited == false)
            {
                neighbors[length] = currCell - xSize;
                connectingWall[length] = 2;
                length++;
            }
        }

        //east
        if (currCell - 1 >= 0 && currCell != check)
        {
            if (cells[currCell - 1].visited == false)
            {
                neighbors[length] = currCell - 1;
                connectingWall[length] = 3;
                length++;
            }
        }

        //west
        if ((currCell + 1) <  totalCells && (currCell + 1) !=check)
        {
            if (cells[currCell + 1].visited == false)
            {
                neighbors[length] = currCell + 1;
                connectingWall[length] = 4;
                length++;
            }
        }

        if (length != 0)
        {
            int curr = Random.Range(0, length);
            currNeighbor = neighbors[curr];
            wallToBreak = connectingWall[curr];
        }
        else
        {
            if (backUp > 0)
            {
                currCell = lastCells[backUp];
                backUp--;
            }
        }
    }
}
