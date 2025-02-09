using UnityEngine;

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
}
