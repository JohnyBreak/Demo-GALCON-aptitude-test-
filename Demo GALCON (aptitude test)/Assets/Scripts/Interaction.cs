using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private List<Planet> selectedPlanets;
    private RaycastHit2D hit;
    private Ray2D ray;
    private Planet planet;
    private Vector2 lastClickedPlanetPosition;
    private GameObject lastClickedPlanet;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
    }
    void ReducingShips()
    {
        foreach (Planet planet in selectedPlanets)
        {
            planet.ReduceShips();
        }
    }
    void CastRay()
    {
        Vector2 origin = Vector2.zero;
        Vector2 dir = Vector2.zero;
        origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(origin, dir);
        if (hit)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<Planet>() != null)
            {
                planet = hit.transform.GetComponent<Planet>();
                ClickPlanet(planet);
                lastClickedPlanetPosition = planet.transform.position;
            }
        }
    }

    void ClickPlanet(Planet planet)
    {
        switch (planet.planetStatus)
        {
            case Planet.Status.Enemy:
                ClickOthetPlanet();
                break;
            case Planet.Status.Neutral:
                ClickOthetPlanet();
                break;
            case Planet.Status.Player:
                ClickPlayerPlanet();
                break;
        }
    }

    void ClickPlayerPlanet()
    {
        if (!hit.transform.GetComponent<Planet>()._isSelected)
        {
            selectedPlanets.Add(hit.transform.gameObject.GetComponent<Planet>());
        }
        else
        {
            selectedPlanets.Remove(hit.transform.gameObject.GetComponent<Planet>());
        }

        hit.transform.GetComponent<Planet>().SelectPlanet();
    }

    void ClickOthetPlanet()
    {
        if (selectedPlanets.Count > 0)
        {
            lastClickedPlanet = hit.transform.gameObject;
            Debug.Log("ATTACK");
            ReducingShips();
            foreach (Planet planet in selectedPlanets)
            {
                planet.SelectPlanet();
            }
            selectedPlanets.Clear();
        }
        else
        {
            Debug.Log("SELECT YOUR PLANET");
        }
    }


    public GameObject GetPlanetForAttack()
    {
        return lastClickedPlanet;
    }

    public Vector2 GetLastAttackPosition()
    {
        return lastClickedPlanetPosition;
    }


}
