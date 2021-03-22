using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Restart() 
    {
        SceneManager.LoadScene("Game");
    }
    public void ExitButton() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);

    }
}
