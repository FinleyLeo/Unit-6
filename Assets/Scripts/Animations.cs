using UnityEngine;

public class Animations : MonoBehaviour
{
    Animator anim;
    PlayerController controller;
    Rigidbody rb;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationLogic();
    }

    void AnimationLogic()
    {
        anim.SetFloat("Speed", Mathf.Abs(controller.dir.magnitude));
        anim.SetBool("Grounded", controller.isGrounded);

        if (rb.linearVelocity.y < -0.1f && !controller.isGrounded)
        {
            anim.SetBool("Falling", true);
        }

        else
        {
            anim.SetBool("Falling", false);
        }

        if (gameObject.CompareTag("Big"))
        {
            anim.SetFloat("AimDirX", controller.horiz);
            anim.SetFloat("AimDirZ", controller.vert);
        }
    }
}
