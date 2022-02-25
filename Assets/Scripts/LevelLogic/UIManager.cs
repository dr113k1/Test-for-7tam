using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider playerSpeedSlider;
    [SerializeField] Text playerSpeedSliderText;

    [SerializeField] Slider bombTimerSlider;
    [SerializeField] Text bombTimerSliderText;

    [SerializeField] Slider bombRadiusSlider;
    [SerializeField] Text bombRadiusSliderText;

    [SerializeField] Slider bombCoolDownSlider;
    [SerializeField] Text bombCoolDownSliderText;

    [SerializeField] Slider destructibleWallCountSlider;
    [SerializeField] Text destructibleWallCountSliderText;

    [SerializeField] Slider enemyCountSlider;
    [SerializeField] Text enemyCountSliderText;

    [SerializeField] GameObject Menu;
    [SerializeField] GameObject MenuButton;

    [SerializeField] GameObject Control_1;
    [SerializeField] GameObject Control_2;

    [SerializeField] GameObject WinImage;
    [SerializeField] GameObject LoseImage;

    [SerializeField] GameObject ResumeButton;
   
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        SettingsShow();
        SetGameSettigs();
    }
    void SettingsShow()//передача слайдерам значение настроек
    {
        playerSpeedSliderText.text = playerSpeedSlider.value.ToString("0");
        bombTimerSliderText.text = bombTimerSlider.value.ToString("0");
        bombRadiusSliderText.text = bombRadiusSlider.value.ToString("0");
        bombCoolDownSliderText.text = bombCoolDownSlider.value.ToString("0");
        destructibleWallCountSliderText.text = destructibleWallCountSlider.value.ToString("0");
        enemyCountSliderText.text = enemyCountSlider.value.ToString("0");
    }
    public void GetGameSettings(float playerSpeed,float bombCoolDown,float bombTimer, int bombRadius,int destructibleWallCount,int enemyCount)
    {
        playerSpeedSlider.value = playerSpeed;
        bombTimerSlider.value = bombTimer;
        bombRadiusSlider.value = bombRadius;
        bombCoolDownSlider.value = bombCoolDown;
        destructibleWallCountSlider.value = destructibleWallCount;
        enemyCountSlider.value = enemyCount;

    }
    void SetGameSettigs()//сохранение настроек
    {
      
            PlayerPrefs.SetFloat("playerSpeed", playerSpeedSlider.value);

            PlayerPrefs.SetFloat("bombTimer", bombTimerSlider.value);

        
            PlayerPrefs.SetInt("bombRadius", (int)bombRadiusSlider.value);

       
            PlayerPrefs.SetFloat("bombCoolDown", bombCoolDownSlider.value);

       
            PlayerPrefs.SetInt("destructibleWallCount", (int)destructibleWallCountSlider.value);

       
            PlayerPrefs.SetInt("enemyCount", (int)enemyCountSlider.value);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MenuActive()
    {
        Control_1.SetActive(false);
        Control_2.SetActive(false);
        MenuButton.SetActive(false);
        Menu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Control_1.SetActive(true);
        Control_2.SetActive(true);
        MenuButton.SetActive(true);
        Menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Win()
    {
        Time.timeScale = 0;
        MenuActive();
        WinImage.SetActive(true);
        ResumeButton.SetActive(false);
    }
    public void Lose()
    {
        Time.timeScale = 0;
        MenuActive();
        LoseImage.SetActive(true);
        ResumeButton.SetActive(false);
    }
}
