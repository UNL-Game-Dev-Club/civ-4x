using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TargetedUnitMenu : MonoBehaviour {

    public MobileUnit targetedUnit;
    public MobileUnit selectedUnit;

    public GameObject targetSelector;

    public Text nameText;

    // Health
    public RectTransform healthBar;
    public Text healthText;

    public Text attackText;
    public Text rangeText;

    public GameObject[] buttons;
    public int[] buttonCosts;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        LoadUnitData();
    }

    // Display the stats of the currentUnit
    public void LoadUnitData () {
        if (targetedUnit == null) {
            return;
        }

        nameText.text = "Enemy " + targetedUnit.type;

        healthBar.localScale = new Vector3(targetedUnit.healthPoints / (float)targetedUnit.maxHealth, 1, 1);
        healthText.text = targetedUnit.healthPoints + " / " + targetedUnit.maxHealth;


        attackText.text = "Attack Power: " + targetedUnit.attackPower;
        rangeText.text = "Attack Range: " + targetedUnit.attackRange;

        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].SetActive(false);
        }

        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].SetActive(true);
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(10 + (65 * i), 10, 0);

            if (selectedUnit == null) {
                continue;
            }

            if (buttonCosts[i] > selectedUnit.remainingWalk) {
                buttons[i].GetComponent<Button>().interactable = false;
            }
            else {
                buttons[i].GetComponent<Button>().interactable = true;
            }
        }

        if (selectedUnit == null) {
            buttons[0].GetComponent<Button>().interactable = false;
        }
        else if (!selectedUnit.IsWithinAttackRange(targetedUnit)) {
            buttons[0].GetComponent<Button>().interactable = false;
        }
    }

    public void ConfirmAttackButton () {
        if (!selectedUnit.IsWithinAttackRange(targetedUnit) || selectedUnit.remainingWalk < 1) {
            return;
        }

        GameObject newVFX = (GameObject)Instantiate(selectedUnit.attackFX, selectedUnit.transform.position, new Quaternion(0, 0, 0, 0));
        newVFX.GetComponent<Projectile>().Launch(targetedUnit, selectedUnit.attackPower);

        selectedUnit.remainingWalk = 0;
        selectedUnit.attackMode = false;

        Game.gameVar.cameraController.UnselectUnit();
        CancelAttackButton();
    }

    public void CancelAttackButton () {
        selectedUnit = null;
        targetedUnit = null;

        targetSelector.SetActive(false);
        gameObject.SetActive(false);
        
        return;
    }
}