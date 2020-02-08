using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

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

	public Sprite selectorIcon;

	public Sprite goldIcon;
	public Sprite ironIcon;
	public Sprite woodIcon;
	public Sprite foodIcon;
	public Sprite stoneIcon;
	
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
        if (wallMode && Input.GetMouseButtonDown(0)) {
            Vector3Int selectedTilePos = Camera.main.GetComponent<CameraController>().selectedTilePos;

            bool built = Game.gameVar.GetCurrentPlayer().GenerateWall(selectedTilePos.x, selectedTilePos.y, true);

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

    			int costIndex = 0;
    			HideIcons(i);

    			// Display gold cost
    			if (building.goldCost > 0) {
    				SetIcon(i, costIndex, goldIcon);

    				Color tempColor = new Color(0, 0, 0, 1);
    				if (building.goldCost > Game.gameVar.GetCurrentPlayer().gold) {
    					tempColor = new Color(1, 0, 0, 1);
    				}

    				SetText(i, costIndex, building.goldCost + "", tempColor);
    				costIndex++;
    			}

    			// Display iron cost
    			if (building.ironCost > 0) {
    				SetIcon(i, costIndex, ironIcon);

    				Color tempColor = new Color(0, 0, 0, 1);
    				if (building.ironCost > Game.gameVar.GetCurrentPlayer().iron) {
    					tempColor = new Color(1, 0, 0, 1);
    				}

    				SetText(i, costIndex, building.ironCost + "", tempColor);
    				costIndex++;
    			}

    			// Display wood cost
    			if (building.woodCost > 0) {
    				SetIcon(i, costIndex, woodIcon);

    				Color tempColor = new Color(0, 0, 0, 1);
    				if (building.woodCost > Game.gameVar.GetCurrentPlayer().wood) {
    					tempColor = new Color(1, 0, 0, 1);
    				}

    				SetText(i, costIndex, building.woodCost + "", tempColor);
    				costIndex++;
    			}

    			// Display food cost
    			if (building.foodCost > 0 && costIndex < 3) {
    				SetIcon(i, costIndex, foodIcon);

    				Color tempColor = new Color(0, 0, 0, 1);
    				if (building.foodCost > Game.gameVar.GetCurrentPlayer().food) {
    					tempColor = new Color(1, 0, 0, 1);
    				}

    				SetText(i, costIndex, building.foodCost + "", tempColor);
    				costIndex++;
    			}

    			// Display stone cost
    			if (building.stoneCost > 0 && costIndex < 3) {
    				SetIcon(i, costIndex, stoneIcon);

    				Color tempColor = new Color(0, 0, 0, 1);
    				if (building.stoneCost > Game.gameVar.GetCurrentPlayer().stone) {
    					tempColor = new Color(1, 0, 0, 1);
    				}

    				SetText(i, costIndex, building.stoneCost + "", tempColor);
    				costIndex++;
    			}

    		}
    		else {
    			buildingNames[i].transform.parent.gameObject.SetActive(false);
    		}
    	}
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

        if (number + offset == 6) {
            BuildWalls();

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

    public void BuildWalls () {
        for (int i = 0; i < 8; i++) {
            Game.gameVar.movementSprites[i].gameObject.SetActive(false);
        }

        buildMode = false;
        wallMode = true;
    }
}
