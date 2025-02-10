using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public TextMeshProUGUI doorText;
    public GameObject text, transition;

    int amount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        amount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && amount == 2)
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 != 3)
            {
               StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex + 1, Color.black));
            }

            else
            {
                StartCoroutine(Transition(0, Color.black));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Big") || other.gameObject.CompareTag("Small"))
        {
            amount++;
            doorText.text = amount + "/2";

            if (amount == 2)
            {
                text.SetActive(true);
                text.GetComponent<Animator>().SetBool("active", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Big") || other.gameObject.CompareTag("Small"))
        {
            amount--;
            doorText.text = amount + "/2";

            text.GetComponent<Animator>().SetBool("active", false);
        }
    }

    IEnumerator Transition(int sceneName, Color color)
    {
        transition.GetComponent<Image>().color = color;
        transition.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
