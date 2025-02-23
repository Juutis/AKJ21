using System.Linq;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float beamLength;

    [SerializeField]
    private float probeLength;

    [SerializeField]
    private float laserCD;
    private float lastHit = 0f;

    private bool isActive = false;
    Transform origin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && Time.time - lastHit > laserCD)
        {
            Pulse();
        }

        Probe();
    }

    private void Pulse()
    {
        RaycastHit[] hits = Physics.RaycastAll(origin.position, origin.forward, beamLength, LayerMask.GetMask("Destroyable"));

        if (hits.Length == 0)
        {
            return;
        }

        RaycastHit nearest = hits.OrderBy(x => x.distance).FirstOrDefault(x => x.collider != null);

        if (nearest.collider.gameObject.TryGetComponent(out DestroyableAsteroid asteroid))
        {
            asteroid.Hit();
        }

        if (nearest.collider.gameObject.TryGetComponent(out DestroyablePortal portal))
        {
            var engineLevel = ShipUpgradeManager.main.GetCurrentHighestUpgrade(ShipUpgradeType.Laser).IntValue;
            portal.Hit(engineLevel);
        }
    }

    private void Probe()
    {
        RaycastHit[] hits = Physics.RaycastAll(origin.position, origin.forward, probeLength, LayerMask.GetMask("Portal"));
        WorldOrigin worldOrigin = WorldOrigin.OfActiveWorld;

        if (hits.Length > 0)
        {
            worldOrigin.SetHiding(true);
        }
        else
        {
            worldOrigin.SetHiding(false);
        }
    }

    public void Activate()
    {
        lastHit = Time.time;
        isActive = true;
        SoundManager.main.PlayLoop(GameSoundType.Laser);
    }

    public void Deactivate()
    {
        isActive = false;
        SoundManager.main.StopLoop(GameSoundType.Laser);
    }

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
    }
}
