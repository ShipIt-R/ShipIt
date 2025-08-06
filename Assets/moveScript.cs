using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class moveScript : MonoBehaviour
{
    // Referenz auf PathCreator, um auf pathPoints zuzugreifen
    public PathCreator pathCreator;

    // Geschwindigkeit und ob man sich bewegt
    public float speed;
    public bool moving = false;
    private int point = 0;

    // Die Liste mit den Wegpunkten
    private List<Vector3> pathPoints;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Warten, bis pathPoints gesetzt ist
        if (pathCreator != null && pathCreator.GetPathPoints() != null && pathCreator.GetPathPoints().Count > 0)
        {
            pathPoints = pathCreator.GetPathPoints();
            transform.position = pathPoints[0];
            moving = true;
        }
        else
        {
            moving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Pr�fen, ob pathPoints gesetzt und g�ltig ist
        if (!moving)
        {
            // Pr�fen, ob ein neuer Pfad erstellt wurde
            if (pathCreator != null && pathCreator.GetPathPoints() != null && pathCreator.GetPathPoints().Count > 0)
            {
                pathPoints = pathCreator.GetPathPoints();
                point = 0;
                transform.position = pathPoints[0];
                GetComponent<SpriteRenderer>().enabled = true;
                moving = true;
            }
            else
            {
                return;
            }
        }

        if (moving && pathPoints != null && pathPoints.Count > 1)
        {
            if (point == pathPoints.Count - 1)
            {
                // Am Ende angekommen: zur�ck zum Anfang
                point = 0;
                transform.position = pathPoints[0];
            }
            else
            {
                Vector3 moveVector = Vector3.Normalize(pathPoints[point + 1] - pathPoints[point]) * speed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, 0, GetAngleFromVector3(moveVector));
                float overwalk = CompareOverwalk(moveVector, pathPoints[point + 1] - transform.position);
                // Wenn der Vektor l�nger ist als der Abstand zum n�chsten Punkt, dann gehe zum n�chsten Punkt
                if (overwalk > 0)
                {
                    transform.position = pathPoints[point + 1];
                    point++;
                }
                else
                {
                    transform.position = transform.position + moveVector;
                }
            }
        }
    }

    // Vergleichen um wieveiel der eine Vektor l�nger ist als der andere.
    private float CompareOverwalk(Vector3 walkvec, Vector3 destvec)
    {
        // Diagonale Vektoren sollten nicht vorkommen, deshalb funktioniert meine berechnung mit diagonalen Vektoren nicht. Ich stelle hier sicher das es keine Diagonalen Vektoren sind.
        if (!((walkvec.x == 0 | walkvec.y == 0) & (destvec.x == 0 | destvec.y == 0)))
        {
            // Ich lasse einen Fehler geben, wenn ein Diagonaler Vektor berechnet wird
            throw new ArithmeticException("Der Lauf- oder Zielvektor ist diagonal!" + Convert.ToString(walkvec) + Convert.ToString(destvec));
        }
        // Hier rechne ich noch aus, wie viel ich �ber das Ziel hinausschiesse. Da einer der Vektorgr�ssen immer 0 ist, bekomme ich immer die L�nge.
        return Mathf.Abs(walkvec.x + walkvec.y) - Mathf.Abs(destvec.x + destvec.y);
    }

    private float GetAngleFromVector3(Vector3 dir)
    {
        float angleRad = Mathf.Atan2(dir.y, dir.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;

        return (angleDeg + 270f) % 360f;
    }
}
