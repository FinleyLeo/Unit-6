using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    Animator anim;

    GameObject activeObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Big") || other.gameObject.CompareTag("Crate"))
        {
            anim.SetBool("Pressed", true);

            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                activeObject = GameObject.Find("Bridge");
                activeObject.GetComponent<Animator>().SetBool("Active", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Big") || other.gameObject.CompareTag("Crate"))
        {
            anim.SetBool("Pressed", false);

            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                activeObject = GameObject.Find("Bridge");
                activeObject.GetComponent<Animator>().SetBool("Active", false);
            }
        }
    }
}
