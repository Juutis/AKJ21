using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ShipHealth : MonoBehaviour
{
    private int maxHP = 1;
    private int currentHP;

    [SerializeField]
    private float shieldRechargeAmount;
    [SerializeField]
    private float shieldRechargeCD;

    [SerializeField]
    private float hitCD = 1f;

    private float lastHit = 0f;

    [SerializeField]
    private GameObject shipMesh;

    [SerializeField]
    private ParticleSystem explosionPrefab;

    private Rigidbody rb;

    [SerializeField]
    private Transform RespawnPoint;

    public int MaxHp {get {return maxHP;}}
    public int CurrentHp {get {return currentHP;}}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
        UpdateShield();
    }

    [SerializeField]
    float hpRechargeInterval = 0.5f;
    float hpRechargeTimer = 0f;
    bool isRecharging = false;

    public void UpdateShield() {
        ShipUpgrade upgrade = ShipUpgradeManager.main.GetCurrentHighestUpgrade(ShipUpgradeType.Shield);
        maxHP = upgrade.IntValue;
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P)) {
            currentHP--;
        }
        #endif
        if (!isRecharging) {
            if (currentHP < maxHP) {
                isRecharging = true;
                hpRechargeTimer = 0f;
            }
        }
        hpRechargeTimer += Time.deltaTime;

        if (hpRechargeTimer > hpRechargeInterval) {
            GainHealth();
            isRecharging = false;
        }
    }

    public void GainHealth() {
        currentHP += 1;
        if (currentHP > maxHP) {
            currentHP = maxHP;
        }
    }

    private void Hit()
    {
        if (Time.time - lastHit < hitCD)
        {
            return;
        }

        lastHit = Time.time;

        currentHP--;

        //foreach (Material material in materials)
        //{
        //    material.color = Color.Lerp(matColorCache[material], Color.red, ((float)maxHP - currentHP) / maxHP * 0.666f);
        //}

        if (currentHP <= 0)
        {
            Explode();
        }
    }

    bool alive = true;

    private void Explode()
    {
        if (!alive) {
            return;
        }
        shipMesh.SetActive(false);
        var explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        Invoke("ReSpawn", 3.0f);
        alive = false;
        CameraManager.Main.ActivateDeathCam();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Hit();
            var bullet = other.GetComponent<Bullet>();
            bullet.Kill();
        }
    }
    

    public void ReSpawn() {
        alive = true;
        currentHP = maxHP;
        shipMesh.SetActive(true);
        transform.position = RespawnPoint.position;
        transform.rotation = RespawnPoint.rotation;
        rb.position = RespawnPoint.transform.position;
        rb.rotation = RespawnPoint.rotation;
    }
}
