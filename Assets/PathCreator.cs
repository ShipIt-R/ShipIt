using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PathCreator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] pathTiles; // Alle PathTiles, die erlaubt sind
    public int[] endRange;   // Der erlaubte Zielbereich

    private List<Vector3> pathPoints = new List<Vector3>();
    public bool finished = false;

    public Tilemap pathOverlayTilemap;
    public TileBase pathVisualTile;    // Das Tile, das den Pfad anzeigt

    private List<Vector3Int> pathCells = new List<Vector3Int>();

    void Start()
    {
        Vector3Int stallpos = new Vector3Int(14, 0, 0);
        pathPoints.Add(tilemap.GetCellCenterWorld(stallpos));
        AddPathPoint(stallpos);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

            // Prüfen, ob das angeklickte Tile ein PathTile ist
            TileBase clickedTile = tilemap.GetTile(cellPos);
            if (System.Array.IndexOf(pathTiles, clickedTile) == -1)
                return;

            Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPos);

            // Prüfen, ob der neue Punkt diagonal zum letzten ist
            if (pathPoints.Count > 0)
            {
                Vector3 last = pathPoints[pathPoints.Count - 1];
                Vector3 diff = tileCenter - last;
                if (Mathf.Abs(diff.x) > 0 && Mathf.Abs(diff.y) > 0)
                    return; // Diagonal, also nicht hinzufügen
            }

            pathPoints.Add(tileCenter);
            AddPathPoint(cellPos);

            // Wenn ein Punkt gesetzt wurde, prüfen ob es ein Endfeld ist
            if (pathPoints.Count > 1)
            {
                // Debug.Log(cellPos.ToString() + endRange.ToString());
                if (cellPos.x > endRange[0] && cellPos.x < endRange[1] && cellPos.y > endRange[2] && cellPos.y < endRange[3])
                {
                    // Pfad ist fertig
                    Debug.Log("Pfad abgeschlossen!");
                    ClearVisualPath();
                    finished = true;
                    return;
                }
            }
        }
    }

    // Dies Passiert wenn der Pfad ungültig ist.
    private void ClearPath()
    {
        pathPoints.Clear();
        ClearVisualPath();
        Debug.Log("Pfad gelöscht!");
    }

    void AddPathPoint(Vector3Int cellPos)
    {
        pathCells.Add(cellPos);
        pathOverlayTilemap.SetTile(cellPos, pathVisualTile);
    }

    void ClearVisualPath()
    {
        foreach (var cell in pathCells)
            pathOverlayTilemap.SetTile(cell, null);
        pathCells.Clear();
    }
    public List<Vector3> GetPathPoints()
    {
        if (finished) 
        {
            return pathPoints;
        } else
        {
            return null;
        }
    }
}
