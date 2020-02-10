using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {

	// Values set in the inspector
	public bool isHuman;

	// Values set during gameplay
	public int playerNumber;
	public Color playerColor;
	public Vector3 cameraPosition;
	public Vector3Int startingPosition;
    public MobileUnit lastSelectedUnit;

	// Resource Variables
	public int gold;
	public int iron;
	public int wood;
	public int food;
	public int stone;
    public int lava;

	public int goldProfit;
	public int ironProfit;
	public int woodProfit;
	public int foodProfit;
	public int stoneProfit;
    public int lavaProfit;

	// Objects defined during gameplay
	public List<MobileUnit> ownedUnits = new List<MobileUnit>();

    // Start is called before the first frame update
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        
    }

    // Generates a new unit by creating a copy of "unitObject" and assigning it to this player's team
    public void GenerateUnit (GameObject unitObject, int posX, int posY) {
    	if (unitObject.GetComponent<MobileUnit>() == null) {
    		return;
    	}

    	GameObject newObject = (GameObject)Instantiate(unitObject, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    	MobileUnit newUnit = newObject.GetComponent<MobileUnit>();

    	newObject.transform.position = Game.gameVar.groundMap.GetCellCenterWorld(new Vector3Int(posX, posY, 0));
        if (newUnit.colorSprite == null)
        {
            newObject.GetComponent<SpriteRenderer>().color = playerColor;
        }
        else
        {
            newUnit.colorSprite.color = playerColor;
        }

    	newUnit.newPosition = newObject.transform.position;
    	newUnit.teamNumber = playerNumber;

    	ownedUnits.Add(newUnit);
    }

    // Generate a building belonging to this player. return true if successful, false if unsuccessful
    public bool GenerateBuilding (GameTile building, int posX, int posY, bool costMatters) {
    	Tile previousTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(posX, posY, 1));

    	// If cost matters, check to make sure the player has enough resources to afford the cost of the building
    	if (costMatters) {
    		if (building.goldCost > gold) {
    			return false;
    		}
    		if (building.ironCost > iron) {
    			return false;
    		}
    		if (building.woodCost > wood) {
    			return false;
    		}
    		if (building.foodCost > food) {
    			return false;
    		}
    		if (building.stoneCost > stone) {
    			return false;
    		}
            if (building.lavaCost > lava) {
                return false;
            }

    		bool canBuild = false;

    		for (int i = 0; i < building.buildingTiles.Length; i++) {
    			if (previousTile == building.buildingTiles[i]) {
    				canBuild = true;
    			}
    		}

    		if (!canBuild) {
    			return false;
    		}
    	}

    	Game.gameVar.terrainMap.SetTile(new Vector3Int(posX, posY, 1), building.tile);
    	
    	if (building.colorTile != null) {
    		Game.gameVar.colorMap.SetTile(new Vector3Int(posX, posY, 1), building.colorTile);

    		Game.gameVar.colorMap.SetTileFlags(new Vector3Int(posX, posY, 1), TileFlags.None);
    		Game.gameVar.colorMap.SetColor(new Vector3Int(posX, posY, 1), playerColor);
    	}

    	gold -= building.goldCost;
    	iron -= building.ironCost;
    	wood -= building.woodCost;
    	food -= building.foodCost;
    	stone -= building.stoneCost;
        lava -= building.lavaCost;

    	// Adjust per-turn profits
    	switch (building.buildingName) {
    		case "Farm":
    			foodProfit += 50;
    		break;

    		case "Mine":
    			int tileNumber = Game.gameVar.mapGenerator.GetTileNumber(previousTile);

    			if (tileNumber == 6) {
    				ironProfit += 50;
    			}
    			else if (tileNumber == 7) {
    				goldProfit += 25;
    			}
    		break;

    		case "Stone Mine":
    			stoneProfit += 80;
    		break;

    		case "Lumber Yard":
    			woodProfit += 100;
    		break;

            case "Lava Mine":
                lavaProfit += 10;
            break;
    	}

    	return true;
    }

    // Generate a wall or other interconnecting structure at the given coordinates
    // Returns true or false depending on if the structure is successfully built or not
    public bool GenerateWall (int xPos, int yPos, bool costMatters, GameTile[] tiles) {
        Tile previousTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(xPos, yPos, 1));

        if (previousTile != null) {
            return false;
        }

        GameTile wallTile = GetDirectionalTile(xPos, yPos, tiles);

        if (costMatters) {
            if (wallTile.goldCost > gold) {
    			return false;
    		}
    		if (wallTile.ironCost > iron) {
    			return false;
    		}
    		if (wallTile.woodCost > wood) {
    			return false;
    		}
    		if (wallTile.foodCost > food) {
    			return false;
    		}
    		if (wallTile.stoneCost > stone) {
    			return false;
    		}
            if (wallTile.lavaCost > lava) {
                return false;
            }
        }

        gold -= wallTile.goldCost;
    	iron -= wallTile.ironCost;
    	wood -= wallTile.woodCost;
    	food -= wallTile.foodCost;
    	stone -= wallTile.stoneCost;
        lava -= wallTile.lavaCost;

        Game.gameVar.terrainMap.SetTile(new Vector3Int(xPos, yPos, 1), wallTile.tile);

        RefreshTile(xPos, yPos + 1, tiles);
        RefreshTile(xPos, yPos - 1, tiles);
        RefreshTile(xPos - 1, yPos, tiles);
        RefreshTile(xPos + 1, yPos, tiles);

        return true;
    }

    // Refreshes an interconnecting structure
    void RefreshTile (int xPos, int yPos, GameTile[] tiles) {
        if (GetNeighborCode(xPos, yPos, tiles) == "0") {
            return;
        }

        GameTile wallTile = GetDirectionalTile(xPos, yPos, tiles);

        Game.gameVar.terrainMap.SetTile(new Vector3Int(xPos, yPos, 1), wallTile.tile);
    }

    // Returns the tile that should be placed based on the surrounding tiles
    GameTile GetDirectionalTile (int xPos, int yPos, GameTile[] tiles) {
        string neighborCode = "";
        neighborCode += GetNeighborCode(xPos, yPos + 1, tiles);
        neighborCode += GetNeighborCode(xPos, yPos - 1, tiles);
        neighborCode += GetNeighborCode(xPos - 1, yPos, tiles);
        neighborCode += GetNeighborCode(xPos + 1, yPos, tiles);

        int tileIndex = Game.gameVar.mapGenerator.neighborToInt[neighborCode];

        return tiles[tileIndex];
    }

    // Returns a "0" or "1" depending on whether or not a tile in the given list of tiles was found at the given coordinates
    string GetNeighborCode (int xPos, int yPos, GameTile[] tiles) {
        Tile checkTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(xPos, yPos, 1));

        for (int i = 0; i < tiles.Length; i++) {
            if (tiles[i].tile == checkTile) {
                return "1";
            }
        }

        return "0";
    }
}
