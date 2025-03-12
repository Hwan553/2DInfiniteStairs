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
    private GameObject spawnedCharacter;

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
        playerCoins = PlayerPrefs.GetInt("KiwiScore", 600);

        for (int i = 0; i < _characters.Length; i++)
        {
            if (PlayerPrefs.GetInt(_characters[i].characterName, 0) == 1)
            {
                _characters[i].isPurchased = true;
            }
        }

        if (!_characters[0].isPurchased)
        {
            _characters[0].isPurchased = true;
            PlayerPrefs.SetInt(_characters[0].characterName, 1);
        }
    }

    // 캐릭터 구매 함수
    public bool BuyCharacter(int index)
    {
        if (index == 0) return false;

        if (!_characters[index].isPurchased && playerCoins >= _characters[index].price)
        {
            playerCoins -= _characters[index].price;
            PlayerPrefs.SetInt("KiwiScore", playerCoins);
            PlayerPrefs.SetInt(_characters[index].characterName, 1);
            PlayerPrefs.Save();

            LoadCharacterData();
            return true;
        }
        return false;
    }

    // 선택된 캐릭터를 반환하는 함수
    public CharacterData GetSelectedCharacter()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        if (selectedIndex < 0 || selectedIndex >= _characters.Length)
            return null;

        _characters[selectedIndex].isPurchased = PlayerPrefs.GetInt(_characters[selectedIndex].characterName, 0) == 1;

        return _characters[selectedIndex];
    }

    // 선택된 캐릭터 프리팹을 반환하는 함수
    public GameObject GetSelectedCharacterPrefab()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        if (selectedIndex < 0 || selectedIndex >= _characters.Length)
        {
            return null;
        }

        return _characters[selectedIndex]._characterPrefab;
    }

    // 생성된 캐릭터를 저장하는 함수
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
        if (spawnedCharacter != null)
        {
            spawnedCharacter.GetComponent<PlayerController>().RestartButton();
        }
    }

    // 플레이어의 코인 상태를 반환하는 함수
    public int GetPlayerCoins()
    {
        return playerCoins;
    }

    // 캐릭터 선택 함수
    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= _characters.Length) return;

        selectedCharacterIndex = index;
        PlayerPrefs.SetInt("SelectedCharacterIndex", index);
        PlayerPrefs.Save();
    }

}


