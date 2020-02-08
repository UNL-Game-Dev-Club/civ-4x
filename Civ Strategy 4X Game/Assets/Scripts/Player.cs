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

	public int goldProfit;
	public int ironProfit;
	public int woodProfit;
	public int foodProfit;
	public int stoneProfit;

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
    	newObject.GetComponent<SpriteRenderer>().color = playerColor;

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
    	}

    	return true;
    }

    public bool GenerateWall (int xPos, int yPos, bool costMatters) {
        Tile previousTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(xPos, yPos, 1));

        if (previousTile != null) {
            return false;
        }

        if (costMatters && stone < 100) {
            return false;
        }

        stone -= 100;

        GameTile wallTile = GetWallTile(xPos, yPos);

        Game.gameVar.terrainMap.SetTile(new Vector3Int(xPos, yPos, 1), wallTile.tile);

        RefreshWall(xPos, yPos + 1);
        RefreshWall(xPos, yPos - 1);
        RefreshWall(xPos - 1, yPos);
        RefreshWall(xPos + 1, yPos);

        return true;
    }

    void RefreshWall (int xPos, int yPos) {
        if (GetWallCode(xPos, yPos) == "0") {
            return;
        }

        GameTile wallTile = GetWallTile(xPos, yPos);

        Game.gameVar.terrainMap.SetTile(new Vector3Int(xPos, yPos, 1), wallTile.tile);
    }

    GameTile GetWallTile (int xPos, int yPos) {
        string neighborCode = "";
        neighborCode += GetWallCode(xPos, yPos + 1);
        neighborCode += GetWallCode(xPos, yPos - 1);
        neighborCode += GetWallCode(xPos - 1, yPos);
        neighborCode += GetWallCode(xPos + 1, yPos);

        int wallIndex = Game.gameVar.mapGenerator.neighborToInt[neighborCode];

        return Game.gameVar.wallTiles[wallIndex];
    }

    string GetWallCode (int xPos, int yPos) {
        Tile checkTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(xPos, yPos, 1));

        for (int i = 0; i < Game.gameVar.wallTiles.Length; i++) {
            if (Game.gameVar.wallTiles[i].tile == checkTile) {
                return "1";
            }
        }

        return "0";
    }
}
