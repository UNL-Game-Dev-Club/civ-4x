using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]

public class GameTile {

	public string tileName;

	// The tile used by this GameTile
	public Tile tile;

	// The optional overlay tile which will be colored based on which player it belongs to
	public Tile colorTile;

	// The required cost of moves for a mobile unit to be able to walk onto this tile
	public int moveCost;

	// The higher the spawnRate, the more commonly this GameTile will appear in a randomly generated map
	public int spawnRate;

	public int damage;

	public Game.TileType type;

	public bool tiledBuilding;
	public bool flat;
	public bool wide;

	// Build costs for this tile. These values only matter for building tiles
	public int goldCost;
	public int ironCost;
	public int woodCost;
	public int foodCost;
	public int stoneCost;
	public int lavaCost;

	// If this tile is a building, this name is displayed in the build menu
	public string buildingName;

	// If buildingIcon isn't null, it will will replace the default sprite for this building in the build menu
	public Sprite buildingIcon;

	// These are the only tiles that this building can be built on
	public Tile[] buildingTiles;

	public GameTile[] tileSet;

    GameTile () {
    	
    }
}
