using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject planet;
    [SerializeField] private GameObject quad;
    private float _planetRadius;
    [SerializeField] private int _numberOfSpawn;
    private Vector2 _minScreenBounds;
    private Vector2 _maxScreenBounds;
    private List<Vector2> _positionsOfPlanets;
    [SerializeField] private GameObject aStarObj;
    [SerializeField] private LayerMask _layerMask;
    public List<GameObject> planetList;
    void Awake()
    {
        float scl = planet.transform.localScale.x;
        _planetRadius = planet.GetComponent<CircleCollider2D>().radius * scl;
        MeshCollider mc = quad.GetComponent<MeshCollider>();
        _maxScreenBounds.y = mc.bounds.max.y;
        _maxScreenBounds.x = mc.bounds.max.x;
        _minScreenBounds.y = mc.bounds.min.y;
        _minScreenBounds.x = mc.bounds.min.x;
        SpawnPlanets();
    }
    void Start()
    {
        //aStarObj.SetActive(true);
    }
    void SpawnPlanets()
    {
        float screenX, screenY;
        Vector2 spawnPos;

        for (int i = 0; i < _numberOfSpawn; i++)
        {
            screenX = Random.Range(_minScreenBounds.x, _maxScreenBounds.x);
            screenY = Random.Range(_minScreenBounds.y, _maxScreenBounds.y);
            spawnPos = new Vector2(screenX, screenY);

            GameObject newPlanet = (GameObject)Instantiate(planet, CheckOverlap(spawnPos), planet.transform.rotation);

            if (i < 2)
            {
                if (i == 0)
                    newPlanet.GetComponent<Planet>().CreatePlayerPlanet();
                else
                    newPlanet.GetComponent<Planet>().CreateEnemyPlanet();
            }
            planetList.Add(newPlanet);
        }
    }
    Vector2 CheckOverlap(Vector2 pos)
    {
        Vector2 newSpawnPosition = pos;
        bool check = false;

        while (!check)
        {
            Collider2D[] collider2D = Physics2D.OverlapCircleAll(newSpawnPosition, _planetRadius * 4f, _layerMask);
            
            if (collider2D.Length > 0)
            {
                newSpawnPosition = new Vector2(Random.Range(_minScreenBounds.x, _maxScreenBounds.x),
                                               Random.Range(_minScreenBounds.y, _maxScreenBounds.y));
            }
            else
            {
                check = true;
                break;
            }
        }
        return newSpawnPosition;
    }
}
