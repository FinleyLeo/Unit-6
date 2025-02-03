using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public bool isAiming, throwing;

    float mainWeight, aimWeight;

    Animator anim;
    PlayerController playerScript;
    
    public GameObject upperBody, cam;

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
                StartCoroutine(throwGrace());
                StartCoroutine(SpeedDelay(6));
                anim.SetTrigger("Throw");
            }
        }

        else
        {
            cam.GetComponent<CinemachineRotationComposer>().TargetOffset = new Vector3(0, 2.5f, 0f);
            cam.GetComponent<CinemachineOrbitalFollow>().TargetOffset = new Vector3(0f, 0f, 0f);

            cam.GetComponent<CinemachineOrbitalFollow>().Orbits.Center.Radius = 6;
        }
    }

    void AnimationLogic()
    {
        if (Input.GetMouseButtonDown(1) && playerScript.isGrounded)
        {
            isAiming = true;
            StartCoroutine(AnimateWeightsTo(0, 1, 1, 4));
        }

        else if (Input.GetMouseButtonUp(1) && !throwing || !playerScript.isGrounded)
        {
            isAiming = false;
            StartCoroutine(AnimateWeightsTo(1, 0, 1, 4));
        }
    }

    void CameraAim()
    {
        if (!throwing)
        {
            cam.GetComponent<CinemachineRotationComposer>().TargetOffset = new Vector3(1f, 2.75f, 0f);
            cam.GetComponent<CinemachineOrbitalFollow>().TargetOffset = new Vector3(1f, 0.5f, 0f);

            cam.GetComponent<CinemachineOrbitalFollow>().Orbits.Center.Radius = 4;

            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

        }
    }

    public IEnumerator throwGrace()
    {
        throwing = true;
        yield return new WaitForSeconds(0.5f);
        throwing = false;
        isAiming = false;
    }

    IEnumerator SpeedDelay(float multi)
    {
        playerScript.speed /= multi;
        yield return new WaitForSeconds(1f);
        StartCoroutine(AnimateWeightsTo(1, 0, 1, 2));
        playerScript.speed *= multi;
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
}
