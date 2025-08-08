using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LogicScript : MonoBehaviour
{
    public GameObject FinishedScreen;
    public GameObject vehicleSpawner;
    public GameObject DeliverButton;
    public Text ShopGoldText;
    public Text MineGoldText;
    public int minedGold = 0;
    public int deliveredGold = 0;
    public int GoldGoal;
    public float SecondsperGold;
    private float GoldTimer = 0.0f;
    public int GoldperVehicle = 5;

    // Update is called once per frame
    void Update()
    {
        // Gold generieren
        GoldTimer += Time.deltaTime;
        if (GoldTimer >= SecondsperGold)
        {
            GoldTimer -= SecondsperGold; // Timer zurücksetzen
            minedGold++; // Gold abbauen
        }

        // Die Anzeigezahlen des Golds aktualisieren
        MineGoldText.text = minedGold.ToString();
        ShopGoldText.text = deliveredGold.ToString() + "/" + GoldGoal.ToString();
    }

    public void LoadGold()
    {
        if (minedGold >= GoldperVehicle) // Wenn man genug Gold hat um es zu verladen
        {
            DeliverButton.SetActive(false); // Der Knopf wird deaktiviert, nachdem er gedrückt wurde.
            minedGold -= GoldperVehicle;
            vehicleSpawner.GetComponent<VehicleSpawner>().SpawnVehicle(); // Fahrzeug spawnen
        }
    }

    public void dumpGold()
    {
        deliveredGold += GoldperVehicle;
        // Prüfen, ob das Ziel erreicht wurde
        if (deliveredGold >= GoldGoal)
        {
            FinishLevel();
        }
    }

    public void deliverydestroyed()
    { 
        DeliverButton.SetActive(true); // Wenn der Lastwagen auf irgendwelche Weise zerstört wird, wird der Knopf wieder aktiviert
    }

    public void FinishLevel() // Der "Level Finished" Screen wird aktiviert
    {
        FinishedScreen.SetActive(true);
        DeliverButton.SetActive(false); // Man sollte nicht mehr Gold verladen können, wenn das Level fertig ist
    }

    public void ResetGame()
    {
        deliveredGold = 0;
        minedGold = 0;
        GoldTimer = 0.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Level neu laden
    }

    public void QuitGame()
    {
        Application.Quit(); // Spiel beenden
    }
}