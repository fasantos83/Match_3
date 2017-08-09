using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardDeadlock : MonoBehaviour {

    List<GamePiece> GetRowOrColumnList(GamePiece[,] allPieces, int x, int y, int listLength = 3, bool checkRow = true) {
        int width = allPieces.GetLength(0);
        int height = allPieces.GetLength(1);

        List<GamePiece> pieces = new List<GamePiece>();

        for (int i = 0; i < listLength; i++) {
            if (checkRow) {
                if (x + i < width && y < height && allPieces[x + i, y] != null) {
                    pieces.Add(allPieces[x + i, y]);
                }
            } else {
                if (x < width && y + i < height && allPieces[x, y + i] != null) {
                    pieces.Add(allPieces[x, y + i]);
                }
            }
        }

        return pieces;
    }

    List<GamePiece> GetMinimumMatches(List<GamePiece> gamePieces, int minForMatch = 2) {
        List<GamePiece> matches = new List<GamePiece>();

        var groups = gamePieces.GroupBy(n => n.matchValue);

        foreach (var group in groups) {
            if (group.Count() >= minForMatch && group.Key != MatchValue.None) {
                matches = group.ToList();
            }
        }

        return matches;
    }

    List<GamePiece> GetNeighbors(GamePiece[,] allPieces, int x, int y) {
        int width = allPieces.GetLength(0);
        int height = allPieces.GetLength(1);

        List<GamePiece> neighbors = new List<GamePiece>();

        Vector2[] seacrhDirections = new Vector2[4] { Vector2.left, Vector2.right, Vector2.up, Vector2.down };

        foreach (Vector2 dir in seacrhDirections) {
            int xDir = x + (int)dir.x;
            int yDir = y + (int)dir.y;
            if (xDir >= 0 && xDir < width && yDir >= 0 && yDir < height) {
                if (allPieces[xDir, yDir] != null && !neighbors.Contains(allPieces[xDir, yDir])) {
                    neighbors.Add(allPieces[xDir, yDir]);
                }
            }
        }

        return neighbors;
    }

    bool HasMoveAt(GamePiece[,] allPieces, int x, int y, int listLength = 3, bool checkRow = true) {
        bool hasMoveAt = false;

        List<GamePiece> pieces = GetRowOrColumnList(allPieces, x, y, listLength, checkRow);
        List<GamePiece> matches = GetMinimumMatches(pieces, listLength - 1);

        GamePiece unmatchedPiece = null;
        if (pieces != null && matches != null) {
            if (pieces.Count == listLength && matches.Count == listLength - 1) {
                unmatchedPiece = pieces.Except(matches).FirstOrDefault();
            }

            if (unmatchedPiece != null) {
                List<GamePiece> neighbors = GetNeighbors(allPieces, unmatchedPiece.xIndex, unmatchedPiece.yIndex);
                neighbors = neighbors.Except(matches).ToList();
                neighbors = neighbors.FindAll(n => n.matchValue == matches[0].matchValue);

                matches = matches.Union(neighbors).ToList();
            }

            if (matches.Count >= listLength) {
                string rowOrColStr = checkRow ? "row" : "column";
                Debug.Log("====== AVAILABLE MOVE ======");
                Debug.Log("Move " + matches[0].matchValue + " piece to " + unmatchedPiece.xIndex + "," + unmatchedPiece.yIndex + " to from matching " + rowOrColStr);

                hasMoveAt = true;
            }
        }

        return hasMoveAt;
    }

    public bool IsDeadlocked(GamePiece[,] allPieces, int listLength = 3) {
        int width = allPieces.GetLength(0);
        int height = allPieces.GetLength(1);
        bool isDeadLocked = true;

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (HasMoveAt(allPieces, i, j, listLength, true) || HasMoveAt(allPieces, i, j, listLength, false)) {
                    isDeadLocked = false;
                }
            }
        }

        if (isDeadLocked) {
            Debug.Log("====== BOARD DEADLOCKED ======");
        }
        return isDeadLocked;
    }
}
