using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]

public class GameTile {

	// The tile used by this GameTile
	public Tile tile;

	// The required cost of moves for a mobile unit to be able to walk onto this tile
	public int moveCost;

	// The higher the spawnRate, the more commonly this GameTile will appear in a randomly generated map
	public int spawnRate;

	public Game.TileType type;

    GameTile () {
    	
    }
}
