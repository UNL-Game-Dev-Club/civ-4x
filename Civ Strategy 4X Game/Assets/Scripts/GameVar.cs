using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameVar : MonoBehaviour {

    // Values set in the inspector
    public int numberOfPlayers;
    public int numberOfComputers;

    // Objects referenced in the inspector
	public Grid mainGrid;
	public Tilemap groundMap;
	public Tilemap terrainMap;
	public Tilemap colorMap;
    
	public GameTile[] buildingTiles;
    public GameTile[] wallTiles;

    public GameObject humanPlayer;
    public GameObject computerPlayer;

    public GameObject controlMenu;

    public MapGenerator mapGenerator;

    public Color[] playerColors;
    public GameObject[] units;
    public SpriteRenderer[] movementSprites;
    public Sprite[] numberSprites;

    // Main UI Text
    public Text playerText;
    public Text goldText;
    public Text ironText;
    public Text woodText;
    public Text foodText;
    public Text stoneText;

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
        playerText.text = "Player " + (currentPlayer + 1);
        goldText.text = "" + GetCurrentPlayer().gold + " (+" + GetCurrentPlayer().goldProfit + ")";
        ironText.text = "" + GetCurrentPlayer().iron + " (+" + GetCurrentPlayer().ironProfit + ")";
        woodText.text = "" + GetCurrentPlayer().wood + " (+" + GetCurrentPlayer().woodProfit + ")";
     	foodText.text = "" + GetCurrentPlayer().food + " (+" + GetCurrentPlayer().foodProfit + ")";
     	stoneText.text = "" + GetCurrentPlayer().stone + " (+" + GetCurrentPlayer().stoneProfit + ")";

        if (Input.GetKeyDown("space")) {
            controlMenu.SetActive(false);
        }
    }

    // Returns the player whose turn it currently is
    public Player GetCurrentPlayer () {
        return players[currentPlayer];
    }
}
