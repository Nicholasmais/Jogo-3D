using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootPlayer : MonoBehaviour
{
    public float speed = 10f;
    public float minDistance = 15f;
    public GameObject bulletPrefab; // Prefab da bala

    private bool isAttacking = false;
    public Transform target;
    private Rigidbody objeto;
    private float timer = 0f;
    private float timeBetweenShots = 0.5f; // Intervalo entre os tiros

    private void Start()
    {
        objeto = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < minDistance)
            {
                isAttacking = true;
                if (timer >= timeBetweenShots)
                {
                    Attack(target.position);
                    timer = 0f; // Reinicie o temporizador
                }
                else
                {
                    timer += Time.deltaTime; // Atualize o temporizador
                }
            }
            else
            {
                isAttacking = false;
                objeto.GetComponent<Animator>().SetBool("attack", false);
            }
        }
    }

    void Attack(Vector3 targetPosition)
    {
        objeto.GetComponent<Animator>().SetBool("attack", true);

        objeto.transform.LookAt(targetPosition);

        objeto.transform.rotation = Quaternion.Euler(0, objeto.transform.rotation.eulerAngles.y, 0);

        // Crie a bala (ou outro objeto) e configure sua direção
        CreateBullet(targetPosition);
    }

    void CreateBullet(Vector3 targetPosition)
    {
        // Crie uma instância da bala
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Calcule a direção para o jogador
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Atribua velocidade à bala para que ela se mova na direção correta
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = direction * speed;
        bullet.transform.localPosition += new Vector3(0, 1.5f, 0);

        Destroy(bullet, 3f);
    }
}
