using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject vehiclePrefab; // Prefab des Fahrzeugs, das gespawnt werden soll
    public void SpawnVehicle()
    {
        Instantiate(vehiclePrefab, transform.position, transform.rotation); // Prefab wird gespawnt
    }
}
