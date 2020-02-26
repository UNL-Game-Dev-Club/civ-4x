using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobileUnit : MonoBehaviour {

    // Type of unit
	public string type;
	
    // Health Variables
    public int healthPoints;
    public int maxHealth;

    // Variables for other stats
    public int attackPower;
    public int attackRange;
    public bool attackMode = false;
    public int walkDistance;

	// Action buttons that this unit can use
	//   - 0 = Build
	//   - 1 = Chop
	//   - 2 = Attack
    //   - 3 = Heal

	public int[] buttons;
    public GameObject attackFX;

    // Values set during gameplay
	public int teamNumber;
	public bool canMove;
	public int remainingWalk;
    public SpriteRenderer colorSprite;

	public Vector3 newPosition;

    // Start is called before the first frame update
    void Start () {
        
    }

    void Update () {
        if (transform.position != newPosition) {
        	transform.position = Vector3.MoveTowards(transform.position, newPosition, 10 * Time.deltaTime);
        }
    }

    // When a player starts their next turn, this function is called for each of that player's units
    public void OnTurnStart () {
        Vector3Int cellPos = Game.gameVar.mainGrid.WorldToCell(transform.position);

        TakeTileDamage(cellPos.x, cellPos.y);
    }

    // Move this unit to the tile at (posX, posY) if it is able to
    public bool MoveToTile (int posX, int posY) {
    	if (!canMove) {
    		return false;
    	}

        if (GetMovementCost(new Vector3Int(posX, posY, 0)) > remainingWalk) {
            return false;
        }

    	Tile terrainTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(posX, posY, 1));

    	if (!IsWalkable(new Vector3Int(posX, posY, 0))) {
    		return false;
    	}

    	newPosition = Game.gameVar.groundMap.GetCellCenterWorld(new Vector3Int(posX, posY, 0));

    	remainingWalk -= GetMovementCost(new Vector3Int(posX, posY, 0));

        TakeTileDamage(posX, posY);

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

    // Check if the given tile is within the attack range of the unit
    public bool IsWithinAttackRange(MobileUnit unit)
    {
        Vector3Int cellPos = Game.gameVar.mainGrid.WorldToCell(transform.position);
        Vector3Int enemyCellPos = Game.gameVar.mainGrid.WorldToCell(unit.transform.position);
        enemyCellPos = new Vector3Int(enemyCellPos.x, enemyCellPos.y, 0);

        for (int x = cellPos.x - this.attackRange; x <= cellPos.x + this.attackRange; x++)
        {
            for (int y = cellPos.y - this.attackRange; y <= cellPos.y + this.attackRange; y++)
            {
                if (enemyCellPos == new Vector3Int(x, y, 0) && (cellPos.x != x || cellPos.y != y))
                {
                    return true;
                }
            }
        }

        return false;
    }


    // Check if the given tile is walkable
    private bool IsWalkable (Vector3Int position) {
        if (Game.gameVar.mapGenerator.IsWaterTile(position.x, position.y)) {
            return false;
        }

        Tile currentTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(position.x, position.y, 1));
        int tileNumber = Game.gameVar.mapGenerator.GetTileNumber(currentTile);

        if (Game.gameVar.mapGenerator.gameTiles[tileNumber].moveCost == 0) {
            return false;
        }

        return true;
    }

    // Returns the movement cost of the tile at the given position
    public int GetMovementCost (Vector3Int position) {
        GameTile foundTile = Game.gameVar.GetGameTileAt(position.x, position.y);

        if (foundTile != null) {
            return foundTile.moveCost;
        }

        return 0;
    }

    // Deals the given amount of damage to this unit
    public void TakeDamage (int amount) {
        healthPoints -= amount;

        if (healthPoints <= 0) {
            Destroy(gameObject);
        }

        transform.position += new Vector3(0, 0.75f, 0);
    }

    // Checks if the tile at the given coordinates is dangerous, and deals damage to this unit accordingly
    void TakeTileDamage (int xPos, int yPos) {
        Tile tempTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(xPos, yPos, 1));

        int damageAmount = Game.gameVar.GetTileDamage(tempTile);

        if (damageAmount > 0) {
            TakeDamage(damageAmount);
        }
    }
}