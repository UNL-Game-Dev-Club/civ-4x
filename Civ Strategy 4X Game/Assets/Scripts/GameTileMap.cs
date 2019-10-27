using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileMap : MonoBehaviour
{
    int mapTileSize;

    GameTile[,] map;

    GameObject topLeft;
    GameObject mapTile;


    // Start is called before the first frame update
    GameTileMap()
    {
        // create the tile map for the game with size specified
        map = new GameTile[mapTileSize, mapTileSize];
        Vector2 size = mapTile.GetComponent<Collider2D>().bounds.size;

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for(int j = 0; j < map.GetLength(1); j++)
            {
                float x = topLeft.transform.position.x + (i * (size.x - size.x / Mathf.Pow(3.0f, (1.0f / 3.0f))));
                bool top = i % 2 == 0;
                float y = topLeft.transform.position.y + ((j * size.y) + (top ? size.y / 2 : 0));
                var position = new Vector2(x, y);
                map[i, j] = new GameTile(position, top ? TileOrdinance.TOP : TileOrdinance.BOTTOM);
            }
        }
    }

    /*
     * Methods that return the tile the player is wanting to travel to
     * 
     */
    private GameTile getTileN(GameTile tile)
    {
        GameTile returnTile;
        
        try
        {
            returnTile = map[tile.Coordinates[1] - 1, tile.Coordinates[0]];
        }
        catch (System.IndexOutOfRangeException ex)
        {
            Debug.Log("Invalid Movement");
            returnTile = tile;
        }

        return returnTile;
    }

    private GameTile getTileNE(GameTile tile)
    {
        GameTile returnTile;

        if (tile.Ordinance == TileOrdinance.TOP)
        {
            try
            {
                returnTile = map[tile.Coordinates[1] - 1, tile.Coordinates[0] + 1];
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Invalid Movement");
                returnTile = tile;
            }
        }
        else
        {
            try
            {
                returnTile = map[tile.Coordinates[1], tile.Coordinates[0] + 1];
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Invalid Movement");
                returnTile = tile;
            }

            return returnTile;
        }

        return returnTile;
    }


    private GameTile getTileSE(GameTile tile)
    {
        GameTile returnTile;

        if (tile.Ordinance == TileOrdinance.TOP)
        {
            try
            {
                returnTile = map[tile.Coordinates[1], tile.Coordinates[0] + 1];
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Invalid Movement");
                returnTile = tile;
            }
        }
        else
        {
            try
            {
                returnTile = map[tile.Coordinates[1] + 1, tile.Coordinates[0] + 1];
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Invalid Movement");
                returnTile = tile;
            }

            return returnTile;
        }

        return returnTile;
    }

    private GameTile getTileS(GameTile tile)
    {
        GameTile returnTile;

        try
        {
            returnTile = map[tile.Coordinates[1] + 1, tile.Coordinates[0]];
        }
        catch (System.IndexOutOfRangeException ex)
        {
            Debug.Log("Invalid Movement");
            returnTile = tile;
        }

        return returnTile;
    }

    private GameTile getTileSW(GameTile tile)
    {
        GameTile returnTile;

        if (tile.Ordinance == TileOrdinance.TOP)
        {
            try
            {
                returnTile = map[tile.Coordinates[1], tile.Coordinates[0] - 1];
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Invalid Movement");
                returnTile = tile;
            }
        }
        else
        {
            try
            {
                returnTile = map[tile.Coordinates[1] + 1, tile.Coordinates[0] - 1];
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Invalid Movement");
                returnTile = tile;
            }

            return returnTile;
        }

        return returnTile;
    }


    private GameTile getTileNW(GameTile tile)
    {
        GameTile returnTile;

        if (tile.Ordinance == TileOrdinance.TOP)
        {
            try
            {
                returnTile = map[tile.Coordinates[1] - 1, tile.Coordinates[0] - 1];
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Invalid Movement");
                returnTile = tile;
            }
        }
        else
        {
            try
            {
                returnTile = map[tile.Coordinates[1], tile.Coordinates[0] - 1];
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Invalid Movement");
                returnTile = tile;
            }

            return returnTile;
        }

        return returnTile;
    }
}
