using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject small, big;

    public GameObject left, right;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SpawnCharacters()
    {
        small.transform.position = left.transform.position + new Vector3(0, 3, 0);
        big.transform.position = right.transform.position + new Vector3(0, 3, 0);
    }

    public void Play()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
