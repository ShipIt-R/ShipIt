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
        // Weil es ein Prefab ist muss ich die GameObjects mit Tags suchen
        tilemap = GameObject.FindGameObjectWithTag("FloorTilemap").GetComponent<Tilemap>();
        pathOverlayTilemap = GameObject.FindGameObjectWithTag("OverlayTilemap").GetComponent<Tilemap>();
        // H�nge den Startpunkt an den Pfad an
        Vector3Int stallpos = new Vector3Int(14, 0, 0);
        pathPoints.Add(tilemap.GetCellCenterWorld(stallpos));
        AddPathPoint(stallpos);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !finished)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

            // Pr�fen, ob das angeklickte Tile ein PathTile ist
            TileBase clickedTile = tilemap.GetTile(cellPos);
            if (System.Array.IndexOf(pathTiles, clickedTile) == -1)
                return;

            Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPos);

            // Pr�fen, ob der neue Punkt diagonal zum letzten ist
            if (pathPoints.Count > 0)
            {
                Vector3 last = pathPoints[pathPoints.Count - 1];
                Vector3 diff = tileCenter - last;
                if (Mathf.Abs(diff.x) > 0 && Mathf.Abs(diff.y) > 0) // Wenn es einen Unterschied in der x oder y Koordinate gibt zwischen dem 1. und 2. Punkt wird der Punkt nicht hinzugef�gt
                    return;
            }

            pathPoints.Add(tileCenter); // F�ge das Tile dem Pfad hinzu
            AddPathPoint(cellPos); // F�ge Das Tile zum Overlay hinzu

            // Wenn ein Punkt gesetzt wurde, pr�fen ob es ein Endfeld ist
            if (pathPoints.Count > 1)
            {
                // Debug.Log(cellPos.ToString() + endRange.ToString());
                if (cellPos.x > endRange[0] && cellPos.x < endRange[1] && cellPos.y > endRange[2] && cellPos.y < endRange[3])
                {
                    // Pfad ist fertig
                    Debug.Log("Pfad abgeschlossen!");
                    ClearVisualPath();
                    finished = true; // Der Pfad wird als fertig markiert
                    return;
                }
            }
        }
    }

    private void ClearPath() // Diese Funktion kann benutzt werden um den Pfad zu l�schen
    {
        pathPoints.Clear();
        ClearVisualPath();
        Debug.Log("Pfad gel�scht!");
    }

    void AddPathPoint(Vector3Int cellPos) // Funktion um das Overlay zu setzen und die Pfadpunktliste zu aktualisieren
    {
        pathCells.Add(cellPos);
        pathOverlayTilemap.SetTile(cellPos, pathVisualTile);
    }

    void ClearVisualPath() // Das Overlay wird gel�scht
    {
        foreach (var cell in pathCells) // Jedes Tile der Liste wird mit null �berschrieben
            pathOverlayTilemap.SetTile(cell, null);
        pathCells.Clear();
    }
    public List<Vector3> GetPathPoints() // Der Pfad wird zur�ckgegeben, aber nur wenn das Pfadzeichnen fertig ist
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
