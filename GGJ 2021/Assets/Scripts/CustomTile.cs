using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTile : MonoBehaviour
{
    public Vector2 gridSize = new Vector2 (1,1);
    public bool Snap = true;
    private List <Vector2> _cantGo;

    public SpriteRenderer _spr;
    public Sprite _spriteOn;
    public Sprite _spriteOff;

    public void On()
    {
        _spr.sprite = _spriteOn;
    }

    public void Off()
    {
        _spr.sprite = _spriteOff;
    }

    public void OnDrawGizmos() {
        if (Snap)
            SnapToGrid();
    }

    public void SetGridSize()
    {
        gridSize = new Vector2(16, 16);
    }

    public void SnapToGrid()
    {
        var position = new Vector2(
            Mathf.Round(this.transform.position.x/this.gridSize.x) * this.gridSize.x,
            Mathf.Round(this.transform.position.y/this.gridSize.y) * this.gridSize.y
        );

        this.transform.position = position;

    }

    public Vector2 GridPos()
    {
        return new Vector2(this.transform.position.x/16, this.transform.position.y/16);
    }

    public void OccupiedTiles (List <Vector2> ocu)
    {
        if (_cantGo != null)
            _cantGo.Clear();

        _cantGo = ocu;
    }
}
