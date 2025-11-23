using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputSystem_Actions _action;

    [Header("Camera")]
    [SerializeField]
    Transform _cameraPoint;
    [SerializeField]
    Transform _cameraPitch;
    [SerializeField]
    Transform _camera;

    float _pitchValue = 45;

    [Space]
    float _horizontalSensitivity = 50;
    float _verticalSensitivity = 50;

    void Awake()
    {
        _action = new InputSystem_Actions();
    }

    void OnEnable()
    {
        _action.Enable();

        _action.Player.Attack.performed += (InputAction.CallbackContext context) => { Mine(); };
    }

    void OnDisable()
    {
        _action.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Move();

        Look();
    }

    void Move()
    {
        Vector2 input = _action.Player.Move.ReadValue<Vector2>();

        // Vector3 forward = new Vector3(_cameraHolder.transform.forward.x, 0, _cameraHolder.transform.forward.z);
        // Vector3 right = new Vector3(_cameraHolder.transform.right.x, 0, _cameraHolder.transform.right.z);

        // Vector3 rotatedInput = right * input.x + forward * input.y;
        Vector3 rotatedInput = _cameraPoint.right * input.x + _cameraPoint.forward * input.y;

        Vector3 moveDir = rotatedInput.normalized * 5 * Time.deltaTime;

        transform.Translate(moveDir);
    }

    void Look()
    {
        //move camera to player.
        _cameraPoint.position = transform.position;

        //mouse input
        Vector2 input = Vector2.zero;

        if(_action.Player.Aim.ReadValue<float>() > 0)
        {
            input = _action.Player.Look.ReadValue<Vector2>();
        }

        //Yaw
        _cameraPoint.Rotate(Vector3.up * input.x * _horizontalSensitivity * Time.deltaTime);

        //Pitch
        _pitchValue = Mathf.Clamp(_pitchValue + -input.y * _verticalSensitivity * Time.deltaTime, -90, 90);

        Vector3 euler = _cameraPitch.eulerAngles;
        euler.x = _pitchValue;
        _cameraPitch.eulerAngles = euler;

        //raycast
        Vector3 origin = Vector3.zero;
        Vector3 direction = Vector3.zero;
        float maxDistance = 10;
        RaycastHit hitInfo;

        bool raycastHit = Physics.Raycast(origin: origin, direction: direction, hitInfo: out hitInfo, maxDistance: maxDistance);

        _camera.transform.localPosition = new Vector3(0, 0, raycastHit ? -hitInfo.point.z : -maxDistance);
    }

    void Mine()
    {
        print("mine!");
    }
}
