using Unity.Cinemachine;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    public bool smallSelected;

    public GameObject big, small, cam;

    PlayerController bigScript, smallScript;
    Animator bigAnim, smallAnim;
    CinemachineCamera cineCam;
    CinemachineRotationComposer camRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bigScript = big.GetComponent<PlayerController>();
        smallScript = small.GetComponent<PlayerController>();

        bigAnim = big.GetComponent<Animator>();
        smallAnim = small.GetComponent<Animator>();

        cineCam = cam.GetComponent<CinemachineCamera>();
        camRotation = cam.GetComponent<CinemachineRotationComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            smallSelected = !smallSelected;

            if (smallSelected && bigScript.isGrounded && !bigScript.delayFix && !bigScript.holdingSmall)
            {
                cineCam.Follow = small.transform;
                cineCam.Lens.FieldOfView = 60;
                camRotation.TargetOffset = new Vector3(0, 1, 0);

                bigAnim.SetFloat("Speed", 0);
            }

            else if (!smallSelected && smallScript.isGrounded && !smallScript.delayFix)
            {
                cineCam.Follow = big.transform;
                cineCam.Lens.FieldOfView = 60;
                camRotation.TargetOffset = new Vector3(0, 2.5f, 0);

                smallAnim.SetFloat("Speed", 0);
            }
        }
    }
}
