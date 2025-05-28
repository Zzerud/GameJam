using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public static ThirdPersonController instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform cameraPivotPitch;
    [SerializeField] private Transform cameraTransform;
    private CharacterController cc;
    public bool isEnabledMove = true;

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

    [Range(.1f, 2.0f)] [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 2;
    [SerializeField] private float minPitch = -30f, maxPitch = 60f;
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
        Vector3 targetPos = transform.position + pivotOffset;
        cameraPivot.position = Vector3.Lerp(cameraPivot.position, targetPos, Time.deltaTime * collisionSmoothSpeed);

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

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
        if (Physics.SphereCast(pivotPos, 0.2f, dir, out hit, maxCheckDist))
        {
            float hitDist = hit.distance;
            currentDistance = Mathf.Lerp(currentDistance, Mathf.Clamp(hitDist, minDistance, maxDistance), Time.deltaTime * collisionSmoothSpeed);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, defaultDistance, Time.deltaTime * collisionSmoothSpeed);
        }

        // установка позиции камеры
        Vector3 finalPos = cameraPivotPitch.TransformPoint(new Vector3(0f, 0f, -currentDistance));
        cameraTransform.position = finalPos;
        cameraTransform.LookAt(cameraPivot.position + Vector3.up * (pivotOffset.y * 0.9f));
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            Vector3 moveDir = cameraPivot.forward * v + cameraPivot.right * h;
            moveDir.y = 0f;
            moveDir.Normalize();

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            cc.Move(moveDir * speed * Time.deltaTime);
        }

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (velocity.y < 0)
                velocity.y = -2f;

        }
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        isGrounded = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, cc.height / 2 + 0.1f, LayerMask.GetMask("Default"));
    }
}
