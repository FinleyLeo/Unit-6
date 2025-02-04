using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f, jumpHeight = 5f, rayDistance = 0.5f;
    float smoothVel, turnSpeed = 0.1f;

    float horiz, vert;

    public bool isGrounded, delayFix, holdingSmall;

    public LayerMask groundLayer;

    Vector3 dir, movedir;

    Rigidbody rb;
    Animator anim;
    Throwing throwScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        throwScript = GetComponent<Throwing>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        StartCoroutine(Jump());
        GroundCheck();
        Animations();
    }

    void Movement()
    {
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
        dir = new Vector3(horiz, 0, vert).normalized;

        if (dir.magnitude >= 0.1f)
        {
            if (gameObject.CompareTag("Big"))
            {
                if (!throwScript.throwing)
                {
                    float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, turnSpeed);

                    transform.rotation = Quaternion.Euler(0, angle, 0);

                    movedir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    rb.linearVelocity = new Vector3(movedir.normalized.x * speed, rb.linearVelocity.y, movedir.normalized.z * speed);
                }
            }

            else
            {
                float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, turnSpeed);

                transform.rotation = Quaternion.Euler(0, angle, 0);

                movedir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                rb.linearVelocity = new Vector3(movedir.normalized.x * speed, rb.linearVelocity.y, movedir.normalized.z * speed);
            }
        }
    }

    IEnumerator Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !delayFix)
        {
            if (gameObject.CompareTag("Big"))
            {
                if (!throwScript.isAiming)
                {
                    anim.SetTrigger("Jump");
                    delayFix = true;

                    StartCoroutine(SpeedDelay(4));

                    yield return new WaitForSeconds(0.35f);

                    rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

                    delayFix = false;
                }
            }

            else
            {
                anim.SetTrigger("Jump");
                delayFix = true;

                StartCoroutine(SpeedDelay(4));

                yield return new WaitForSeconds(0.35f);

                delayFix = false;

                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
            
        }
    }

    IEnumerator SpeedDelay(float multi)
    {
        speed /= multi;
        yield return new WaitForSeconds(0.35f);
        speed *= multi;
    }

    void Animations()
    {
        anim.SetFloat("Speed", Mathf.Abs(dir.magnitude));
        anim.SetFloat("YVel", rb.linearVelocity.y);
        anim.SetBool("Grounded", isGrounded);

        if (gameObject.CompareTag("Big"))
        {
            anim.SetFloat("AimDirX", horiz);
            anim.SetFloat("AimDirZ", vert);
        }
    }

    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer))
        {
            Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.green);

            if (!isGrounded)
            {
                StartCoroutine(SpeedDelay(2));
            }

            isGrounded = true;
        }

        else
        {
            Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.white);

            isGrounded = false;
        }
    }

    
}
