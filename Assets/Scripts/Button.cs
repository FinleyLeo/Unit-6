using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    Animator anim;

    GameObject activeObject;

    public GameObject door1, door2, door3, door4;

    public int buttonNum;

    static bool active1, active2;

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

            if (SceneManager.GetActiveScene().name == "Level 1")
            {
                if (buttonNum == 0)
                {
                    door1.GetComponent<Animator>().SetBool("Active", true);
                }

                else if (buttonNum == 1)
                {
                     door2.GetComponent<Animator>().SetBool("Active", true);
                }

                else if (buttonNum == 2)
                {
                    door3.GetComponent<Animator>().SetBool("Active", true);
                }

                else if (buttonNum == 3)
                {
                    active1 = true;

                    if (active1 && active2)
                    {
                        door4.GetComponent<Animator>().SetBool("Active", true);
                    }
                }

                else if (buttonNum == 4)
                {
                    active2 = true;

                    if (active1 && active2)
                    {
                        door4.GetComponent<Animator>().SetBool("Active", true);
                    }
                }
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

            if (SceneManager.GetActiveScene().name == "Level 1")
            {
                if (buttonNum == 0)
                {
                    door1.GetComponent<Animator>().SetBool("Active", false);
                }

                else if (buttonNum == 1)
                {
                    door2.GetComponent<Animator>().SetBool("Active", false);
                }

                else if (buttonNum == 2)
                {
                    door3.GetComponent<Animator>().SetBool("Active", false);
                }

                else if (buttonNum == 3)
                {
                    active1 = false;

                    if (!active1 || !active2)
                    {
                        door4.GetComponent<Animator>().SetBool("Active", false);
                    }
                }

                else if (buttonNum == 4)
                {
                    active2 = false;

                    if (!active1 || !active2)
                    {
                        door4.GetComponent<Animator>().SetBool("Active", false);
                    }
                }
            }
        }
    }
}
