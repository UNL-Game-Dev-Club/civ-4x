using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

	public Tilemap groundMap;
	public Tilemap terrainMap;

	public Tile[] landTiles = new Tile[20];

    // Start is called before the first frame update
    void Start () {
        GenerateMap(10, 10);
    }

    // Update is called once per frame
    void Update () {
        
    }

    //Randomly generate a new map of the given dimensions sizeX and sizeY
    void GenerateMap (int sizeX, int sizeY) {
    	for (int x = 0; x < sizeX; x++) {
        	for (int y = 0; y < sizeY; y++) {
        		groundMap.SetTile(new Vector3Int(x, y, 0), landTiles[0]);

        		int nextTile = Random.Range(0, 3);

        		if (nextTile > 0) {
        			terrainMap.SetTile(new Vector3Int(x, y, 0), landTiles[nextTile]);
        		}
        	}
        }
    }
}
