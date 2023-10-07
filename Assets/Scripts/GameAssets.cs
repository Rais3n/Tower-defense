using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameAssets : MonoBehaviour
{
    //private static GameAssets _i;
    //public static GameAssets i { 
    //    get { 
    //        if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
    //        return _i; } 
    //}

    public static GameAssets i { get; private set; }

    public Tile tilemapSpritesAvailableSpaces;
    public Tile[] tilemapSpritesNotAvailableSpaces;
    
    public Tilemap background;
    public Tilemap decorations;
    public Transform cannonball;
    public Transform arrow;
    public Transform rocket;
    public Transform damagePopupTransform;

    private void Awake()
    {
        i = this;
    }

    public Transform GetProjectile(string projectile)
    {
        return projectile switch
        {
            "cannonball" => cannonball,
            "arrow" => arrow,
            "rocket" => rocket,
            _ => null
        };
    }
}
