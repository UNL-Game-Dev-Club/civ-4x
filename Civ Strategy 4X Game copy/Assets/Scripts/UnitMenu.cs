using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class UnitMenu : MonoBehaviour {

	public MobileUnit currentUnit;

	public Text nameText;

	// Health
	public RectTransform healthBar;
	public Text healthText;

	public Text moveText;

	public Text attackText;
	public Text rangeText;

	public GameObject[] buttons;

    // Start is called before the first frame update
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
    	
    }

    // Display the stats of the currentUnit
    public void LoadUnitData () {
    	nameText.text = currentUnit.type;

    	healthBar.localScale = new Vector3(currentUnit.healthPoints / (float)currentUnit.maxHealth, 1, 1);
    	healthText.text = currentUnit.healthPoints + " / " + currentUnit.maxHealth;

    	moveText.text = "Moves Remaining: " + currentUnit.remainingWalk + " / " + currentUnit.walkDistance;

    	attackText.text = "Attack Power: " + currentUnit.attackPower;
    	rangeText.text = "Attack Range: " + currentUnit.attackRange;

    	for (int i = 0; i < buttons.Length; i++) {
    		buttons[i].SetActive(false);
    	}

    	for (int i = 0; i < currentUnit.buttons.Length; i++) {
    		buttons[currentUnit.buttons[i]].SetActive(true);
    		buttons[currentUnit.buttons[i]].GetComponent<RectTransform>().anchoredPosition = new Vector3(10 + (65 * i), 10, 0);
    	}
    }

    // Buttons representing actions that various units can perform:

    // Opens the building menu
    public void BuildButton () {
    	BuildMenu buildMenu = Camera.main.GetComponent<CameraController>().buildMenu;

    	if (!buildMenu.gameObject.activeSelf) {
    		buildMenu.gameObject.SetActive(true);
    		buildMenu.currentUnit = currentUnit;
    		buildMenu.DisplayMenu();

    		Camera.main.GetComponent<CameraController>().moveSelector.SetActive(false);
    	}
    	else {
    		buildMenu.gameObject.SetActive(false);
    		Camera.main.GetComponent<CameraController>().SelectUnit(currentUnit.gameObject);
    	}
    }

    // Orders the selected unit to chop down a forest or wheat tile
    public void ChopButton () {
    	Vector3Int position = Game.gameVar.mainGrid.WorldToCell(currentUnit.transform.position);
    	Tile tile = (Tile)Game.gameVar.terrainMap.GetTile(new Vector3Int(position.x, position.y, 1));
    	int tileNumber = Game.gameVar.mapGenerator.GetTileNumber(tile);

    	switch (tileNumber) {
    		case 3:
    			Game.gameVar.terrainMap.SetTile(new Vector3Int(position.x, position.y, 1), null);
    			Game.gameVar.GetCurrentPlayer().wood += Random.Range(50, 100);

    			currentUnit.remainingWalk = 0;
    		break;

    		case 4:
    			Game.gameVar.terrainMap.SetTile(new Vector3Int(position.x, position.y, 1), null);
    			Game.gameVar.GetCurrentPlayer().food += Random.Range(50, 100);

    			currentUnit.remainingWalk = 0;
    		break;
    	}
    }

    public void AttackButton () {

    }
}
