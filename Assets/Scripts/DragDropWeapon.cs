using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragDropWeapon : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform prefab;
    [SerializeField] private int cost;

    public static event EventHandler<OnMouseEnterEventArgs> OnMouseEnter;
    public class OnMouseEnterEventArgs : EventArgs
    {
        public int cost;
    }

    public static event EventHandler OnClick;

    public static event EventHandler OnMouseExit;

    private static CanvasGroup canvasGroup;
    private static Transform weapon;
    private bool canBuy;
    private void Awake()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(weapon!= null)
        {
            Destroy(weapon.gameObject);
            weapon = null;
        }
        if (canBuy && weapon == null)
        {
            weapon = Instantiate(prefab);
            Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionInWorld.z = 0f;
            weapon.position = mousePositionInWorld;
            canvasGroup.alpha = .2f;
            GameManager.Instance.isCanvasClicked = true;
            if(GameManager.Instance.GetWeapon()!=null)
            {
                OnClick?.Invoke(this, EventArgs.Empty);
            }
            GameManager.Instance.ChangeSelectedWeapon(weapon.GetComponent<BaseWeapon>());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke(this, new OnMouseEnterEventArgs { 
            cost = this.cost 
        });
        if (GameManager.Instance.GetAmountOfMoney() >= cost && GameManager.Instance.IsGameInPrepareMode())
        {
            canBuy = true;
        }
        else canBuy = false;
    }
    public static void WeaponToNull()
    {
        weapon = null;
    }

    public static void RestartCanvas()
    {
        canvasGroup.alpha = 1f;
    }
}
