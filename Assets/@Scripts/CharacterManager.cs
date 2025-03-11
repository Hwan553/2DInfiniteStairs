using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{

    public static CharacterManager Instance { get; private set; }
    public CharacterData[] _characters = new CharacterData[4];
    private int playerCoins;
    private int selectedCharacterIndex = 0;
    private GameObject spawnedCharacter; // ������ ĳ���͸� �����ϴ� ���� �߰�

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCharacterData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadCharacterData()
    {
        playerCoins = PlayerPrefs.GetInt("Kiwi", 600);

        for (int i = 0; i < _characters.Length; i++)
        {
            if (PlayerPrefs.GetInt(_characters[i].characterName, 0) == 1)
            {
                _characters[i].isPurchased = true;
            }
        }

        // ù ��° ĳ���ʹ� �⺻������ ���ŵ� ������ ����
        if (!_characters[0].isPurchased)
        {
            _characters[0].isPurchased = true;
            PlayerPrefs.SetInt(_characters[0].characterName, 0);
        }
    }

    // ĳ���� ���� �Լ�
    public bool BuyCharacter(int index)
    {
        if (index == 0) return false; // ù ��° ĳ���ʹ� ���� �Ұ�

        if (!_characters[index].isPurchased && playerCoins >= _characters[index].price)
        {
            playerCoins -= _characters[index].price;
            PlayerPrefs.SetInt("Kiwi", playerCoins);
            _characters[index].isPurchased = true;
            PlayerPrefs.SetInt(_characters[index].characterName, 0);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    // ���õ� ĳ���͸� ��ȯ�ϴ� �Լ�
    public CharacterData GetSelectedCharacter()
    {
        return _characters[PlayerPrefs.GetInt("SelectedCharacterIndex", 0)];
    }

    // ���õ� ĳ���� �������� ��ȯ�ϴ� �Լ�
    public GameObject GetSelectedCharacterPrefab()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        if (selectedIndex < 0 || selectedIndex >= _characters.Length)
        {
            return null;
        }

        return _characters[selectedIndex]._characterPrefab;
    }

    // ������ ĳ���͸� �����ϴ� �Լ�
    public void SetSpawnedCharacter(GameObject character)
    {
        spawnedCharacter = character;
    }

    
    public void MovePlayer()
    {
        if (spawnedCharacter != null)
        {
            
            spawnedCharacter.GetComponent<PlayerController>().PlayerMove();
        }
        
    }

    public void TurnPlayer()
    {
        if (spawnedCharacter != null)
        {
            spawnedCharacter.GetComponent<PlayerController>().PlayerTurn();
        }

    }

    public void ReStartPlayer()
    {
        if(spawnedCharacter != null)
        {
            spawnedCharacter.GetComponent<PlayerController>().RestartButton();
        }
    }

    // �÷��̾��� ���� ���¸� ��ȯ�ϴ� �Լ�
    public int GetPlayerCoins()
    {
        return playerCoins;
    }

    // ĳ���� ���� �Լ�
    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= _characters.Length) return;

        selectedCharacterIndex = index;
        PlayerPrefs.SetInt("SelectedCharacterIndex", index);
        PlayerPrefs.Save();
    }




}


