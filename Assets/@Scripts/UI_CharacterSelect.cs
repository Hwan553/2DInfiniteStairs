using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_CharacterSelect : MonoBehaviour
{
    public Image _characterImage;
    public TMP_Text _characterNameText, _characterPriceText, _coinText;
    public Button leftButton, rightButton, buyButton, selectButton, startButton;

    private int currentIndex = 0;

    void Start()
    {
        currentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        UpdateUI();
    }

    void UpdateUI()
    {
        CharacterData currentCharacter = CharacterManager.Instance._characters[currentIndex];

       
        int playerCoins = PlayerPrefs.GetInt("KiwiScore", 0);

        if (currentCharacter._characterPrefab != null)
        {
            _characterImage.sprite = currentCharacter._characterPrefab.GetComponent<SpriteRenderer>().sprite;
        }

        _characterNameText.text = currentCharacter.characterName;
        _characterPriceText.text = currentCharacter.isPurchased ? "Owned" : $"Price: {currentCharacter.price}";
        _coinText.text = $"Coins: {playerCoins}";

        if (currentIndex == 0)
        {
            buyButton.gameObject.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(!currentCharacter.isPurchased);
        }

        selectButton.gameObject.SetActive(currentCharacter.isPurchased);
    }

    public void MoveLeft()
    {
        currentIndex = (currentIndex - 1 + CharacterManager.Instance._characters.Length) % CharacterManager.Instance._characters.Length;
        UpdateUI();
    }

    public void MoveRight()
    {
        currentIndex = (currentIndex + 1) % CharacterManager.Instance._characters.Length;
        UpdateUI();
    }

    public void BuyCharacter()
    {
        if (CharacterManager.Instance.BuyCharacter(currentIndex))
        {
            UpdateUI();
            _coinText.color = Color.white;
        }
        else
        {
            Debug.Log("Insufficient coins");
            _coinText.color = Color.red;
        }
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}


