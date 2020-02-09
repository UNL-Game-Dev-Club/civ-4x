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

    public CameraController cameraController;
    
	public GameTile[] buildingTiles;
    // public GameTile[] wallTiles;
    // public GameTile[] lavaTiles;

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
    public Text[] goldText;
    public Text[] ironText;
    public Text[] woodText;
    public Text[] foodText;
    public Text[] stoneText;
    public Text[] lavaText;

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
        goldText[0].text = "" + GetCurrentPlayer().gold;
        ironText[0].text = "" + GetCurrentPlayer().iron;
        woodText[0].text = "" + GetCurrentPlayer().wood;
     	foodText[0].text = "" + GetCurrentPlayer().food;
     	stoneText[0].text = "" + GetCurrentPlayer().stone;
        lavaText[0].text = "" + GetCurrentPlayer().lava;

        goldText[1].text = "(+" + GetCurrentPlayer().goldProfit + ")";
        ironText[1].text = "(+" + GetCurrentPlayer().ironProfit + ")";
        woodText[1].text = "(+" + GetCurrentPlayer().woodProfit + ")";
        foodText[1].text = "(+" + GetCurrentPlayer().foodProfit + ")";
        stoneText[1].text = "(+" + GetCurrentPlayer().stoneProfit + ")";
        lavaText[1].text = "(+" + GetCurrentPlayer().lavaProfit + ")";

        if (Input.GetKeyDown("space")) {
            controlMenu.SetActive(false);
        }
    }

    // Returns the player whose turn it currently is
    public Player GetCurrentPlayer () {
        return players[currentPlayer];
    }

    public int GetTileDamage (Tile tile) {
        GameTile foundTile = GetGameTile(tile);

        if (foundTile != null) {
            return foundTile.damage;
        }

        return 0;
    }

    // Returns the GameTile that corresponds to the given Tile
    public GameTile GetGameTile (Tile tile) {
        if (tile == null) {
            return null;
        }

        GameTile foundTile = null;

        // Check for building tiles
        for (int i = 0; i < buildingTiles.Length; i++) {
            foreach (GameTile directionalTile in buildingTiles[i].tileSet) {
                if (tile == directionalTile.tile) {
                    foundTile = buildingTiles[i];
                }
            }

            if (tile == buildingTiles[i].tile) {
                foundTile = buildingTiles[i];
            }
        }

        // Check for natural tiles
        for (int i = 0; i < mapGenerator.gameTiles.Length; i++) {
            if (tile == mapGenerator.gameTiles[i].tile) {
                foundTile = mapGenerator.gameTiles[i];
            }
        }

        return foundTile;
    }

    // Returns the highest-level GameTile that can be found at the given coordinates
    // (buildings and terrain will be returned first, then ground tiles)
    public GameTile GetGameTileAt (int xPos, int yPos) {
        Tile currentTile = (Tile)terrainMap.GetTile(new Vector3Int(xPos, yPos, 1));
        GameTile foundTile = GetGameTile(currentTile);

        if (foundTile != null) {
            return foundTile;
        }

        currentTile = (Tile)groundMap.GetTile(new Vector3Int(xPos, yPos, 0));
        foundTile = GetGameTile(currentTile);

        return foundTile;
    }
}