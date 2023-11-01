using UnityEngine;
public class GameManager : MonoBehaviour
{
    public GameObject inimigo;
    void Start()
    {
        InvokeRepeating("GeraInimigo", 0, 1f);
    }
    private void GeraInimigo()
    {
        float x = Random.Range(-6, 7);
        float drag = Random.Range(0, 2);
        var ninimigo = Instantiate(inimigo, new Vector3(x, 10, 0),
        Quaternion.identity);
        ninimigo.GetComponent<Rigidbody>().drag = drag;
    }
}
