using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public float speed = 2.5f;
    public float minDistance = 5f;
    private bool isAttacking = false;

    public Transform target;
    public Rigidbody objeto;
    private void Start()
    {
        objeto = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {        
        if (target != null && !this.isAttacking)
        {
            if (Vector3.Distance(transform.position, target.position) < minDistance)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                direction.y = 0;
                transform.Translate(direction * speed * Time.deltaTime, Space.World);
                transform.LookAt(target);
                objeto.GetComponent<Animator>().SetBool("run", true);
            }
            else
            {
                objeto.GetComponent<Animator>().SetBool("run", false);
            }            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.isAttacking = true;
            objeto.GetComponent<Animator>().SetBool("run", false);
            objeto.GetComponent<Animator>().SetBool("attacking", true);            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        this.isAttacking = false;
        objeto.GetComponent<Animator>().SetBool("attacking", false);
    }
}
