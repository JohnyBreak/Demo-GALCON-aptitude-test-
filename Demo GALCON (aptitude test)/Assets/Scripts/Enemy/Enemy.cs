using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _planetForAttack;
    [SerializeField]
    private float _cooldown = 5f, startAttackTime = 3f;
    private Planet  _capturedPlanet;
    [SerializeField] private int _countOfShips;
    [SerializeField] private MapGenerator mapGenerator;
    private List<GameObject> _planetList;
    [SerializeField] private List<GameObject> _planetsForAttackList;
    [SerializeField] private List<GameObject> _capturedPlanetsList;
    

    void Start()
    {
        _planetList = mapGenerator.planetList;
        InvokeRepeating("UpdatePlanetLists", startAttackTime, _cooldown);
        //InvokeRepeating("ClearPlanetLists", 3f, 5f);
    }
    void UpdatePlanetLists()
    {
        foreach (GameObject planet in _planetList)
        {
            if (planet.GetComponent<Planet>().planetStatus == Planet.Status.Enemy)
            {
                
                _capturedPlanetsList.Add(planet);
            }
            else
            {
                _planetsForAttackList.Add(planet);
            }
        }
        if (_capturedPlanetsList.Count > 0)
        {
            
            PickAttackPlanet();
            PickPlanetForAttack();
            Attack();
        }
    }
    void PickAttackPlanet()
    {
        foreach (GameObject planet in _capturedPlanetsList)
        {
            if (planet.GetComponent<Planet>().countOfShips > 25)
            {
                _capturedPlanet = planet.GetComponent<Planet>();
                break;
            }
        }
    }
    void PickPlanetForAttack()
    {
        _planetForAttack = null;
        foreach (GameObject planet in _planetsForAttackList)
        {
            if (_capturedPlanet.countOfShips / 2 > planet.GetComponent<Planet>().countOfShips)
            {
                _planetForAttack = planet;
                
            }
        }
    }
    void Attack()
    {
        if (_planetForAttack != null)
        {
            _capturedPlanet.SetPlanetForAttack(_planetForAttack);
            _capturedPlanet.ReduceShips();
            
        }
        ClearPlanetLists();
    }
    void ClearPlanetLists()
    {
        _planetsForAttackList.Clear();
        _capturedPlanetsList.Clear();
    }
    public void UpdateCountOfShips(int ships)
    {
        _countOfShips = ships;
    }
}
