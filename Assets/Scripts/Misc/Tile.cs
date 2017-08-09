using System.Collections;
using UnityEngine;

public enum TileType {
    Normal,
    Obstacle,
    Breakable
}

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour {

    public int xIndex;
    public int yIndex;

    Board board;

    public TileType tileType = TileType.Normal;

    SpriteRenderer spriteRenderer;

    public int breakableValue = 0;
    public Sprite[] breakableSprites;

    public Color normalColor;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y, Board board) {
        xIndex = x;
        yIndex = y;
        this.board = board;

        if (tileType == TileType.Breakable && breakableSprites[breakableValue] != null) {
            spriteRenderer.sprite = breakableSprites[breakableValue];
        }
    }

    void OnMouseDown() {
        if (board != null) {
            board.ClickTile(this);
        }
    }

    void OnMouseEnter() {
        if (board != null) {
            board.DragToTile(this);
        }
    }

    void OnMouseUp() {
        if (board != null) {
            board.ReleaseTile();
        }
    }

    public void BreakTile() {
        if(tileType == TileType.Breakable) {
            StartCoroutine(BreakTileRoutine());
        }
    }

    IEnumerator BreakTileRoutine() {
        breakableValue = Mathf.Clamp(breakableValue--, 0, breakableValue);

        yield return new WaitForSeconds(0.25f);

        if(breakableSprites[breakableValue] != null) {
            spriteRenderer.sprite = breakableSprites[breakableValue];
        }

        if(breakableValue == 0) {
            tileType = TileType.Normal;
            spriteRenderer.color = normalColor;
        }
    }
}
