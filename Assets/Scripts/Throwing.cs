using UnityEngine;

public class Throwing : MonoBehaviour
{
    public bool isAiming;

    Animator anim;

    public GameObject upperBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationLogic();

        if (isAiming)
        {
            CameraAim();
        }
    }

    void AnimationLogic()
    {
        if (Input.GetMouseButton(0))
        {
            isAiming = true;
        }

        else
        {
            isAiming = false;
        }

        if (isAiming)
        {
            anim.SetLayerWeight(0, 0);
            anim.SetLayerWeight(1, 1);
        }

        else
        {
            anim.SetLayerWeight(0, 1);
            anim.SetLayerWeight(1, 0);
            // upperBody.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void CameraAim()
    {
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        // upperBody.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, 0, 0);
    }
}
