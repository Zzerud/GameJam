using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public static ThirdPersonController instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform cameraPivotPitch;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private AudioSource sounds;
    [SerializeField] private AudioClip whistleSound;
    public Animator animator;
    private CharacterController cc;
    public bool isEnabledMove = true;
    public bool isEnabledRun = true;
    public bool isEnabledJump = true;

    [Header("Camera Offset")]
    public Vector3 pivotOffset = new Vector3(0f, 1.8f, 0f);

    [Header("Camera Distance")]
    [SerializeField] private float defaultDistance = 4f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 6f;
    [SerializeField] private float collisionSmoothSpeed = 10f;
    private float currentDistance;

    [Header("Movement Settings")]
    [Range(.1f, 5.0f)][SerializeField] private float walkSpeed = 4f;
    [Range(2.1f, 9.0f)][SerializeField] private float runSpeed = 6f;

    [Range(.1f, 2.0f)][SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 2;
    [SerializeField] private float minPitch = -30f, maxPitch = 60f;
    [SerializeField] private LayerMask ignoreCameraCollision;

    [Header("Whistle")]
    public List<EnemyNPCBehaviour> enemies = new List<EnemyNPCBehaviour>();
    private float pitch = 0;


    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isEnabledMove)
        {
            HandleMovement();
            GroundCheck();
            if (Input.GetKeyDown(KeyCode.Q))
                WhistleAt();
        }
    }
    private void LateUpdate()
    {
        if (isEnabledMove)
        {
            HandleMouse();
            HandleCameraCollision();
        }
    }
    private void HandleMouse()
    {
        if (Time.timeScale == 0) return;

        Vector3 targetPos = transform.position + pivotOffset;
        cameraPivot.position = Vector3.Lerp(cameraPivot.position, targetPos, Time.deltaTime * collisionSmoothSpeed);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPivot.Rotate(Vector3.up * mouseX, Space.World);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cameraPivotPitch.localEulerAngles = Vector3.right * pitch;
    }
    void HandleCameraCollision()
    {
        Vector3 pivotPos = cameraPivot.position;
        Vector3 desiredCamPos = cameraPivotPitch.TransformPoint(new Vector3(0f, 0f, -defaultDistance));
        Vector3 dir = (desiredCamPos - pivotPos).normalized;
        float maxCheckDist = defaultDistance;

        RaycastHit hit;
        if (Physics.SphereCast(pivotPos, 0.2f, dir, out hit, maxCheckDist, ignoreCameraCollision))
        {
            float hitDist = hit.distance;
            currentDistance = Mathf.Lerp(currentDistance, Mathf.Clamp(hitDist, minDistance, maxDistance), Time.deltaTime * collisionSmoothSpeed);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, defaultDistance, Time.deltaTime * collisionSmoothSpeed);
        }

        Vector3 finalPos = cameraPivotPitch.TransformPoint(new Vector3(0f, 0f, -currentDistance));
        cameraTransform.position = finalPos;
        cameraTransform.LookAt(cameraPivot.position + Vector3.up * (pivotOffset.y * 0.9f));
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        bool isMove = h != 0 || v != 0;

        if (isMove)
        {
            if (inputDir.magnitude >= 0.1f)
            {
                Vector3 moveDir = cameraPivot.forward * v + cameraPivot.right * h;
                moveDir.y = 0f;
                moveDir.Normalize();
                bool isRunning = Input.GetKey(KeyCode.LeftShift) && isEnabledRun;

                animator.SetBool("Walking", true);
                animator.SetBool("Running", isRunning);

                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                float speed = isRunning ? runSpeed : walkSpeed;
                cc.Move(moveDir * speed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetBool("Running", false);
            animator.SetBool("Walking", false);
        }

        if (isGrounded && isEnabledJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetTrigger("Jump");
            }
            if (velocity.y < 0)
                velocity.y = -2f;

        }
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
    private void GroundCheck()
    {
        RaycastHit hit;
        isGrounded = Physics.SphereCast(transform.position, 0.1f, Vector3.down, out hit, .2f, LayerMask.GetMask("Ground"));
    }

    public void WhistleAt()
    {
        StartCoroutine(WhistleCoroutine());
    }
    private IEnumerator WhistleCoroutine()
    {
        CanvasControllerChapter1.instance.interactQText.DOFade(0, 0.2f);
        EnemyNPCBehaviour closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        sounds.PlayOneShot(whistleSound);

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = enemy;
            }
        }
        yield return new WaitForSeconds(0.9f);
        if (closestEnemy != null)
            closestEnemy.Investigate(transform.position);
    }

    public void StateCharacter(bool active)
    {
        isEnabledMove = active;
        cc.enabled = active;
        animator.SetBool("Running", active);
        animator.SetBool("Walking", active);
    }
}
