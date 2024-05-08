using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPet : MonoBehaviour
{

    public PetDatabase petDB;
    public SpriteRenderer artworkSprite;

    private int selectedOption = 0;
    // Start is called before the first frame update
    void Start()
    {

        if (!PlayerPrefs.HasKey("selectedPetIndex"))
        {
            selectedOption = 0;


        }
        else
        {

            Load();
        }
        UpdateCharacter(selectedOption);
    }
    private void UpdateCharacter(int selectedOption)
    {


        Pet character = petDB.GetPet(selectedOption);
        artworkSprite.sprite = character.petSprite;


    }

    private void Load()
    {

        selectedOption = PlayerPrefs.GetInt("selectedPetIndex");

    }

    public void loadScene(string s)
    {

    
        SceneManager.LoadScene(s);
    }



}
