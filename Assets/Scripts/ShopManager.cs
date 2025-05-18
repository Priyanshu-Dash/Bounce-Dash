using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject[] characterPrefabs; // Assign Player, Player 1, Player 2 in Inspector
    public Transform characterPreviewSpot; // Assign an empty GameObject in the scene for preview
    private GameObject currentPreview;
    private int selectedCharacterIndex = 0;

    void Start()
    {
        // Load selected character from PlayerPrefs
        selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        ShowCharacterPreview(selectedCharacterIndex);
    }

    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacterIndex);
        PlayerPrefs.Save();
        ShowCharacterPreview(selectedCharacterIndex);

        // Switch the player in the game immediately
        if (GameManager.Instance != null)
            GameManager.Instance.SwitchPlayer(selectedCharacterIndex);
    }

    void ShowCharacterPreview(int index)
    {
        // Destroy previous preview
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
        // Instantiate new preview at preview spot
        if (characterPrefabs != null && characterPrefabs.Length > index)
        {
            currentPreview = Instantiate(characterPrefabs[index], characterPreviewSpot.position, Quaternion.identity, characterPreviewSpot);
            // Optionally disable scripts/colliders on preview
            foreach (var comp in currentPreview.GetComponents<MonoBehaviour>())
                comp.enabled = false;
            foreach (var col in currentPreview.GetComponents<Collider2D>())
                col.enabled = false;
        }
    }
} 