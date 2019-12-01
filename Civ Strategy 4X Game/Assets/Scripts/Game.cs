using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game {
    
    public static GameVar gameVar;

    // An enum for specifying which type of tile a GameTile is
    public enum TileType {
    	Ground,
    	Terrain,
    	Water
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

    		newPlayer.GenerateUnit(gameVar.units[0], newPlayer.startingPosition.x, newPlayer.startingPosition.y);

    		// Starting resources
    		newPlayer.gold = 1000;
    		newPlayer.iron = 0;
    		newPlayer.wood = 0;
    		newPlayer.food = 0;
    	}

    	StartPlayerTurn(0);
    }

    // End the previous player's turn and start the next player's turn
    public static void NextPlayerTurn () {
    	EndPlayerTurn(gameVar.currentPlayer);

    	gameVar.currentPlayer++;

    	if (gameVar.currentPlayer >= gameVar.numberOfPlayers + gameVar.numberOfComputers) {
    		gameVar.currentPlayer = 0;
    	}

    	StartPlayerTurn(gameVar.currentPlayer);
    }

    // Begin the next turn for the player with the number "playerNumber"
    public static void StartPlayerTurn (int playerNumber) {
    	Player currentPlayer = gameVar.players[playerNumber];
    	gameVar.currentPlayer = playerNumber;

    	foreach (MobileUnit unit in currentPlayer.ownedUnits) {
    		unit.canMove = true;
    		unit.remainingWalk = unit.walkDistance;
    	}

    	if (currentPlayer.isHuman) {
    		Camera.main.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    		Camera.main.transform.position = currentPlayer.cameraPosition;
    	}
    	else {

    	}
    }

    // End the turn of the player with the number "playerNumber"
    public static void EndPlayerTurn (int playerNumber) {
    	Player currentPlayer = gameVar.players[playerNumber];

    	currentPlayer.cameraPosition = Camera.main.transform.position;
    }
}














