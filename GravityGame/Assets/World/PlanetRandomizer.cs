using UnityEngine;

public class PlanetRandomizer : MonoBehaviour
{
    [SerializeField]
    private GameObject innerRings;

    [SerializeField]
    private GameObject outerRings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init() {
        Randomize();
    }

    public void Randomize() {
        innerRings.SetActive(false);
        outerRings.SetActive(false);
        var planetColor = Random.ColorHSV(0.0f, 1.0f, 0.2f, 0.95f, 0.2f, 1.0f);
        GetComponent<Renderer>().material.color = planetColor;
        if (Random.Range(0.0f, 1.0f) < 0.3f) {
            innerRings.SetActive(true);
            var rings1Color = Random.ColorHSV(0.0f, 1.0f, 0.2f, 0.95f, 0.2f, 1.0f);
            innerRings.GetComponent<Renderer>().material.color = rings1Color;

            if (Random.Range(0.0f, 1.0f) < 0.3f) {
                outerRings.SetActive(true);
                var rings2Color = Random.ColorHSV(0.0f, 1.0f, 0.2f, 0.95f, 0.2f, 1.0f);
                outerRings.GetComponent<Renderer>().material.color = rings2Color;
            }
        }
        var scale = Random.Range(4f, 15f);
        transform.localScale = new Vector3(scale, scale, scale);
        var rotation = new Vector3(Random.Range(0.0f, 360f), Random.Range(0.0f, 360f), Random.Range(0.0f, 360f));
        transform.Rotate(rotation);
    }
}
