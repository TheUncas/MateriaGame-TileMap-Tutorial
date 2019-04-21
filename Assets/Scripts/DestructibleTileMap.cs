using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTileMap : MonoBehaviour
{
    #region Properties
    private Tilemap tileMap;
    private GridLayout grid;
    private Vector3Int tilePosition;
    #endregion

    #region Unity callbacks
    public void Start()
    {
        tileMap = GetComponent<Tilemap>();
        grid = GetComponentInParent<GridLayout>();

        foreach (Vector3Int position in tileMap.cellBounds.allPositionsWithin)
        {
            TileBase t = tileMap.GetTile(position);
            if (!Equals(t, null))
            {
                if (t is DestructibleTile)
                {
                    DestructibleTile dt = Instantiate(t) as DestructibleTile;
                    dt.StartUp(position, dt.tileMap, dt.gameObject);
                    tileMap.SetTile(position, dt);
                }
            }
        }

    }
    #endregion

    public void Damage(Projectile projectile, Vector3 pContactPoint)
    {
        TileBase tileToDamage = tileMap.GetTile(grid.WorldToCell(pContactPoint));
        if (!Equals(tileToDamage, null))
        {
            if (tileToDamage is DestructibleTile)
            {
                ((DestructibleTile)tileToDamage).ApplyDamage(10);
                tileMap.RefreshTile(((DestructibleTile)tileToDamage).tilePosition);
            }
        }
    }
}