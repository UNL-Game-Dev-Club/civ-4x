using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameVar : MonoBehaviour {

	public Grid mainGrid;

	public Tilemap groundMap;
	public Tilemap terrainMap;

	public Vector2 mapSize;

    // Start is called before the first frame update
    void Start () {
        Game.gameVar = GetComponent<GameVar>();
    }

    // Update is called once per frame
    void Update () {
        
    }
}
