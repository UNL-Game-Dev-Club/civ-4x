using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float cameraSpeed;
	public GameObject tileSelector;

	Rigidbody2D rb;

    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
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
    	}
    	else {
    		tileSelector.SetActive(false);
    	}
    }
}
