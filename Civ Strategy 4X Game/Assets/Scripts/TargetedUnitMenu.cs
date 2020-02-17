using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TargetedUnitMenu : MonoBehaviour
{

    public MobileUnit targetedUnit;
    public MobileUnit selectedUnit;

    public Text nameText;

    // Health
    public RectTransform healthBar;
    public Text healthText;

    public Text attackText;
    public Text rangeText;

    public GameObject[] buttons;
    public int[] buttonCosts;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LoadUnitData();
    }

    // Display the stats of the currentUnit
    public void LoadUnitData()
    {
        selectedUnit = Game.gameVar.GetCurrentPlayer().lastSelectedUnit;
        nameText.text = targetedUnit.type;

        healthBar.localScale = new Vector3(targetedUnit.healthPoints / (float)targetedUnit.maxHealth, 1, 1);
        healthText.text = targetedUnit.healthPoints + " / " + targetedUnit.maxHealth;


        attackText.text = "Attack Power: " + targetedUnit.attackPower;
        rangeText.text = "Attack Range: " + targetedUnit.attackRange;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }

        for (int i = 0; i < buttons.Length; i++)
        {
     
            buttons[i].SetActive(true);
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(10 + (65 * i), 10, 0);
            if (buttonCosts[i] > selectedUnit.remainingWalk)
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                buttons[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void ConfirmAttackButton()
    {
        if (!selectedUnit.IsWithinAttackRange(targetedUnit) || selectedUnit.remainingWalk < 1) 
        {
            return;
        }
        targetedUnit.TakeDamage(selectedUnit.attackPower);
        selectedUnit.remainingWalk = 0;
        selectedUnit.attackMode = false;
    }

    public void CancelAttackButton()
    {
        return;
    }
}