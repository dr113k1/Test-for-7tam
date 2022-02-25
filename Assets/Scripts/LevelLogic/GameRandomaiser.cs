using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameRandomaiser : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject destructibleWall;
    [SerializeField] GameObject enemyObj1;
    [SerializeField] GameObject enemyObj2;
    [SerializeField] GameObject enemyObj3;
    [SerializeField] Button button;
    [SerializeField] GameObject UIGameObject;
    float playerSpeed = 4;
    float bombTimer = 3f;
    int bombRadius = 3;
    float bombCoolDown = 1f;
    int playerCoordinatesX;
    int playerCoordinatesY;
    int enemyCount=4;
    int[] eneemyXCord;
    int[] eneemyYCord;
     int destructibleWallCount=30;
    void Start()
    {
        PlayerPrefsLoad();
        UIGameObject.GetComponent<UIManager>().GetGameSettings(playerSpeed, bombCoolDown, bombTimer, bombRadius, destructibleWallCount, enemyCount);
        eneemyXCord = new int[enemyCount];
        eneemyYCord = new int[enemyCount];
        PlayerSpawn();
        EnemySpawn();
        DestructibleWallSpawn();
    }
    void Update()
    {
        WinWatcher();
    }
    void WinWatcher()
    {
        if (enemyCount <= 0)
        {
           
            UIGameObject.GetComponent<UIManager>().Win();
        }
    }
   public void EnemyDestroyed()
    {
        enemyCount--;
    }
   public void PlayerDestroyed()
    {
        UIGameObject.GetComponent<UIManager>().Lose();
    }
    private void PlayerSpawn()//спавн игрока
    {
        playerCoordinatesX = Random.Range(-8, 9);
        do
        {
            playerCoordinatesY = Random.Range(-4, 5);
        } 
        while (playerCoordinatesY % 2 != 0);

        player.transform.position = new Vector3(playerCoordinatesX, 0.75f, playerCoordinatesY);
        player.GetComponent<Player>().OnGameStart(playerSpeed, bombCoolDown, bombTimer, bombRadius,gameObject);
        button.onClick.AddListener(player.GetComponent<Player>().BombPlantButton);
    }
    void PlayerPrefsLoad()//загрузка последних настроек
    {
        if (PlayerPrefs.HasKey("playerSpeed"))
            playerSpeed = PlayerPrefs.GetFloat("playerSpeed");
        else
            PlayerPrefs.SetFloat("playerSpeed", playerSpeed);

        if (PlayerPrefs.HasKey("bombTimer"))
            bombTimer = PlayerPrefs.GetFloat("bombTimer");
        else
            PlayerPrefs.SetFloat("bombTimer", bombTimer);

        if (PlayerPrefs.HasKey("bombRadius"))
            bombRadius = PlayerPrefs.GetInt("bombRadius");
        else
            PlayerPrefs.SetInt("bombRadius", bombRadius);

        if (PlayerPrefs.HasKey("bombCoolDown"))
            bombCoolDown = PlayerPrefs.GetFloat("bombCoolDown");
        else
            PlayerPrefs.SetFloat("bombCoolDown", bombCoolDown);

        if (PlayerPrefs.HasKey("destructibleWallCount"))
            destructibleWallCount = PlayerPrefs.GetInt("destructibleWallCount");
        else
            PlayerPrefs.SetInt("destructibleWallCount", destructibleWallCount);

        if (PlayerPrefs.HasKey("enemyCount"))
            enemyCount = PlayerPrefs.GetInt("enemyCount");
        else
            PlayerPrefs.SetInt("enemyCount", enemyCount);


    }


    private void EnemySpawn()//спавн врагов
    {
        int i = 0;
        while (i < enemyCount)
        {
            int X = Random.Range(-8, 9);
            int Y = Random.Range(-4, 5);
            if ((X == playerCoordinatesX) && (playerCoordinatesY - 2 <= Y) && (playerCoordinatesY + 2 >= Y))
                continue;
            if ((Y == playerCoordinatesY) && (playerCoordinatesX - 2 <= X) && (playerCoordinatesX + 2 >= X))
                continue;
            if ((Y % 2 != 0) && (X % 2 != 0))
                continue;
            int a = Random.Range(1, 4);
            eneemyXCord[i] = X;
            eneemyYCord[i] = Y;
            GameObject enemy;
            switch (a)
            {
                case 1:
                    {
                        bool turn;
                        var randomaser = Random.value < 0.5f ? turn = true : turn = false;
                        if (turn)
                        {
                            enemy=Instantiate(enemyObj1, new Vector3(X, 0.75f, Y), Quaternion.identity);

                        }
                        else
                        {
                            enemy= Instantiate(enemyObj1, new Vector3(X, 0.75f, Y), Quaternion.Euler(0, transform.eulerAngles.y +90, 0));
                        }
                        enemy.GetComponent<Enemy>().OnGameStart(gameObject);
                        break;
                    }
                case 2:
                    {
                        enemy=Instantiate(enemyObj2, new Vector3(X, 0.75f, Y), Quaternion.identity);
                        enemy.GetComponent<Enemy>().OnGameStart(gameObject);
                        break;
                    }
                case 3:
                    {
                        enemy=Instantiate(enemyObj3, new Vector3(X, 0.75f, Y), Quaternion.identity);
                        enemy.GetComponent<Enemy>().OnGameStart(gameObject);
                        break;
                    }
            }
            i++;
        }
    }

    private void DestructibleWallSpawn()//спавн стенок
    {
        GameObject DestrWals = Instantiate(new GameObject("Destr_Walls"));
        while (destructibleWallCount > 0)
        {
            int wallX = Random.Range(-8, 9);
            int wallY = Random.Range(-4, 5);
            if ((wallX == playerCoordinatesX) && (playerCoordinatesY - 1 <= wallY) && (playerCoordinatesY + 1 >= wallY))
                continue;
            if ((wallY == playerCoordinatesY) && (playerCoordinatesX - 1 <= wallX) && (playerCoordinatesX + 1 >= wallX))
                continue;
            if ((wallY % 2 != 0) && (wallX % 2 != 0))
                continue;
            bool onEnemy = false;
            for (int i = 0; i < eneemyXCord.Length; i++)
            {
                if ((wallX == eneemyXCord[i]) && (wallY == eneemyYCord[i]))
                {
                    onEnemy = true;
                }
            }
            if (onEnemy == true)
                continue;

            Instantiate(destructibleWall, new Vector3(wallX, 0.75f, wallY), Quaternion.identity, DestrWals.transform);
            destructibleWallCount--;
        }
    }
}
