using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMenu : MonoBehaviour {

	public MobileUnit currentUnit;

	public Text nameText;

	// Health
	public RectTransform healthBar;
	public Text healthText;

	public Text moveText;

	public Text attackText;
	public Text rangeText;

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
    }
}
