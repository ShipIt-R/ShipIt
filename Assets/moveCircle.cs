using JetBrains.Annotations;
using UnityEngine;

public class moveCircle : MonoBehaviour
{
    public float speed;
    public float circle_size;
    public int degree_circle_start;
    public int degree_circle_end;
    private float t;
    private int increment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = degree_circle_start;
        if (degree_circle_end > degree_circle_start)
        {
            increment = 1;
        } else
        {
            increment = -1;
        }
            transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // t verändern
        t += increment * speed * Time.deltaTime;
        if ((t > degree_circle_end & increment == 1) | (t < degree_circle_end & increment == -1))
        {
            t -= degree_circle_end - degree_circle_start;
        }

        // Position verändern
        transform.position = new Vector3(circle_size * Mathf.Cos(Rad(t)), circle_size * Mathf.Sin(Rad(t)), 0);
    }

    private float Rad(float deg)
    {
        return deg * Mathf.Deg2Rad;
    }
}
