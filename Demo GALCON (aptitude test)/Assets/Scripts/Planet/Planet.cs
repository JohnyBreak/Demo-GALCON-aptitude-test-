using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    [SerializeField] private int _leaveShipsCount;
    public bool _isSelected;
    public int countOfShips = 0;
    private float _planetRadius;
    [SerializeField] private GameObject sprite, selectLight;
    [SerializeField] private Material playerMat, enemyMat, neutralMat;
    [SerializeField] private TextMeshPro tmp;
    [SerializeField] private int _maxRange = 5, _minRange = 65;
    private Enemy enemy;
    private float increaseTimer;
    public GameObject enemyAttackPlanet;
    bool timer = true;
    public enum Status
    {
        Player,
        Neutral,
        Enemy
    }
    public Status planetStatus;

    void Start()
    {
        float scl = transform.localScale.x;
        _planetRadius = GetComponent<CircleCollider2D>().radius * scl;
        if (countOfShips == 0)
        {
            countOfShips = Random.Range(_minRange, _maxRange);
            tmp.text = countOfShips.ToString();
        }
        _isSelected = false;
        SetStatus();
        StartIncreasingShips();
        enemy = GetComponent<Enemy>();
    }
    void SetStatus()
    {
        switch (planetStatus)
        {
            case Status.Player:
                sprite.GetComponent<SpriteRenderer>().color = Color.blue;
                //GetComponent<Enemy>().enabled = false;
                break;
            case Status.Enemy:
                sprite.GetComponent<SpriteRenderer>().color = Color.red;
                //GetComponent<Enemy>().enabled = true;
                break;
            case Status.Neutral:
                sprite.GetComponent<SpriteRenderer>().color = Color.gray;
                //GetComponent<Enemy>().enabled = true;
                break;
        }
    }
    void Update()
    {
        
    }

    public void CreatePlayerPlanet()
    {
        countOfShips = 50;
        planetStatus = Status.Player;
        SetStatus();
    }
    public void CreateEnemyPlanet()
    {
        countOfShips = 50;
        planetStatus = Status.Enemy;
        SetStatus();
    }

    public void ReduceShips()
    {
        _leaveShipsCount = countOfShips / 2;
        countOfShips -= _leaveShipsCount;
        tmp.text = countOfShips.ToString();
        //if (planetStatus == Status.Player)
            SpawnShips();
       /* else
            SpawnEnemyShips();*/

    }
    void SpawnShips()
    {
        List<Vector2> positionList = new List<Vector2>();
        List<GameObject> shipsForEnemyList = new List<GameObject>();
        for (int i = 0; i < _leaveShipsCount; i++)
        {
            float angle = i * (360f / _leaveShipsCount);
            Vector2 pos2d = transform.position + new Vector3(Mathf.Sin(angle) * _planetRadius, Mathf.Cos(angle) * _planetRadius);

            if (planetStatus == Status.Player)
                Instantiate(ship, pos2d, ship.transform.rotation);
            else
            {
                GameObject newPlanet = (GameObject)Instantiate(ship, pos2d, ship.transform.rotation);
                Debug.Log("enemySpawn");
                shipsForEnemyList.Add(newPlanet);
                
            }
                
        }
        if (planetStatus == Status.Enemy)
        {
            foreach (GameObject ship in shipsForEnemyList)
            {
                ship.GetComponent<Ship>().EnemyAttack(enemyAttackPlanet);
            }
        }
    }
    public void SelectPlanet()
    {
        _isSelected = !_isSelected;
        selectLight.SetActive(_isSelected);
    }
    void IncreaseShips()
    {
        if (planetStatus != Status.Neutral)
        {
            countOfShips++;
            UpdateEnemyShips();
            tmp.text = countOfShips.ToString();
        }
    }
    public void TakeDamage()
    {
        if (countOfShips > 0)
        {
            countOfShips--;
            UpdateEnemyShips();
            tmp.text = countOfShips.ToString();
        }
        else
        {
            planetStatus = Status.Player;
            SetStatus();
            StartIncreasingShips();
        }
    }
    void UpdateEnemyShips()
    {
        enemy.UpdateCountOfShips(countOfShips);
    }
    void SpawnEnemyShips()
    {
        
        
    }

    void StartIncreasingShips()
    {
        InvokeRepeating("IncreaseShips", 0, .4f);
    }
    

    public void TakeComrads()
    {
        countOfShips++;
        tmp.text = countOfShips.ToString();
    }
}
