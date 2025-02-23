using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Main;

    [SerializeField]
    private CinemachineCamera playerCamera;

    [SerializeField]
    private CinemachineCamera deathCamera;

    private bool deathCamActive = false;

    [SerializeField]
    private GameObject deathCamTarget;

    private GameObject player;
    private ShipControls controls;

    private bool blending = false;
    private CinemachineBrain brain;
    private Transform ship;


    void Awake()
    {
        if (Main == null) {
            Main = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathCamera.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        controls = player.GetComponent<ShipControls>();
        brain = GetComponent<CinemachineBrain>();
        ship = player.transform.Find("Ship");
    }

    // Update is called once per frame
    void Update()
    {
        if (!deathCamActive) {
            deathCamTarget.transform.position = ship.position;
            deathCamTarget.transform.rotation = ship.rotation;
        }
        if (blending) {
            if (!brain.IsBlending) {
                DeactivateDeathCam();
            }
        }
    }

    public void ActivateDeathCam() {
        deathCamActive = true;
        deathCamera.gameObject.SetActive(true);
        Invoke("StartBlend", 3.5f);
        controls.isDead = true;
    }

    public void StartBlend() {
        deathCamera.gameObject.SetActive(false);
        blending = true;
    }

    public void DeactivateDeathCam() {
        deathCamActive = false;
        controls.isDead = false;
        blending = false;
    }
}
