using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobileUnit : MonoBehaviour {

	public string type;
	public int healthPoints;
	public int walkDistance;

	public int teamNumber;
	public bool canMove;
	public int remainingWalk;

	public Vector3 newPosition;

    // Start is called before the first frame update
    void Start () {
        
    }

    void Update () {
        if (transform.position != newPosition) {
        	transform.position = Vector3.MoveTowards(transform.position, newPosition, 10 * Time.deltaTime);
        }
    }

    // Move this unit to the tile at (posX, posY) if it is able to
    public bool MoveToTile (int posX, int posY) {
    	if (!canMove) {
    		return false;
    	}

    	Tile terrainTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(posX, posY, 1));

    	if (Game.gameVar.mapGenerator.IsWaterTile(posX, posY) || terrainTile == Game.gameVar.mapGenerator.landTiles[1]) {
    		return false;
    	}

    	newPosition = Game.gameVar.groundMap.GetCellCenterWorld(new Vector3Int(posX, posY, 0));

    	remainingWalk -= 1;

    	if (remainingWalk < 1) {
    		canMove = false;
    	}

    	return true;
    }

    // Check if the given tile position is within one tile of this unit
    public bool IsWithinOneTile (Vector3Int position) {
    	Vector3Int cellPos = Game.gameVar.mainGrid.WorldToCell(transform.position);

    	for (int x = cellPos.x - 1; x <= cellPos.x + 1; x++) {
    		for (int y = cellPos.y - 1; y <= cellPos.y + 1; y++) {
    			if ( position == new Vector3Int(x, y, 0) && (cellPos.x != x || cellPos.y != y) ) {
    				return true;
    			}
    		}
    	}

    	return false;
    }
}
