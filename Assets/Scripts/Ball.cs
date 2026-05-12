using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball currentBall;

    public bool canKick = false;

    [SerializeField] private Rigidbody rb;

    private CameraManager cam;

    [Header("Kick")]
    [SerializeField] private float maxForce = 40f;
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float chargeSpeed = 20f;
    [SerializeField] private float upForce = 4f;

    private bool isCharging = false;
    private float currentForce = 0f;
    public static bool isGoal = false;

    [Header("Effect")]
    public GameObject goalEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        cam = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        if (!canKick || currentBall != this) return;

        ChargeKick();
        UpdatePowerUI();
    }

    void ChargeKick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            currentForce += chargeSpeed * Time.deltaTime;
            currentForce = Mathf.Clamp(currentForce, minForce, maxForce);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isCharging)
            {
                KickToNearestGoal(currentForce);
            }

            currentForce = 0f;
            isCharging = false;

            if (GameManager.Instance.powerSlider != null)
            {
                GameManager.Instance.powerSlider.value = 0;
            }
        }
    }

    public void KickToNearestGoal(float force)
    {
        Transform nearestGoal = FindNearestGoal();

        if (nearestGoal == null)return;

        Vector3 dir = (nearestGoal.position - transform.position).normalized;
        dir.y += 0.15f;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(dir * force + Vector3.up * upForce, ForceMode.Impulse);

        if (cam != null)
        {
            cam.PlayKickCamera(transform);
        }
    }

    void UpdatePowerUI()
    {
        if (GameManager.Instance.powerSlider == null) return;

        GameManager.Instance.powerSlider.maxValue = maxForce;
        GameManager.Instance.powerSlider.value = currentForce;
    }

    Transform FindNearestGoal()
    {
        float minDistance = Mathf.Infinity;
        Transform nearest = null;
        GameObject[] goals =GameObject.FindGameObjectsWithTag("Goal");

        foreach (GameObject goal in goals)
        {
            float dist = Vector3.Distance(transform.position, goal.transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = goal.transform;
            }
        }

        return nearest;
    }

    public void AutoKick()
    {
        KickToNearestGoal(maxForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canKick = true;
            currentBall = this;
            GameManager.Instance.ShowKickButton(true);
        }

        if (other.CompareTag("Goal"))
        {
            isGoal = true;

            if (goalEffect != null)
            {
                GameObject effect = Instantiate(goalEffect, other.transform.position, Quaternion.identity);
                Destroy(effect, 3f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canKick = false;

            if (currentBall == this)
            {
                currentBall = null;
            }

            GameManager.Instance.ShowKickButton(false);
        }
    }
}