using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Jump : MonoBehaviour
{
    public float JumpSpeed;
    private bool JumpFlag;
    Rigidbody rb;
    [SerializeField] private Vector3 localGravity;
    private Vector2 JumpY;

    public GameObject moveObject;
    private Move MoveOJ;
    void Start()
    {
        MoveOJ = moveObject.GetComponent<Move>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        JumpFlag = true;
    }

    private void FixedUpdate()
    {
        SetLocalGravity();
    }


    private void SetLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            JumpFlag = true;
        }
    }
    /// <summary>
    /// inputaction‚©‚çŽæ‚Á‚Ä‚«‚Ä‚¢‚é
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        JumpY = context.ReadValue<Vector2>();
        if(!Move.attackNow)
        {
            StartCoroutine(JumpAttack());
        }
    }
    public IEnumerator JumpAttack()
    {

        while (MoveOJ.isWait > MoveOJ.time)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                JumpY.y = 0;
                yield break;
            }
            yield return null;
        }
            if (JumpFlag && JumpY.y > 0.5)
            {
                rb.AddForce(Vector2.up * JumpSpeed, ForceMode.Impulse);
                JumpFlag = false;
            }
    }
}