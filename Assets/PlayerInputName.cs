using UnityEngine;
using UnityEngine.UI;

public class PlayerInputName : MonoBehaviour
{
    public InputField inputField; // Arraste o InputField do Unity aqui
    public Text playerNameText; // Arraste o Text do Unity onde você deseja exibir o nome do jogador

    public void SetPlayerName()
    {
        string playerName = inputField.text;
        playerNameText.text = "Nome do Jogador: " + playerName;
    }
}
