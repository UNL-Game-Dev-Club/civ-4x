using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

	public Tilemap groundMap;
	public Tilemap terrainMap;

	public Tile[] landTiles;
    public Tile[] waterTiles;

    Dictionary<Vector2Int, int> directionToInt = new Dictionary<Vector2Int, int>();
    Dictionary<int, Vector2Int> intToDirection = new Dictionary<int, Vector2Int>();

    // Start is called before the first frame update
    void Start () {
        InitializeDictionaries();

        //GenerateMap(50, 50, 5);
    }

    // Update is called once per frame
    void Update () {
        
    }

    // Randomly generate a new map of the given dimensions sizeX and sizeY
    //  - maxRiverCount is the maximum number of rivers that will be generated
    public void GenerateMap (int sizeX, int sizeY, int maxRiverCount) {
        InitializeDictionaries();

        Game.gameVar.mapSize = new Vector2(sizeX, sizeY);
        int riverCount = 0;

    	for (int x = -1; x <= sizeX; x++) {
        	for (int y = -1; y <= sizeY; y++) {
                if (x >= 0 && x < sizeX && y >= 0 && y < sizeY) {
                    if (!IsWaterTile(x, y)) {
                        // put the ground layer on the bottom layer (z = 0)
                        groundMap.SetTile(new Vector3Int(x, y, 0), landTiles[0]);

                        int nextTile = Random.Range(0, landTiles.Length);

                        // put the terrain on top(z = 1)
                        if (nextTile > 1) {
                            terrainMap.SetTile(new Vector3Int(x, y, 1), landTiles[nextTile]);
                        }
                    }
                }
                else {
                    terrainMap.SetTile(new Vector3Int(x, y, 0), landTiles[1]);

                    if (Random.Range(0, 50) == 17 && riverCount < maxRiverCount) {
                        GenerateRiver(x, y);
                        riverCount++;
                    }
                }
        	}
        }
    }

    // Generate a new river across the map starting at a given position
    void GenerateRiver (int posX, int posY) {
        Vector2Int position = new Vector2Int(posX, posY);
        Vector2Int direction = new Vector2Int(0, 0);

        int nextTurn = -1;

        //Set up the intitial direction vector
        if (posX == -1) {
            direction += new Vector2Int(1, 0);
        }
        if (posY == -1) {
            direction += new Vector2Int(0, 1);
        }
        if (posX == Game.gameVar.mapSize.x) {
            direction += new Vector2Int(-1, 0);
        }
        if (posY == Game.gameVar.mapSize.y) {
            direction += new Vector2Int(0, -1);
        }

        position += direction;

        while ( Game.IsInMapBounds(new Vector3Int(position.x, position.y, 0)) && !IsWaterTile(position.x, position.y) ) {
            groundMap.SetTile(new Vector3Int(position.x, position.y, 0), waterTiles[0]);
            terrainMap.SetTile(new Vector3Int(position.x, position.y, 1), null);

            position += direction;

            //Random chance of the river either continuing straight or turning slightly each time a new part of it is generated
            if (Random.Range(0, 2) == 1) {
                int tempInt = directionToInt[direction] + nextTurn;

                if (Random.Range(0, 10) == 7) {
                    tempInt += nextTurn;
                }

                if (tempInt < 0) {
                    tempInt += 8;
                }
                else if (tempInt > 7) {
                    tempInt -= 8;
                }

                direction = intToDirection[tempInt];

                nextTurn *= -1;
            }
        }
    }

    // Check if the tile located at the given x and y position is a water tile
    public bool IsWaterTile (int posX, int posY) {
        for (int i = 0; i < waterTiles.Length; i++) {
            Tile tempTile = (Tile)groundMap.GetTile(new Vector3Int(posX, posY, 0));

            if (tempTile == waterTiles[i]) {
                return true;
            }
        }

        return false;
    }

    // Initialize keys and values for dictionaries
    void InitializeDictionaries () {
        if (directionToInt.Count > 0) {
            return;
        }

        //For converting a direction vector into an integer
        directionToInt.Add(new Vector2Int(0, 1), 0);
        directionToInt.Add(new Vector2Int(1, 1), 1);
        directionToInt.Add(new Vector2Int(1, 0), 2);
        directionToInt.Add(new Vector2Int(1, -1), 3);
        directionToInt.Add(new Vector2Int(0, -1), 4);
        directionToInt.Add(new Vector2Int(-1, -1), 5);
        directionToInt.Add(new Vector2Int(-1, 0), 6);
        directionToInt.Add(new Vector2Int(-1, 1), 7);

        //For converting an integer into the corresponding direction vector
        intToDirection.Add(0, new Vector2Int(0, 1));
        intToDirection.Add(1, new Vector2Int(1, 1));
        intToDirection.Add(2, new Vector2Int(1, 0));
        intToDirection.Add(3, new Vector2Int(1, -1));
        intToDirection.Add(4, new Vector2Int(0, -1));
        intToDirection.Add(5, new Vector2Int(-1, -1));
        intToDirection.Add(6, new Vector2Int(-1, 0));
        intToDirection.Add(7, new Vector2Int(-1, 1));
    }
}