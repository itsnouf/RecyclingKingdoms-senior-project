using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PetManager : MonoBehaviour
{
    public PetDatabase petDB;
    public SpriteRenderer petSpriteRenderer;
    private int selectedPetIndex = 0;
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedPetIndex"))
        {
            selectedPetIndex = 0;
        }
        else
        {
            Load();
        }
        UpdatePet(selectedPetIndex);
    }
    public void NextPet()
    {
        selectedPetIndex++;
        if (selectedPetIndex >= petDB.PetCount)
        {
            selectedPetIndex = 0;
        }
        UpdatePet(selectedPetIndex);
        Save();
    }

    public void PreviousPet()
    {
        selectedPetIndex--;
        if (selectedPetIndex < 0)
        {
            selectedPetIndex = petDB.PetCount - 1;
        }
        UpdatePet(selectedPetIndex);
        Save();
    }

    private void UpdatePet(int index)
    {
        Pet pet = petDB.GetPet(index);
        petSpriteRenderer.sprite = pet.petSprite;
    }

    private void Load()
    {
        selectedPetIndex = PlayerPrefs.GetInt("selectedPetIndex");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectedPetIndex", selectedPetIndex);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}  

