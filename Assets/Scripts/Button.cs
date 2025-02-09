using UnityEditor.AnimatedValues;
using UnityEngine;

public class Button : MonoBehaviour
{
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Big") || other.gameObject.CompareTag("Crate"))
        {
            anim.SetBool("Pressed", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Big") || other.gameObject.CompareTag("Crate"))
        {
            anim.SetBool("Pressed", false);
        }
    }
}
