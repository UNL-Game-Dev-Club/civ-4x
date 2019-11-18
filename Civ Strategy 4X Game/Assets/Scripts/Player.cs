using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Values set in the inspector
	public bool isHuman;

	// Values set during gameplay
	public int playerNumber;
	public Color playerColor;

	public Vector3 cameraPosition;


	// Objects defined during gameplay
	public List<MobileUnit> ownedUnits = new List<MobileUnit>();

    // Start is called before the first frame update
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        
    }

    // Generates a new unit by creating a copy of "unitObject" and assigning it to this player's team
    public void GenerateUnit (GameObject unitObject, int posX, int posY) {
    	if (unitObject.GetComponent<MobileUnit>() == null) {
    		return;
    	}

    	GameObject newObject = (GameObject)Instantiate(unitObject, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    	MobileUnit newUnit = newObject.GetComponent<MobileUnit>();

    	newObject.transform.position = Game.gameVar.groundMap.GetCellCenterWorld(new Vector3Int(posX, posY, 0));
    	newObject.GetComponent<SpriteRenderer>().color = playerColor;

    	newUnit.newPosition = newObject.transform.position;
    	newUnit.teamNumber = playerNumber;

    	ownedUnits.Add(newUnit);
    }
}
