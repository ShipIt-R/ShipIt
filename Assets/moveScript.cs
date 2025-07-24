using JetBrains.Annotations;
using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class moveScript : MonoBehaviour
{
    // Liste mit den Koordinaten, die das Objekt der Reihe nach abl�uft.
    public float[][] path = new float[][] 
    {
        new float[] { 1.53f, 0.72f }, new float[] { -1.53f, 0.72f }, new float[] { -1.53f, 0.24f }, new float[] { 1.04f, 0.24f }, new float[] { 1.04f, -0.24f }, new float[] {-1.53f, -0.24f}, new float[] {-1.53f, -0.72f}, new float[] { 1.53f, -0.72f }, new float[] { 1.53f, 0.72f }
    };

    // Ver�nderbare Werte
    public float speed;
    public bool moving = true;
    private int point = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Das Objekt wird an den Startpunkt "Teleportiert"
        transform.position = new Vector3(path[0][0], path[0][1], 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Wenn das Objekt sich bewegen sollte... wird der ganze Block ausgef�hrt.
        if (moving) {
            // Wenn es am Ende angekommen ist
            if (point == path.Length - 1)
            {
                // Zum Testen teleportiert es sich am Ende zur�ck zum Anfang. Sp�ter h�rt es auf sich zu bewegen.
                //moving = false;
                point = 0;
                transform.position = new Vector3(path[0][0], path[0][1], 0);
            } else
            {
                // Der einfachheit halber werden hier noch einige Variablen definiert, damit man sie nicht immer neu ausrechnen muss.
                Vector3 moveVector = PathVector3(path[point], path[point + 1]) * speed * Time.deltaTime;
                float overwalk = CompareOverwalk(moveVector, new Vector3(path[point + 1][0], path[point + 1][1], 0) - transform.position);
                // Wenn man �ber den n�chsten Punkt hinuslaufen w�rde. Hierf�r Schaue ich ob der Vektor von der Position zum Zielpunkt kleiner ist, als der Vektor den ich laufen w�rde.
                if (overwalk > 0) 
                { 
                    // Wenn ich dar�berhinauslaufen w�rde, laufe ich zum n�chsten Punkt und ich bin einen Punkt weiter.
                    transform.position = new Vector3(path[point + 1][0], path[point + 1][1], 0);
                    point++;
                } else
                {
                    // Sonst laufe ich einfach den Vektor den ich laufen w�rde.
                    transform.position = transform.position + moveVector;
                }
            }
        }
    }

    // Verbindungsvektor von 2 Koordinaten berechnen
    private Vector3 PathVector3(float[] coordbase, float[] coordend)
    {
        return new Vector3(coordend[0] - coordbase[0], coordend[1] - coordbase[1], 0);
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
}
