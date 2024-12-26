using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Jump : MonoBehaviour
{
    public static Jump Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); 
        }
    }

    public float JumpSpeed;
    public bool JumpFlag;
    Rigidbody rb;
    [SerializeField] private Vector3 localGravity;
    public Vector2 JumpInput;
    public float JumpY;
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
        JumpInput = context.ReadValue<Vector2>();
        if(!Attack.Instance.attackNow)
        {
            StartCoroutine(JumpAttack());
        }
    }
    public IEnumerator JumpAttack()
    {
        JumpY = JumpInput.y;
        while (MoveOJ.isWait > MoveOJ.time)
        {
            if (Attack.Instance.attackNow)
            {
                JumpY = 0;
                yield break;
            }
            yield return null;
        }
            if (JumpFlag && JumpY > 0.5)
            {
                rb.AddForce(Vector2.up * JumpSpeed, ForceMode.Impulse);
                JumpFlag = false;
            }
    }
}