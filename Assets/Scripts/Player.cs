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

    [Header("Mining")]
    [SerializeField]
    LayerMask _miningLayerMask;


    [Space]
    float _horizontalSensitivity = 50;
    float _verticalSensitivity = 50;

    float _maxCameraDistance = 10;
    float _cameraZoom;

    float _cameraRadius = 0.15f;

    void Awake()
    {
        _action = new InputSystem_Actions();

        _cameraZoom = _maxCameraDistance;
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

        Vector3 rotatedInput = _cameraPoint.right * input.x + _cameraPoint.forward * input.y;

        Vector3 moveDir = rotatedInput.normalized * 5 * Time.deltaTime;

        transform.Translate(moveDir);
    }

    void Look()
    {
        //move camera to player.
        _cameraPoint.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);

        Vector2 input = Vector2.zero;

        if(_action.Player.Aim.IsPressed())
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
        Vector3 origin = _cameraPoint.position;
        Vector3 direction = -_camera.forward;
        float maxDistance = _cameraZoom;
        RaycastHit hitInfo;

        bool raycastHit = Physics.SphereCast(origin: origin, radius: _cameraRadius, direction: direction, hitInfo: out hitInfo, maxDistance: maxDistance);

        _camera.transform.localPosition = new Vector3(0, 0, raycastHit ? -hitInfo.distance : -maxDistance);

        if(raycastHit)
        {
            _cameraZoom = hitInfo.distance;
        }

        //zoom
        _cameraZoom = Mathf.Clamp(_cameraZoom + _action.Player.Zoom.ReadValue<Vector2>().y, 0, _maxCameraDistance);
    }

    void Mine()
    {
        print("mine!");

        Collider[] hitInfo = Physics.OverlapSphere(
            position: transform.position + _cameraPoint.forward + Vector3.up,
            radius: 0.5f,
            layerMask: _miningLayerMask
            );

        foreach(Collider hit in hitInfo)
        {
            if(hit.TryGetComponent(out Ore ore))
            {
                print("found ore! - mining");
                ore.Mine();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _cameraPoint.forward + Vector3.up, 0.5f);
    }
}
