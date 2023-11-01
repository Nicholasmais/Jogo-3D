using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateEnemy : MonoBehaviour
{
    public GameObject inimigo;
    public float vel = 5f;
    public float minX = -20f;
    public float maxX = 20f;
    public float minY = -8f;
    public float maxY = 8f;
    public bool temporary = false;
    public bool stopGenerate = false;
    void Start()
    {
        InvokeRepeating("CriarInimigo", 0, vel);
    }

    void CriarInimigo()
    {
        if (this.stopGenerate)
        {
            return;
        }
        // Gere coordenadas X e Y aleatórias dentro dos intervalos especificados
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Crie uma posição com as coordenadas aleatórias
        Vector2 posAleatoria = new Vector2(8.0f, 4.0f);

        // Crie uma cópia do inimigo e defina sua posição
        GameObject copiaPrefab = Instantiate(inimigo, posAleatoria, Quaternion.identity);
        if (this.temporary)
        {
            Destroy(copiaPrefab, 3f);
        }
    }
}
