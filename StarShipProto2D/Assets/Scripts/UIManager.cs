using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager: MonoBehaviour
{

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider fuelSlider;
    
    [SerializeField] private GameObject gameOver;

    private TextMeshPro gameOverText;


    private void Start()
    {
        gameOverText = gameOver.GetComponent<TextMeshPro>();
        Starship.tookDamage += UpdateHealth;
        Starship.playerDied += GameOver;
        Fuel.collectFuel += UpdateFuel;
    }


    private void UpdateHealth()
    {
        healthSlider.value--; 
    }

    private void UpdateFuel()
    {
        fuelSlider.value++;
    }

    private void GameOver()
    {
        
        gameOver.SetActive(true);

        while(gameOverText.alpha < 255)
        {
            gameOverText.alpha += 10;
            
        }
        
    }

    private void OnDestroy()
    {
        Starship.tookDamage -= UpdateHealth;
        Starship.playerDied -= GameOver;
        Fuel.collectFuel -= UpdateFuel;
    }

}
