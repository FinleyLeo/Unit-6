using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public bool isAiming, throwing;

    bool canPickup;

    public float throwForce = 5f;

    Animator anim;
    PlayerController playerScript;
    SwitchCharacter switcher;

    public GameObject cam, crosshair, pickupText;
    public GameObject small, sitPoint, handPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        playerScript = GetComponent<PlayerController>();
        switcher = GameObject.Find("Switcher").GetComponent<SwitchCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationLogic();
        ThrowLogic();
    }

    void Throw()
    {
        small.GetComponent<Outline>().enabled = false;
        small.GetComponent<Rigidbody>().useGravity = true;

        small.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
        
        small.transform.parent = null;
        small.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    void ThrowLogic()
    {
        if (isAiming)
        {
            CameraAim();

            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(ThrowGrace());
                anim.SetTrigger("Throw");
            }
        }

        else
        {
            cam.GetComponent<CinemachineCamera>().Lens.FieldOfView = Mathf.Lerp(cam.GetComponent<CinemachineCamera>().Lens.FieldOfView, 60, 6f * Time.deltaTime);
            cam.GetComponent<CinemachineRotationComposer>().TargetOffset = Vector3.Lerp(cam.GetComponent<CinemachineRotationComposer>().TargetOffset, new Vector3(0f, 2.5f, 0f), 6f * Time.deltaTime);
            cam.GetComponent<CinemachineOrbitalFollow>().TargetOffset = Vector3.Lerp(cam.GetComponent<CinemachineOrbitalFollow>().TargetOffset, new Vector3(0f, 0f, 0f), 6f * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.F) && canPickup)
        {
            small.transform.parent = sitPoint.transform;

            small.GetComponent<Outline>().enabled = false;
            small.GetComponent<Rigidbody>().useGravity = false;
            small.GetComponent<Animator>().SetTrigger("Pick Up");
            small.GetComponent<Animator>().SetBool("Grounded", false);

            pickupText.GetComponent<Animator>().SetBool("active", false);

            playerScript.holdingSmall = true;
        }

        if (small.transform.parent != null && small.transform.parent.name == "SitPoint" && !isAiming)
        {
            small.transform.SetPositionAndRotation(sitPoint.transform.position, sitPoint.transform.rotation);
        }

        else if (small.transform.parent != null && isAiming)
        {
            small.transform.SetPositionAndRotation(handPos.transform.position, handPos.transform.rotation);
        }
    }

    void AnimationLogic()
    {
        if (Input.GetMouseButtonDown(1) && playerScript.isGrounded && playerScript.holdingSmall)
        {
            isAiming = true;
            playerScript.speed = 2;

            small.GetComponent<Animator>().SetTrigger("Thrown");
            small.transform.parent = handPos.transform;
            

            crosshair.SetActive(true);
            StartCoroutine(AnimateWeightsTo(0, 1, 1, 4));
        }

        else if (Input.GetMouseButtonUp(1) && playerScript.holdingSmall && !throwing)
        {
            isAiming = false;
            playerScript.speed = 4;

            small.GetComponent<Animator>().SetTrigger("Pick Up");
            small.transform.parent = sitPoint.transform;
            small.transform.SetPositionAndRotation(sitPoint.transform.position, sitPoint.transform.rotation);

            crosshair.SetActive(false);
            StartCoroutine(AnimateWeightsTo(1, 0, 1, 4));
        }
    }

    void CameraAim()
    {
        if (!throwing)
        {
            cam.GetComponent<CinemachineRotationComposer>().TargetOffset = Vector3.Lerp(cam.GetComponent<CinemachineRotationComposer>().TargetOffset, new Vector3(1f, 2.75f, 0f), 6f * Time.deltaTime);
            cam.GetComponent<CinemachineOrbitalFollow>().TargetOffset = Vector3.Lerp(cam.GetComponent<CinemachineOrbitalFollow>().TargetOffset, new Vector3(1f, 0.5f, 0f), 6f * Time.deltaTime);

            cam.GetComponent<CinemachineCamera>().Lens.FieldOfView = Mathf.Lerp(cam.GetComponent<CinemachineCamera>().Lens.FieldOfView, 40, 6f * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        }
    }

    public IEnumerator ThrowGrace()
    {
        throwing = true;
        yield return new WaitForSeconds(1f);
        StartCoroutine(AnimateWeightsTo(1, 0, 1, 2));
        crosshair.SetActive(false);

        playerScript.speed = 4;
        playerScript.holdingSmall = false;
        throwing = false;
        isAiming = false;
    }

    IEnumerator AnimateWeightsTo(int oldLayerIndex, int newLayerIndex, float goalValue, float transSpeed)
    {
        float newStartValue = anim.GetLayerWeight(newLayerIndex);
        float oldStartValue = anim.GetLayerWeight(oldLayerIndex);

        for (float i = 0f; i <= 1f; i += Time.deltaTime * transSpeed)
        {
            anim.SetLayerWeight(newLayerIndex, Mathf.Lerp(newStartValue, goalValue, i));
            anim.SetLayerWeight(oldLayerIndex, Mathf.Lerp(oldStartValue, 0f, i));
            yield return null;
        }

        anim.SetLayerWeight(newLayerIndex, goalValue);
        anim.SetLayerWeight(oldLayerIndex, 0f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup") && !switcher.smallSelected && !playerScript.holdingSmall)
        {
            other.gameObject.GetComponentInParent<Outline>().enabled = true;
            canPickup = true;

            pickupText.SetActive(true);
            pickupText.GetComponent<Animator>().SetBool("active", true);
        }

        else if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.GetComponentInParent<Outline>().enabled = false;
            canPickup = false;

            pickupText.GetComponent<Animator>().SetBool("active", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.GetComponentInParent<Outline>().enabled = false;
            canPickup = false;

            pickupText.GetComponent<Animator>().SetBool("active", false);
        }
    }
}
