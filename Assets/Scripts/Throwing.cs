using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public bool isAiming, throwing;

    bool canPickup;

    Animator anim;
    PlayerController playerScript;
    
    public GameObject cam, crosshair, small, sitPoint, handPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        playerScript = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationLogic();

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
            small.transform.SetPositionAndRotation(sitPoint.transform.position, sitPoint.transform.rotation);

            small.GetComponent<Outline>().enabled = false;
            small.GetComponent<Rigidbody>().useGravity = false;
            small.GetComponent<Animator>().SetTrigger("Pick Up");

            playerScript.holdingSmall = true;
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
            small.transform.SetPositionAndRotation(handPos.transform.position, handPos.transform.rotation);

            crosshair.SetActive(true);
            StartCoroutine(AnimateWeightsTo(0, 1, 1, 4));
        }

        else if (Input.GetMouseButtonUp(1) && !throwing || !playerScript.isGrounded)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Small"))
        {
            other.gameObject.GetComponentInParent<Outline>().enabled = true;
            canPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Small"))
        {
            other.gameObject.GetComponentInParent<Outline>().enabled = false;
            canPickup = false;
        }
    }
}
