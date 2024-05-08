using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipVideo : MonoBehaviour
{
    public Button Button; 

    void Start()
    {
        Button.gameObject.SetActive(false); // Disable the button at the start
        StartCoroutine(EnableButtonAfterDelay());
    }

    IEnumerator EnableButtonAfterDelay()
    {
        yield return new WaitForSeconds(10.0f); // the delay time to 10 seconds
        Button.gameObject.SetActive(true); // Enable the button after the delay
    }
    public void skip() {

        SceneManager.LoadScene("stateOfPet");
    }
  

    
}
