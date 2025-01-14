using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    public static JumpComponent Instance;

    Animator animator;
    AudioSource audioSource;
    Rigidbody rb;
    [SerializeField] private Vector3 localGravity;

    public float jumpForce = 10f;
    public bool jumpFlag;
    public AudioClip jumpingSound;
    public AudioClip landingSound;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        rb.useGravity = false;
        jumpFlag = true;
    }


    private void FixedUpdate()
    {
        SetLocalGravity();
    }
    private void SetLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }


    public void JumpVertical(float Jump)
    {
        if (Jump > 0.7 && jumpFlag)
        {
            rb.AddForce(Vector2.up * jumpForce ,ForceMode.Impulse);
            jumpFlag = false;
            animator.SetTrigger("Jump");
            audioSource.PlayOneShot(jumpingSound);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpFlag = true;
            audioSource.PlayOneShot(landingSound);
        }
    }
}
