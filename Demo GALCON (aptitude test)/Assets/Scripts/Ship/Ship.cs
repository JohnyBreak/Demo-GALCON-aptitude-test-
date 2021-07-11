using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Ship : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Rigidbody2D rb;
    private Seeker seeker;
    private Vector2 movePosition;
    private int _currentWaypoint;
    private Path path;
    private bool _reachedEndOfPath;
    private GameObject _planetForAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        movePosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>().GetLastAttackPosition();
        _planetForAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>().GetPlanetForAttack();
        Move();
    }
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {

        Vector2 forceDirection = new Vector2(_planetForAttack.transform.position.x - transform.position.x, _planetForAttack.transform.position.y - transform.position.y);
        rb.AddForce(forceDirection);
        //transform.position = Vector2.MoveTowards(transform.position, _planetForAttack.transform.position, _speed * Time.deltaTime);

        //seeker.StartPath(transform.position, _planetForAttack.transform.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            _currentWaypoint = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Planet>() && collision.gameObject == _planetForAttack)
        {
            if (collision.gameObject.GetComponent<Planet>().planetStatus != Planet.Status.Player)
                collision.gameObject.GetComponent<Planet>().TakeDamage();
            else
                collision.gameObject.GetComponent<Planet>().TakeComrads();

            Destroy(this.gameObject);
        }
    }

    public void EnemyAttack(GameObject planet)
    {
        _planetForAttack = planet;

    }

}
