using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Planet planet;
    [SerializeField] private int _countOfShips;
    private MapGenerator mapGenerator;
    [SerializeField] private List<GameObject> _planetList;
    [SerializeField] private List<GameObject> _planetsForAttackList;
    [SerializeField] private GameObject _planetForAttack;


    void Start()
    {
        planet = GetComponent<Planet>();
        mapGenerator = GameObject.FindGameObjectWithTag("Player").GetComponent<MapGenerator>();
        _planetList = mapGenerator.planetList;
        InvokeRepeating("PickPlanetForAttack", 0, 3f);
        InvokeRepeating("ClearPlanetForAttack", 1, 3f);
    }
    void Update()
    {
        
    }

    

    void PickPlanetForAttack()
    {
        
        _planetList = mapGenerator.planetList;
        
        foreach (GameObject planet in _planetList)
        {
            if (planet.GetComponent<Planet>().planetStatus != Planet.Status.Enemy)
            {
                _planetsForAttackList.Add(planet);
            }
        }

        Attack();
    }
    void Attack()
    {
        if (_countOfShips > 24)
        {
            foreach (GameObject planetAttack in _planetsForAttackList)
            {
                if (planetAttack.GetComponent<Planet>().countOfShips < _countOfShips / 2)
                {
                    //attack
                    _planetForAttack = planetAttack;
                    planet.enemyAttackPlanet = _planetForAttack;
                    planet.ReduceShips();
                }
            }
        }

    }
    void ClearPlanetForAttack()
    {
        _planetsForAttackList.Clear();
    }


    public void UpdateCountOfShips(int ships)
    {
        _countOfShips = ships;
    }
}
