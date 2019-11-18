using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float cameraSpeed;
	public GameObject tileSelector;
    public GameObject unitSelector;
    public GameObject moveSelector;

    public Vector3Int selectedTilePos;
    public MobileUnit selectedUnit;

	Rigidbody2D rb;

    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (!Game.gameVar.GetCurrentPlayer().isHuman) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            OnLeftClick();
        }
    }

    void FixedUpdate () {
        if (!Game.gameVar.GetCurrentPlayer().isHuman) {
            return;
        }

    	// Adjust the velocity of the camera based on user input
    	rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * cameraSpeed;

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
    	}
    	else {
    		tileSelector.SetActive(false);
    	}
    }

    // Called when the left mouse button is first clicked
    void OnLeftClick () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit;
        GameObject hitObject;

        hit = Physics2D.GetRayIntersection(ray);

        // Handles selection of units
        if (hit.collider != null) {
            hitObject = hit.collider.gameObject;

            if (hitObject.GetComponent<MobileUnit>() != null) {
                SelectUnit(hitObject);

                return;
            }
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
    void SelectUnit (GameObject unit) {
        if (unit.GetComponent<MobileUnit>().teamNumber != Game.gameVar.currentPlayer) {
            return;
        }

        transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -10);

        unitSelector.SetActive(true);
        unitSelector.transform.position = unit.transform.position;

        if (unit.GetComponent<MobileUnit>().canMove) {
            moveSelector.SetActive(true);
        }

        selectedUnit = unit.GetComponent<MobileUnit>();
    }

    // Unselect the currently selected unit
    void UnselectUnit () {
        selectedUnit = null;
        unitSelector.SetActive(false);
        moveSelector.SetActive(false);
    }
}














