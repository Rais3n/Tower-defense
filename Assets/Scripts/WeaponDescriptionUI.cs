using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDescriptionUI : MonoBehaviour
{
    [SerializeField] Color enoughMoney;
    [SerializeField] Color notEnoughMoney;

    private Image image;
    private TextMeshProUGUI desriptionText;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        desriptionText = GetComponentInChildren<TextMeshProUGUI>();
        image.gameObject.SetActive(false);
    }
    private void Start()
    {
        DragDropWeapon.OnMouseExit += DragDropWeapon_OnMouseExit;
        DragDropWeapon.OnMouseEnter += DragDropWeapon_OnMouseEnter;
    }

    private void DragDropWeapon_OnMouseEnter(object sender, DragDropWeapon.OnMouseEnterEventArgs e)
    {
        image.gameObject.SetActive(true);
        int currentAmountOfMoney = GameManager.Instance.GetAmountOfMoney();
        int remainingMoney;
        remainingMoney = currentAmountOfMoney - e.cost;
        desriptionText.text = "COST: " + e.cost.ToString() + "\n" + "\n" + "REMAINING FUNDS: " + "\n" + remainingMoney.ToString();
        if(remainingMoney < 0)
        {
            image.color= notEnoughMoney;
        }
        else
        {
            image.color= enoughMoney;
        }
    }


    private void DragDropWeapon_OnMouseExit(object sender, System.EventArgs e)
    {
        image.gameObject.SetActive(false);
    }
}
