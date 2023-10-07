using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedWeaponColor;
    [SerializeField] private Color placeNotAvaiableColor;
    [SerializeField] private Color defaultAttackRangeColor;

    private BaseWeapon baseWeapon;
    private GameObject canvas;
    private SpriteRenderer spriteRenderer;
    private Image attackRangeImage;
    private bool isColorRed;

    private enum State
    {
        dragged,
        dropped
    }
    private State state;
    private void Awake()
    {
        canvas = transform.parent.Find("Canvas").gameObject;
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        baseWeapon = GetComponentInParent<BaseWeapon>();
        attackRangeImage = canvas.GetComponentInChildren<Image>();
        state = State.dragged;
    }
    private void Start()
    {
        DragDropWeapon.OnClick += DragDropWeapon_OnClick;
    }

    private void DragDropWeapon_OnClick(object sender, System.EventArgs e)
    {
        BaseWeapon weapon = GameManager.Instance.GetWeapon();
        if (baseWeapon == weapon)
        {
            canvas.SetActive(false);
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.dragged:
                ChangeAttackRangeColor();
                break;
            case State.dropped:
                if (Input.GetMouseButtonDown(0))
                {
                    BaseWeapon currentSelectedWeapon = GameManager.Instance.GetWeapon();
                    if (currentSelectedWeapon != null)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        if (hit.collider == null && !GameManager.Instance.isCanvasClicked)
                        {
                            currentSelectedWeapon.transform.Find("Canvas").gameObject.SetActive(false);
                        }
                    }
                }
                break;
        }
    }
    private void OnMouseDown()
    {
        canvas.SetActive(true);
        BaseWeapon currentSelectedWeapon = GameManager.Instance.GetWeapon();

        if (this.baseWeapon != currentSelectedWeapon && currentSelectedWeapon != null)
        {
            currentSelectedWeapon.transform.Find("Canvas").gameObject.SetActive(false);
        }
    }
    private void OnMouseEnter()
    {
        spriteRenderer.color = selectedWeaponColor;
    }
    private void OnMouseExit()
    {
        spriteRenderer.color = defaultColor;
    }
    public BaseWeapon GetWeapon()
    {
        return baseWeapon;
    }
    public void ChangeState()
    {
        state = State.dropped;
    }
    private void ChangeAttackRangeColor()
    {
        int numberOfColliders = NumberOfCollidersOverlap();
        int anotherCollider = 1;
        if (numberOfColliders == anotherCollider)
        {
            attackRangeImage.color = placeNotAvaiableColor;
        }
        else
        {
            if (GameManager.Instance.IsPlaceAvailable())
            {
                attackRangeImage.color = defaultAttackRangeColor;
            }
            else attackRangeImage.color = placeNotAvaiableColor;
        }
    }
    private int NumberOfCollidersOverlap()
    {
        Collider2D collider = GameManager.Instance.GetWeapon().GetComponentInChildren<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        int weaponLayer = 7;
        contactFilter.layerMask = weaponLayer;
        Collider2D[] array = new Collider2D[1];
        return collider.OverlapCollider(contactFilter, array);
    }
}
