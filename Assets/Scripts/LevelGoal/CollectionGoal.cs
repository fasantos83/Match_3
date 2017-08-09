using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : MonoBehaviour {

    public GamePiece prefabToCollect;

    [Range(1, 50)]
    public int numberToCollect = 5;

    SpriteRenderer spriteRenderer;

    void Start () {
        if(prefabToCollect != null) {
            spriteRenderer = prefabToCollect.GetComponent<SpriteRenderer>();
        }		
	}

    public void CollectPiece(GamePiece piece) {
        if (piece != null) {
            SpriteRenderer pieceSR = piece.GetComponent<SpriteRenderer>();
            if (spriteRenderer.sprite == pieceSR.sprite && prefabToCollect.matchValue == piece.matchValue) {
                numberToCollect--;
                numberToCollect = Mathf.Clamp(numberToCollect, 0, numberToCollect);
            }
        }
    }
	
}
