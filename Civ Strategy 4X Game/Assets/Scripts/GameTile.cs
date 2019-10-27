using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{

    public TileOrdinance ordinance;

    public int tileSize;

    // tile coordinates getter
    public int[] coordinates;

    public Vector2 positionOnScreen;

    // TODO: create and implement Biome class for tile display / type
    // Biome environment;

    bool isRevealed = false;

    // TODO: create and implement Enemy class for an array of the current enemies on the tile
    // Enemy[] enemiesOnTile;

    public GameTile(int[] coordinates, TileOrdinance ordinance)
    {
        this.coordinates = coordinates;
        this.ordinance = ordinance;
    }

    void Reveal()
    {
        isRevealed = true;

        // TODO: change the displayed sprite off of the "fog" to the tile's display sprites
    }
}

public enum TileOrdinance { BOTTOM, TOP };
