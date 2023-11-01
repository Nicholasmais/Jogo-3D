using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ChangeSceneOnClick : MonoBehaviour, IPointerClickHandler
{
    public string sceneName; // Nome da cena para a qual você deseja fazer a transição
    public InputField inputField; // Referência ao InputField onde o jogador insere o nome

    public void OnPointerClick(PointerEventData eventData)
    {
        // Verifique se o botão esquerdo do mouse foi clicado
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Obtenha o nome do jogador a partir do InputField
            string playerName = inputField.text;

            // Salve o nome do jogador nos PlayerPrefs
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.SetFloat("Timer", 0f);

            PlayerPrefs.Save();

            // Use SceneManager para fazer a transição para a cena desejada
            SceneManager.LoadScene(sceneName);
        }
    }
}
