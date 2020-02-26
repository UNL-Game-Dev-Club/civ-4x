using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

	public float cameraSpeed;
	public GameObject tileSelector;
    public GameObject unitSelector;
    public GameObject targetSelector;
    public GameObject moveSelector;

    public Vector3Int selectedTilePos;
    public MobileUnit selectedUnit;
    public MobileUnit targetedUnit;

    public UnitMenu unitMenu;
    public BuildMenu buildMenu;
    public TargetedUnitMenu targetedUnitMenu;

	Rigidbody rb;
    Vector3 lastMousePos;

    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        if (!Game.gameVar.GetCurrentPlayer().isHuman) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            OnLeftClick();
        }

        if (Input.GetKeyDown("return") || Input.GetKeyDown("enter")) {
            Game.NextPlayerTurn();
        }
    }

    void FixedUpdate () {
        if (!Game.gameVar.GetCurrentPlayer().isHuman) {
            return;
        }

    	rb.velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * cameraSpeed;

    	MoveTileSelector();
    }

    // Move the tile selector sprite to the location of the tile directly under the mouse position
    void MoveTileSelector () {
    	Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = Game.gameVar.mainGrid.WorldToCell(mouseWorldPos);

    	if (Game.IsInMapBounds(cellPos)) {
    		tileSelector.SetActive(true);
        	tileSelector.transform.position = Game.gameVar.groundMap.GetCellCenterWorld(cellPos);
            selectedTilePos = new Vector3Int(cellPos.x, cellPos.y, 0);

            GameTile currentTile = Game.gameVar.GetGameTileAt(selectedTilePos.x, selectedTilePos.y);

            // Adjust the layering of the tileSelector as needed
            if (currentTile.flat) {
                tileSelector.GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
            else {
                tileSelector.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }

            // Adjust the size of the tileSelector as needed
            if (currentTile.wide) {
                tileSelector.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            else {
                tileSelector.transform.localScale = new Vector3(1, 1, 1);
            }
    	}
    	else {
    		tileSelector.SetActive(false);
    	}
    }

    // Called when the left mouse button is first clicked
    void OnLeftClick () {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit;
        GameObject hitObject;

        hit = Physics2D.GetRayIntersection(ray);

        if (buildMenu.gameObject.activeSelf) {
            if (!selectedUnit.IsWithinOneTile(selectedTilePos)) {
                buildMenu.GetComponent<BuildMenu>().CloseMenu();
                UnselectUnit();
            }

            return;
        }

        // Handles selection of units
        if (hit.collider != null) {
            hitObject = hit.collider.gameObject;

            if (hitObject.GetComponent<MobileUnit>() != null) {
                SelectUnit(hitObject);

                return;
            }
            else {
                targetedUnitMenu.gameObject.SetActive(false);
            }
        }
        else {
            targetedUnitMenu.CancelAttackButton();
        }

        // Handles movement of the selected unit
        if (selectedUnit != null) {
            if (selectedUnit.IsWithinOneTile(selectedTilePos) && selectedUnit.canMove) {
                if (selectedUnit.MoveToTile(selectedTilePos.x, selectedTilePos.y)) {
                    UnselectUnit();
                }
            }
            else {
                UnselectUnit();
            }

            return;
        }
    }

    // Set the given unit as the selected unit
    public void SelectUnit (GameObject unit) {
        if (unit.GetComponent<MobileUnit>().teamNumber != Game.gameVar.currentPlayer) {
           TargetUnit(unit);
        }

        //transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -10);
        else {
            unitSelector.SetActive(true);
            unitSelector.transform.position = unit.transform.position;

            Vector3Int unitPos = Game.gameVar.mainGrid.WorldToCell(unit.transform.position);

            // Display the movement costs of the nearby walkable tiles
            if (unit.GetComponent<MobileUnit>().canMove) {
                moveSelector.SetActive(true);

                for (int i = 0; i < 8; i++) {
                    Vector2Int direction = Game.gameVar.mapGenerator.intToDirection[i];
                    int moveCost = unit.GetComponent<MobileUnit>().GetMovementCost(new Vector3Int(unitPos.x + direction.x, unitPos.y + direction.y, 1));

                    if (moveCost > 0 && moveCost < 4) {
                        Game.gameVar.movementSprites[i].gameObject.SetActive(true);
                        Game.gameVar.movementSprites[i].sprite = Game.gameVar.numberSprites[moveCost];

                        if (moveCost > unit.GetComponent<MobileUnit>().remainingWalk) {
                            Game.gameVar.movementSprites[i].color = new Color(1, 0, 0, 1);
                        }
                        else {
                            Game.gameVar.movementSprites[i].color = new Color(1, 1, 1, 1);
                        }
                    }
                    else {
                        Game.gameVar.movementSprites[i].gameObject.SetActive(false);
                    }
                }
            }

            if (selectedUnit != null) {
                selectedUnit.GetComponent<Collider2D>().enabled = true;
            }

            selectedUnit = unit.GetComponent<MobileUnit>();
            selectedUnit.GetComponent<Collider2D>().enabled = false;

            unitMenu.gameObject.SetActive(true);
            unitMenu.currentUnit = selectedUnit;
            Game.gameVar.GetCurrentPlayer().lastSelectedUnit = selectedUnit;
            unitMenu.LoadUnitData();

            if (targetedUnitMenu.targetedUnit != null) {
                targetedUnitMenu.selectedUnit = selectedUnit;
                targetedUnitMenu.LoadUnitData();
            }
        }
       
    }

    public void TargetUnit (GameObject unit) {
        targetSelector.SetActive(true);
        targetSelector.transform.position = unit.transform.position;

        targetedUnit = unit.GetComponent<MobileUnit>();

        targetedUnitMenu.gameObject.SetActive(true);
        targetedUnitMenu.targetedUnit = targetedUnit;
        targetedUnitMenu.selectedUnit = selectedUnit;
        targetedUnitMenu.LoadUnitData();
    }

    // Unselect the currently selected unit
    public void UnselectUnit () {
        selectedUnit.GetComponent<Collider2D>().enabled = true;

        selectedUnit = null;
        unitSelector.SetActive(false);
        moveSelector.SetActive(false);
        unitMenu.gameObject.SetActive(false);
        buildMenu.gameObject.SetActive(false);
        
        // targetedUnitMenu.gameObject.SetActive(false);
        targetedUnitMenu.CancelAttackButton();
    }
}