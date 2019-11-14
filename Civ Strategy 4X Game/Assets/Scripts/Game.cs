using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game {
    
    public static GameVar gameVar;

    // Check if the x and y coordinates of a Vector3Int are within the boundaries of the map
    public static bool IsInMapBounds (Vector3Int position) {
    	return (position.x >= 0 && position.x < gameVar.mapSize.x && position.y >= 0 && position.y < gameVar.mapSize.y);
    }
}
