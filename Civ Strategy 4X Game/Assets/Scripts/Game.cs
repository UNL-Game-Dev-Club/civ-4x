using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game {
    
    public static GameVar gameVar;

    // An enum for specifying which type of tile a GameTile is
    public enum TileType {
    	Ground,
    	Terrain,
    	Water,
    	Building
    }

    // Check if the x and y coordinates of a Vector3Int are within the boundaries of the map
    public static bool IsInMapBounds (Vector3Int position) {
    	return (position.x >= 0 && position.x < gameVar.mapSize.x && position.y >= 0 && position.y < gameVar.mapSize.y);
    }

    // Initialize a new game
    public static void StartNewGame (int mapSizeX, int mapSizeY) {
    	gameVar.currentPlayer = 0;
    	gameVar.mapGenerator.GenerateMap(mapSizeX, mapSizeY, 5);

    	// Create each human player and add them to the list of players
    	for (int i = 0; i < gameVar.numberOfPlayers; i++) {
    		GameObject newObject = (GameObject)Object.Instantiate(gameVar.humanPlayer, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    		Player newPlayer = newObject.GetComponent<Player>();

    		newPlayer.startingPosition = new Vector3Int(Random.Range(5, mapSizeX - 5), Random.Range(5, mapSizeY - 5), 0);

    		newPlayer.cameraPosition = gameVar.groundMap.GetCellCenterWorld(newPlayer.startingPosition);
    		newPlayer.cameraPosition = new Vector3(newPlayer.cameraPosition.x, newPlayer.cameraPosition.y, -10);

    		gameVar.players.Add(newPlayer);
    	}

    	// Create each computer player and add them to the list of players
    	for (int i = 0; i < gameVar.numberOfComputers; i++) {
    		GameObject newObject = (GameObject)Object.Instantiate(gameVar.computerPlayer, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    		Player newPlayer = newObject.GetComponent<Player>();

    		newPlayer.startingPosition = new Vector3Int(Random.Range(5, mapSizeX - 5), Random.Range(5, mapSizeY - 5), 0);

    		gameVar.players.Add(newPlayer);
    	}

    	// Finish initialization of the players
    	for (int i = 0; i < gameVar.players.Count; i++) {
    		Player newPlayer = gameVar.players[i];

    		newPlayer.playerNumber = i;

    		newPlayer.playerColor = gameVar.playerColors[i];

    		newPlayer.GenerateUnit(gameVar.units[1], newPlayer.startingPosition.x, newPlayer.startingPosition.y);
            newPlayer.GenerateUnit(gameVar.units[2], newPlayer.startingPosition.x+1, newPlayer.startingPosition.y);

            newPlayer.lastSelectedUnit = newPlayer.ownedUnits[0];


            newPlayer.GenerateBuilding(gameVar.buildingTiles[0], newPlayer.startingPosition.x, newPlayer.startingPosition.y, false);

    		// Starting resources for each player
    		newPlayer.gold = 1000;
    		newPlayer.iron = 0;
    		newPlayer.wood = 0;
    		newPlayer.food = 100;
    		newPlayer.stone = 0;
    	}

    	StartPlayerTurn(0);
    }

    // End the previous player's turn and start the next player's turn
    public static void NextPlayerTurn () {
    	EndPlayerTurn(gameVar.currentPlayer);

        gameVar.cameraController.unitMenu.gameObject.SetActive(false);
        gameVar.cameraController.buildMenu.gameObject.SetActive(false);
        gameVar.cameraController.targetedUnitMenu.gameObject.SetActive(false);
        gameVar.cameraController.targetSelector.SetActive(false);
        gameVar.cameraController.unitSelector.SetActive(false);
        gameVar.cameraController.tileSelector.SetActive(false);
        gameVar.cameraController.moveSelector.SetActive(false);

        gameVar.currentPlayer++;

    	if (gameVar.currentPlayer >= gameVar.numberOfPlayers + gameVar.numberOfComputers) {
    		gameVar.currentPlayer = 0;
    	}

        //gameVar.controlMenu.SetActive(false);

    	StartPlayerTurn(gameVar.currentPlayer);
    }

    // Begin the next turn for the player with the number "playerNumber"
    public static void StartPlayerTurn (int playerNumber) {
    	Player currentPlayer = gameVar.players[playerNumber];
    	gameVar.currentPlayer = playerNumber;

    	foreach (MobileUnit unit in currentPlayer.ownedUnits) {
    		unit.canMove = true;
    		unit.remainingWalk = unit.walkDistance;

            unit.OnTurnStart();
    	}

    	if (currentPlayer.isHuman) {
    		Camera.main.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    		
            if (currentPlayer.lastSelectedUnit == null) {
                Camera.main.transform.position = currentPlayer.cameraPosition;
            }
            else {
                Camera.main.transform.position = new Vector3(currentPlayer.lastSelectedUnit.transform.position.x, currentPlayer.lastSelectedUnit.transform.position.y, -10);
            }
    	}
    	else {

    	}

    	// Give the player the amount of resources they obtain per turn
    	currentPlayer.gold += currentPlayer.goldProfit;
    	currentPlayer.iron += currentPlayer.ironProfit;
    	currentPlayer.wood += currentPlayer.woodProfit;
    	currentPlayer.food += currentPlayer.foodProfit;
    	currentPlayer.stone += currentPlayer.stoneProfit;
        currentPlayer.lava += currentPlayer.lavaProfit;
    }

    // End the turn of the player with the number "playerNumber"
    public static void EndPlayerTurn (int playerNumber) {
    	Player currentPlayer = gameVar.players[playerNumber];

    	currentPlayer.cameraPosition = Camera.main.transform.position;
    }
}
