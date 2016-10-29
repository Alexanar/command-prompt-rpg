using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Map
{
    //map grid
    private int[,] mapGrid;
    public int playerPositionRow { get; set; }
    public int playerPositionCol { get; set; }
    public int endPositionRow { get; set; }
    public int endPositionCol { get; set; }
    //levels
    public int currentLevel { get; set; }

    //create creation (0 = unusable, 1 = usable, 3 = start, 4 = end)
    public void CreateGrid(int level, string message)
    {
        switch (level)
        {
            case 1:
                mapGrid = new int[,]
                {
                        { 0,0,0,0,4,0 },
                        { 0,0,0,1,1,1,},
                        { 0,0,1,1,0,1 },
                        { 1,1,1,1,0,0,},
                        { 0,3,0,0,0,0 },
                        { 0,1,0,0,0,0,}
                };
                SetSpecialPositions();
                DisplayMap(mapGrid);
                currentLevel = 1;
                break;
            case 2:
                mapGrid = new int[,]
                {
                        { 0,3,1,0,0,0 },
                        { 1,1,1,0,0,0,},
                        { 1,0,0,1,0,0 },
                        { 1,1,1,1,0,0,},
                        { 0,1,0,0,0,0 },
                        { 0,1,1,4,0,0,}
                };
                SetSpecialPositions();
                DisplayMap(mapGrid);
                currentLevel = 2;
                break;
            default:
                mapGrid = new int[,]
                {
                        { 0,0,0,0,4,0 },
                        { 0,0,0,1,1,1,},
                        { 0,0,1,1,0,1 },
                        { 1,1,1,1,0,0,},
                        { 0,3,0,0,0,0 },
                        { 0,1,0,0,0,0,}
                };
                SetSpecialPositions();
                DisplayMap(mapGrid);
                break;
        }
        Console.WriteLine(message);
    }

    //get player position and end of level position from map grid
    public void SetSpecialPositions()
    {
        for (int i_row = 0; i_row < mapGrid.GetLength(0); i_row++)
        {
            for (int i_col = 0; i_col < mapGrid.GetLength(1); i_col++)
            {
                //player position get
                if (mapGrid[i_row, i_col] == 3)
                {
                    playerPositionRow = i_row;
                    playerPositionCol = i_col;
                }
                //end position get
                if (mapGrid[i_row, i_col] == 4)
                {
                    endPositionRow = i_row;
                    endPositionCol = i_col;
                }

            }
        }
    }

    //when player reaches end of level tile
    public void EndOfLevel()
    {
        if (CompareTiles(playerPositionRow, endPositionRow, playerPositionCol, endPositionCol))
        {
            Console.WriteLine("You can see the exit ahead");
            Console.WriteLine("Type ENTER to enter");
        }
    }

    //carried all special tile functions
    public void SpecialTiles()
    {
        EndOfLevel();
    }


    //check if able to move to tile
    public bool CanMove(int row, int col)
    {
        if (playerPositionRow + row >= 0
            && playerPositionRow + row < mapGrid.GetLength(0)
            && playerPositionCol + col >= 0
            && playerPositionCol + col < mapGrid.GetLength(1)
            && mapGrid[playerPositionRow + row, playerPositionCol + col] != 0)
        {
            return true;
        }
        return false;
    }

    //compare two tile positions on the grid to see if player is on the tile
    public bool CompareTiles(int playerR, int tileR, int playerC, int tileC)
    {
        if (playerR == tileR && playerC == tileC)
        {
            return true;
        }
        return false;
    }

    //displays map with player position
    public void DisplayMap(int[,] mapArray)
    {
        int player_row = mapArray.GetLength(0);
        int player_col = mapArray.GetLength(1);
        string[,] playerMap = new string[player_row, player_col];

        //copy map data to player map array
        for (int i_row = 0; i_row < player_row; i_row++)
        {
            for (int i_col = 0; i_col < player_col; i_col++)
            {
                playerMap[i_row, i_col] = mapArray[i_row, i_col].ToString();
            }
        }
        //show player position on map as 'P'
        playerMap[playerPositionRow, playerPositionCol] = "P";
        Console.Clear();

        //print out map data in matrix format
        for (int i_row = 0; i_row < player_row; i_row++)
        {
            for (int i_col = 0; i_col < player_col; i_col++)
            {
                Console.Write(string.Format("{0} ", playerMap[i_row, i_col]));
            }
            Console.WriteLine();
        }
    }


    public int PlayerPositionRowChange(int number)
    {
        playerPositionRow = playerPositionRow + number;
        return playerPositionRow;
    }
    public int PlayerPositionColChange(int number)
    {
        playerPositionCol = playerPositionCol + number;
        return playerPositionCol;
    }
    public int[,] MapGridData()
    {
        return mapGrid;
    }




}

