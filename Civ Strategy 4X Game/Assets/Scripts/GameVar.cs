using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameVar : MonoBehaviour {

    // Values set in the inspector
    public int numberOfPlayers;
    public int numberOfComputers;

    // Objects referenced in the inspector
	public Grid mainGrid;
	public Tilemap groundMap;
	public Tilemap terrainMap;
    
    public GameObject humanPlayer;
    public GameObject computerPlayer;

    public MapGenerator mapGenerator;

    public Color[] playerColors;
    public GameObject[] units;

    // Values set during gameplay
    public Vector2 mapSize;
    public int currentPlayer;

    // Objects defined during gameplay
    public List<Player> players = new List<Player>();

    // Start is called before the first frame update
    void Start () {
        Game.gameVar = GetComponent<GameVar>();
        mapGenerator = GetComponent<MapGenerator>();

        Game.StartNewGame(50, 50);
    }

    // Update is called once per frame
    void Update () {
        
    }

    // Returns the player whose turn it currently is
    public Player GetCurrentPlayer () {
        return players[currentPlayer];
    }
}
