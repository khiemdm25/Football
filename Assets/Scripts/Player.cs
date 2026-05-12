using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float speedRunning = 10f;
    [SerializeField] private float speedRotation = 720f;

    private float blend = 0f;
    private int blendHash;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        blendHash = Animator.StringToHash("Blend");
    }

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        float h = 0;
        float v = 0;

        if (Input.GetKey(KeyCode.W)) v += 1;
        if (Input.GetKey(KeyCode.S)) v -= 1;
        if (Input.GetKey(KeyCode.A)) h -= 1;
        if (Input.GetKey(KeyCode.D)) h += 1;

        Vector3 direction = new Vector3(h, 0, v).normalized;
        rb.MovePosition(transform.position + direction * speedRunning * Time.deltaTime);

        float targetBlend = 0f;

        if (direction.magnitude > 0.1f)
        {
            targetBlend = 1f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speedRotation * Time.deltaTime);
        }

        blend = Mathf.Lerp(blend, targetBlend, Time.deltaTime * 5f);
        animator.SetFloat(blendHash, blend);
    }
}