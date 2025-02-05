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
        anim.SetFloat("YVel", rb.linearVelocity.y);
        anim.SetBool("Grounded", controller.isGrounded);

        if (gameObject.CompareTag("Big"))
        {
            anim.SetFloat("AimDirX", controller.horiz);
            anim.SetFloat("AimDirZ", controller.vert);
        }
    }
}
