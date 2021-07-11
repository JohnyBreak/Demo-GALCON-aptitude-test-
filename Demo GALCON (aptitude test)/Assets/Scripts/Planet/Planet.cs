using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    [SerializeField] private int _leaveShipsCount, _maxShips = 300;
    public bool _isSelected;
    public int countOfShips = 0;
    private float _planetRadius;
    [SerializeField] private GameObject sprite, selectLight;
    [SerializeField] private Material playerMat, enemyMat, neutralMat;
    [SerializeField] private TextMeshPro tmp;
    [SerializeField] private int _maxRange = 5, _minRange = 65;
    private float increaseTimer;
    private GameObject _planetForAttack;


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
    }
    void SetStatus()
    {
        switch (planetStatus)
        {
            case Status.Player:
                sprite.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case Status.Enemy:
                sprite.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case Status.Neutral:
                sprite.GetComponent<SpriteRenderer>().color = Color.gray;
                break;
        }
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
            SpawnShips();


    }
    void SpawnShips()
    {
        List<Vector2> positionList = new List<Vector2>();

        List<Vector2> shipsList = new List<Vector2>();

        for (int i = 0; i < _leaveShipsCount; i++)
        {
            float angle = i * (360f / _leaveShipsCount);
            Vector2 pos2d = transform.position + new Vector3(Mathf.Sin(angle) * _planetRadius, Mathf.Cos(angle) * _planetRadius);

            if (planetStatus != Status.Neutral)
            {
                GameObject newShip = (GameObject)Instantiate(ship, pos2d, ship.transform.rotation);
                
                    newShip.GetComponent<Ship>().SetShip(_planetForAttack, this);
                
                
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
        if (planetStatus != Status.Neutral && countOfShips < _maxShips)
        {
            countOfShips++;
            tmp.text = countOfShips.ToString();
        }
    }
    public void TakeDamage(Ship.Status shipStatus)
    {
        if (countOfShips > 0)
        {
            countOfShips--;
            tmp.text = countOfShips.ToString();
        }
        else
        {
            if (shipStatus == Ship.Status.Player)
                planetStatus = Status.Player;
            else
            {
                planetStatus = Status.Enemy;
                selectLight.SetActive(false);
            }
                

            SetStatus();
            StartIncreasingShips();
        }
    }
    public void SetPlanetForAttack(GameObject planetForAttack)
    {
        _planetForAttack = planetForAttack;
    }
    void StartIncreasingShips()
    {
        InvokeRepeating("IncreaseShips", 0, .45f);
    }
    public void TakeComrads()
    {
        countOfShips++;
        tmp.text = countOfShips.ToString();
    }
}
