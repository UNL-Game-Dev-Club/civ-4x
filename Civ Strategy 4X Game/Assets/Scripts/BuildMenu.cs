using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuildMenu : MonoBehaviour {

	// Text variables:
	public Text[] buildingNames;
	
	public Text[] text1;
	public Text[] text2;
	public Text[] text3;

	// Icon variables:
	public RawImage[] images;
	
	public RawImage[] icon1;
	public RawImage[] icon2;
	public RawImage[] icon3;

	public Button[] buttons = new Button[3];

	public Sprite selectorIcon;

	public Sprite goldIcon;
	public Sprite ironIcon;
	public Sprite woodIcon;
	public Sprite foodIcon;
	public Sprite stoneIcon;
    public Sprite lavaIcon;
	
	public MobileUnit currentUnit;

	int offset = 0;
	int buildingCount;

	public bool buildMode;
    public bool wallMode;
	GameTile currentBuilding;

    // Start is called before the first frame update
    void Start () {
    	buildMode = false;
        buildingCount = Game.gameVar.buildingTiles.Length;
        DisplayMenu();
    }

    // Update is called once per frame
    void Update () {
    	if (EventSystem.current.IsPointerOverGameObject()) {
    		return;
    	}

        if (wallMode && Input.GetMouseButtonDown(0)) {
            GenerateWall();

            return;
        }

        if (buildMode && Input.GetMouseButtonDown(0)) {
        	Vector3Int selectedTilePos = Camera.main.GetComponent<CameraController>().selectedTilePos;

            if (currentUnit.IsWithinOneTile(selectedTilePos)) {     
    			bool built = Game.gameVar.GetCurrentPlayer().GenerateBuilding(currentBuilding, selectedTilePos.x, selectedTilePos.y, true);

    			if (built) {
    				currentUnit.remainingWalk = 0;
    				Camera.main.GetComponent<CameraController>().UnselectUnit();
    			}
            }

            return;
        }
    }

    // Display information about which buildings can be built on the build menu
    public void DisplayMenu () {
    	for (int i = 0; i < 3; i++) {
    		int index = i + offset;
    		
    		if (index < buildingCount) {
    			buildingNames[i].transform.parent.gameObject.SetActive(true);

				GameTile building = Game.gameVar.buildingTiles[index];

    			buildingNames[i].text = building.buildingName;
    			
    			if (building.buildingIcon != null) {
    				images[i].texture = building.buildingIcon.texture;
    			}
    			else {
    				images[i].texture = building.tile.sprite.texture;
    			}

    			buttons[i].interactable = true;

    			int costIndex = 0;
    			HideIcons(i);

   				if (DisplayCost(building.goldCost, Game.gameVar.GetCurrentPlayer().gold, i, costIndex, goldIcon)) {
   					costIndex++;
   				}

   				if (DisplayCost(building.ironCost, Game.gameVar.GetCurrentPlayer().iron, i, costIndex, ironIcon)) {
   					costIndex++;
   				}

   				if (DisplayCost(building.woodCost, Game.gameVar.GetCurrentPlayer().wood, i, costIndex, woodIcon)) {
   					costIndex++;
   				}

   				if (DisplayCost(building.foodCost, Game.gameVar.GetCurrentPlayer().food, i, costIndex, foodIcon)) {
   					costIndex++;
   				}

   				if (DisplayCost(building.stoneCost, Game.gameVar.GetCurrentPlayer().stone, i, costIndex, stoneIcon)) {
   					costIndex++;
   				}

   				if (DisplayCost(building.lavaCost, Game.gameVar.GetCurrentPlayer().lava, i, costIndex, lavaIcon)) {
   					costIndex++;
   				}
    		}
    		else {
    			buildingNames[i].transform.parent.gameObject.SetActive(false);
    		}
    	}
    }

    // Display the cost of a particular resource to build a particular building in the build menu
    bool DisplayCost (int cost, int balance, int index, int costIndex, Sprite icon) {
    	if (cost > 0 && costIndex < 3) {
            SetIcon(index, costIndex, icon);

            Color tempColor = new Color(0, 0, 0, 1);
            if (cost > balance) {
                tempColor = new Color(1, 0, 0, 1);
                buttons[index].interactable = false;
            }

            SetText(index, costIndex, cost + "", tempColor);
            // costIndex++;
            return true;
        }

        return false;
    }

    // Change the offset of the build menu by the specified amount
    public void MoveOffset (int amount) {
    	offset += amount;
    	
    	if (buildingCount <= 3) {
    		offset = 0;
    	}
    	else if (offset > buildingCount - 3) {
    		offset = buildingCount - 3;
    	}
    	else if (offset < 0) {
    		offset = 0;
    	}

    	DisplayMenu();
    }

    // Set the sprite of the icon specified by "number" and "iconNumber" to the given sprite "icon"
    public void SetIcon (int number, int iconNumber, Sprite icon) {
    	switch (number) {
    		case 0:
    			icon1[iconNumber].gameObject.SetActive(true);
    			icon1[iconNumber].texture = icon.texture;
    		break;

    		case 1:
    			icon2[iconNumber].gameObject.SetActive(true);
    			icon2[iconNumber].texture = icon.texture;
    		break;

    		case 2:
    			icon3[iconNumber].gameObject.SetActive(true);
    			icon3[iconNumber].texture = icon.texture;
    		break;
    	}
    }

    // Set the text variable specified by "number" and "textNumber" to the given string "text", and color it the specified "color"
    public void SetText (int number, int textNumber, string text, Color color) {
    	switch (number) {
    		case 0:
    			text1[textNumber].gameObject.SetActive(true);
    			text1[textNumber].text = text;

    			text1[textNumber].color = color;
    		break;

    		case 1:
    			text2[textNumber].gameObject.SetActive(true);
    			text2[textNumber].text = text;

    			text2[textNumber].color = color;
    		break;

    		case 2:
    			text3[textNumber].gameObject.SetActive(true);
    			text3[textNumber].text = text;

    			text3[textNumber].color = color;
    		break;
    	}
    }

    // Hide all resource icons in the build menu
    public void HideIcons (int number) {
    	switch (number) {
    		case 0:
    			icon1[0].gameObject.SetActive(false);
    			icon1[1].gameObject.SetActive(false);
    			icon1[2].gameObject.SetActive(false);

    			text1[0].gameObject.SetActive(false);
    			text1[1].gameObject.SetActive(false);
    			text1[2].gameObject.SetActive(false);
    		break;

    		case 1:
    			icon2[0].gameObject.SetActive(false);
    			icon2[1].gameObject.SetActive(false);
    			icon2[2].gameObject.SetActive(false);

    			text2[0].gameObject.SetActive(false);
    			text2[1].gameObject.SetActive(false);
    			text2[2].gameObject.SetActive(false);
    		break;

    		case 2:
    			icon3[0].gameObject.SetActive(false);
    			icon3[1].gameObject.SetActive(false);
    			icon3[2].gameObject.SetActive(false);

    			text3[0].gameObject.SetActive(false);
    			text3[1].gameObject.SetActive(false);
    			text3[2].gameObject.SetActive(false);
    		break;
    	}
    }

    // Display which nearby tiles the building can be built on.
    // Enter "build mode", which means the building will be built on the next valid tile that is clicked
    public void Build (int number) {
    	if (currentUnit.remainingWalk < 1) {
    		return;
    	}

        if (Game.gameVar.buildingTiles[number + offset].tiledBuilding) {
            BuildWalls(Game.gameVar.buildingTiles[number + offset]);

            return;
        }

    	currentBuilding = Game.gameVar.buildingTiles[number + offset];
    	Camera.main.GetComponent<CameraController>().moveSelector.SetActive(true);

    	for (int i = 0; i < 8; i++) {
    		Vector3Int position = Game.gameVar.mainGrid.WorldToCell(currentUnit.transform.position);
    		position += new Vector3Int(Game.gameVar.mapGenerator.intToDirection[i].x, Game.gameVar.mapGenerator.intToDirection[i].y, 0);

    		Tile previousTile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(position.x, position.y, 1));
    		bool canBuild = false;

    		for (int j = 0; j < currentBuilding.buildingTiles.Length; j++) {
    			if (previousTile == currentBuilding.buildingTiles[j]) {
    				canBuild = true;
    			}
    		}

    		if (canBuild) {
    			Game.gameVar.movementSprites[i].gameObject.SetActive(true);
    			Game.gameVar.movementSprites[i].color = new Color(1, 1, 1, 1);
    			Game.gameVar.movementSprites[i].sprite = selectorIcon;
    		}
    		else {
    			Game.gameVar.movementSprites[i].gameObject.SetActive(false);
    		}
    	}

    	buildMode = true;
        wallMode = false;
    }

    /**
     * Enter "wall building mode", which is similar to "build mode" except it handles the automatic connecting of
     * tiles that can connect to other tiles. It also allows the player to build as many of the building in a row
     * as they have enough resources for.
     *
     * Use this for structures that connect to themselves, such as walls, roads, and lava moats
     */
    public void BuildWalls (GameTile building) {
        for (int i = 0; i < 8; i++) {
            Game.gameVar.movementSprites[i].gameObject.SetActive(false);
        }

        currentBuilding = building;

        buildMode = false;
        wallMode = true;
    }

    // Generate a new chunk of wall belonging to the current player
    void GenerateWall () {
        Vector3Int selectedTilePos = Camera.main.GetComponent<CameraController>().selectedTilePos;
        GameTile[] tiles = currentBuilding.tileSet;

        bool built = Game.gameVar.GetCurrentPlayer().GenerateWall(selectedTilePos.x, selectedTilePos.y, true, tiles);
    }

    public void CloseMenu () {
        foreach (SpriteRenderer sr in Game.gameVar.movementSprites) {
            sr.gameObject.SetActive(false);
        }

        buildMode = false;
        gameObject.SetActive(false);
    }
}