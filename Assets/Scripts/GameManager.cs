using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform enemiesSpawnPoint;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI startButtonText;

    public static GameManager Instance{ get; private set; }

    private BaseWeapon selectedWeapon;
    private int maxEnemies=20;
    private int currentNumberOfEnemies;
    private float spawnTimer;
    private float resetSpawnTime=0f;
    private int money = 5000;
    private int cost;
    private int HP = 100;
    private bool isDragging;

    public bool isCanvasClicked;
    private enum State
    {
        WaitingToStart,
        PreroundPreparation,
        DuringTheRound,
        Pause,
        GameOver,
    }

    private State state;
    private void Awake()
    {
        Instance = this;
        state = State.PreroundPreparation;
        startButton.onClick.AddListener(() =>
        {
            if (state == State.PreroundPreparation)
            {
                state = State.DuringTheRound;
                startButtonText.text = "PAUSE";
            }
            else if (state == State.DuringTheRound)
            {
                state = State.Pause;
                startButtonText.text = "RESUME";
                Time.timeScale = 0f;
            }
            else if (state == State.Pause)
            {
                state = State.DuringTheRound;
                startButtonText.text = "PAUSE";
                Time.timeScale = 1f;
            }
        });
    }
    private void Start()
    {
        Enemy.enemyList.RemoveAt(0);
        DragDropWeapon.OnMouseEnter += DragDropWeapon_OnMouseEnter;
        Enemy.OnEnemyDeath += Enemy_OnEnemyDeath;
        moneyText.text = "MONEY: " + money.ToString();
    }

    private void DragDropWeapon_OnMouseEnter(object sender, DragDropWeapon.OnMouseEnterEventArgs e)
    {
        cost = e.cost;
    }

    private void Update()
    {
        switch (state)
        {
            case State.PreroundPreparation:
                if (isDragging)
                {
                    if (selectedWeapon != null)
                    {
                        DragWeapon(selectedWeapon.transform);
                    }
                }
                break;
            case State.DuringTheRound:
                SpawnEnemies();
                break;
        }

    }
    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f, LayerMask.GetMask("Weapons"));
            if (hit.collider == null)
            {
                OnClickActions();
            }
            else
            {
                AssignSelectedWeapon(hit);
            }
        }
    }

    private void AssignSelectedWeapon(RaycastHit2D hit)
    {
        if (!isDragging)
        {
            selectedWeapon = hit.transform.GetComponentInParent<BaseWeapon>();
        }
    }
    private void OnClickActions()
    {
        if (isCanvasClicked)
        {
            isCanvasClicked = !isCanvasClicked;
            isDragging = true;
            ChangeTemporaryLayerMask();
        }
        else
        {
            if (isDragging)
            {
                if (IsPlaceAvailable())
                {
                    isDragging = false;
                    ReturnToDefaultSettings();
                    DragDropWeapon.WeaponToNull();
                    WeaponPurchase();
                    DragDropWeapon.RestartCanvas();
                    selectedWeapon.GetComponentInChildren<WeaponUI>().ChangeState();
                }
            }
            else
            {
                selectedWeapon = null;
            }
        }
    }
    public bool IsPlaceAvailable()
    {
        
        Tilemap background = GameAssets.i.background;
        Tilemap decorations = GameAssets.i.decorations;
        Tile tilemapSpritesAvailableSpaces = GameAssets.i.tilemapSpritesAvailableSpaces;
        Tile[] tilemapSpritesNotAvailableSpaces = GameAssets.i.tilemapSpritesNotAvailableSpaces;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = background.WorldToCell(mousePosition);
        if (background.GetTile(cellPosition) == tilemapSpritesAvailableSpaces)
        {
            cellPosition = decorations.WorldToCell(mousePosition);
            foreach (Tile tilemap in tilemapSpritesNotAvailableSpaces)
            if(decorations.GetTile(cellPosition) == tilemap)
            {
                    return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private void DragWeapon(Transform weapon)
    {
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePositionInWorld.z = 0f;
        weapon.position = mousePositionInWorld;
    }
    private void ChangeTemporaryLayerMask()
    {
        GameObject visualUI = selectedWeapon.transform.GetComponentInChildren<Collider2D>().gameObject;
        int draggedWeapon = 3;
        visualUI.layer = draggedWeapon;
    }
    private void ReturnToDefaultSettings()
    {
        GameObject visualUI = selectedWeapon.transform.GetComponentInChildren<Collider2D>().gameObject;
        visualUI.GetComponent<Collider2D> ().enabled = true;
        int weapons = 7;
        visualUI.layer = weapons;
    }

    private void Enemy_OnEnemyDeath(object sender, Enemy.OnEnemyDeathEventArgs e)
    {
        money += e.money;
        HP -= e.damage;
        moneyText.text = "MONEY: " + money;
        HPText.text = "HP: " + HP;
    }

    private void WeaponPurchase()
    {
        money -= cost;
        moneyText.text = "MONEY: " + money;
        cost = 0;
    }

    private void SpawnEnemies()
    {
        float nextSpawnTime = .2f;
        if (currentNumberOfEnemies < maxEnemies && spawnTimer > nextSpawnTime)
        {
            Transform enemyTransform = Instantiate(enemyPrefab, enemiesSpawnPoint);
            enemyTransform.localPosition = Vector3.zero;
            enemyTransform.gameObject.SetActive(true);

            currentNumberOfEnemies++;
            spawnTimer = resetSpawnTime;
        }
        spawnTimer += Time.deltaTime;
    }

    public int GetAmountOfMoney()
    {
        return money;
    }

    public bool IsGameInPrepareMode()
    {
        return state == State.PreroundPreparation;
    }

    public void ChangeSelectedWeapon(BaseWeapon baseWeapon)
    {
        selectedWeapon = baseWeapon;
    }
    public BaseWeapon GetWeapon()
    {
        return selectedWeapon;
    }

    public bool IsDragging()
    {
        return isDragging;
    }
}
