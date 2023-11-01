using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject jogador;
    private Vector3 distancia;

    void Start()
    {
        if (jogador != null)
        {
            distancia = transform.position - jogador.transform.position;
        }
    }

    void LateUpdate()
    {
        if (jogador != null)
        {
            transform.position = jogador.transform.position + distancia;
        }
    }
}
