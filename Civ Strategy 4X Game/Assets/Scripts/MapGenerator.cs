﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

	public Tilemap groundMap;
	public Tilemap terrainMap;

    public GameObject[] worldBorders;

    public GameTile[] gameTiles;

    // public int[] movementCosts;

    public Dictionary<Vector2Int, int> directionToInt = new Dictionary<Vector2Int, int>();
    public Dictionary<int, Vector2Int> intToDirection = new Dictionary<int, Vector2Int>();
    public Dictionary<string, int> neighborToInt = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start () {
        InitializeDictionaries();
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
                        groundMap.SetTile(new Vector3Int(x, y, 0), gameTiles[0].tile);

                        int randomRange = 0;
                        for (int i = 2; i < gameTiles.Length; i++) {
                        	randomRange += (int)((float)gameTiles[i].spawnRate * 2f);
                        }

                        int randomNumber = Random.Range(0, randomRange);
                        int nextTile = 0;

                        for (int i = 2; i < gameTiles.Length; i++) {
                        	if (randomNumber < gameTiles[i].spawnRate && nextTile == 0) {
                        		nextTile = i;
                        	}
                        	else {
                        		randomNumber -= gameTiles[i].spawnRate;
                        	}
                        }

                        if (nextTile > 1) {
                            SetGameTile(new Vector3Int(x, y, 1), gameTiles[nextTile]);
                        }
                    }
                }
                else {
                    SetGameTile(new Vector3Int(x, y, 1), gameTiles[1]);

                    if (Random.Range(0, 50) == 17 && riverCount < maxRiverCount) {
                        GenerateRiver(x, y);
                        riverCount++;
                    }
                }
        	}
        }

        SetWorldBorders(sizeX, sizeY);
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
        	SetGameTile(new Vector3Int(position.x, position.y, 1), gameTiles[2]);
        	groundMap.SetTile(new Vector3Int(position.x, position.y, 1), null);
            
            position += direction;

            // Random chance of the river either continuing straight or turning slightly each time a new part of it is generated
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

    // Sets up the outer world boundaries which prevent the camera from straying too far from the map
    void SetWorldBorders (int sizeX, int sizeY) {
        // Upper left world border
        worldBorders[0].transform.position = groundMap.GetCellCenterWorld(new Vector3Int((int)(sizeX / 2.0), sizeY + 1, 0));
        worldBorders[0].transform.localScale = new Vector3(1, sizeX, 1);

        // Upper right world border
        worldBorders[1].transform.position = groundMap.GetCellCenterWorld(new Vector3Int(sizeX + 1, (int)(sizeY / 2.0), 0));
        worldBorders[1].transform.localScale = new Vector3(1, sizeY, 1);

        // Lower left world border
        worldBorders[2].transform.position = groundMap.GetCellCenterWorld(new Vector3Int(-2, (int)(sizeY / 2.0), 0));
        worldBorders[2].transform.localScale = new Vector3(1, sizeY, 1);

        // Lower right world border
        worldBorders[3].transform.position = groundMap.GetCellCenterWorld(new Vector3Int((int)(sizeX / 2.0), -2, 0));
        worldBorders[3].transform.localScale = new Vector3(1, sizeX, 1);
    }

    // Check if the tile located at the given x and y position is a water tile
    public bool IsWaterTile (int posX, int posY) {
        Tile tempTile = (Tile)terrainMap.GetTile(new Vector3Int(posX, posY, 1));
        GameTile gameTile = gameTiles[GetTileNumber(tempTile)];

        return (gameTile.type == Game.TileType.Water);
    }

    // Get the number corresponding to a particular tile
    public int GetTileNumber (Tile tile) {
        for (int i = 0; i < gameTiles.Length; i++) {
            if (gameTiles[i].tile == tile) {
                return i;
            }
        }

        return 0;
    }

    // Places the tile of the given GameTile on the correct tilemap and at the given coordinates
    public void SetGameTile (Vector3Int pos, GameTile gameTile) {
    	switch (gameTile.type) {
    		case Game.TileType.Ground:
    			groundMap.SetTile(new Vector3Int(pos.x, pos.y, 0), gameTile.tile);
    		break;

    		case Game.TileType.Terrain:
    			terrainMap.SetTile(new Vector3Int(pos.x, pos.y, 1), gameTile.tile);
    		break;

    		case Game.TileType.Water:
    			terrainMap.SetTile(new Vector3Int(pos.x, pos.y, 1), gameTile.tile);
    		break;

    		case Game.TileType.Building:
    			terrainMap.SetTile(new Vector3Int(pos.x, pos.y, 1), gameTile.tile);
    		break;
    	}
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

        // For converting a tile neighbor code into an int
        neighborToInt.Add("0000", 4);
        neighborToInt.Add("1000", 10);
        neighborToInt.Add("0100", 9);
        neighborToInt.Add("0010", 12);
        neighborToInt.Add("0001", 11);

        neighborToInt.Add("0101", 0);
        neighborToInt.Add("0110", 2);
        neighborToInt.Add("1010", 8);
        neighborToInt.Add("1001", 6);
        neighborToInt.Add("1100", 13);
        neighborToInt.Add("0011", 14);

        neighborToInt.Add("1011", 7);
        neighborToInt.Add("0111", 1);
        neighborToInt.Add("1110", 5);
        neighborToInt.Add("1101", 3);

        neighborToInt.Add("1111", 4);
    }
}