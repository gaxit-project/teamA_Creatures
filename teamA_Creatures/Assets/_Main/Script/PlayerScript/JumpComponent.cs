using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    public static JumpComponent Instance;

    Animator animator;
    AudioSource jumpAudioSource;
    AudioSource landingAudioSource;
    Rigidbody rb;
    [SerializeField] private Vector3 localGravity = new Vector3(0, -9.81f, 0);

    public float jumpForce = 10f;
    public bool jumpFlag;
    public AudioClip jumpingSound;
    public AudioClip landingSound;

    private bool isLanding = false;

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

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        jumpAudioSource = gameObject.AddComponent<AudioSource>();
        landingAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        jumpFlag = true;
    }

    private void FixedUpdate()
    {
        if (!isLanding) SetLocalGravity();
    }

    private void SetLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }

    public void JumpVertical(float Jump)
    {
        if (Jump > 0.7f && jumpFlag)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpFlag = false;
            animator.SetTrigger("Jump");
            //jumpAudioSource.PlayOneShot(jumpingSound);
            AudioManager.GetInstance().PlaySE("playerMove", 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isLanding)
        {
            isLanding = true;
            jumpFlag = true;
            //landingAudioSource.PlayOneShot(landingSound);
            AudioManager.GetInstance().PlaySE("playerMove", 2);
            StartCoroutine(ResetLandingFlag());
        }
    }

    private IEnumerator ResetLandingFlag()
    {
        yield return new WaitForSeconds(0.1f);
        isLanding = false;
    }
}
