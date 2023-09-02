using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoadLamp : MonoBehaviour
{
    private Light2D _light;
    private Light2D _lightSmall;
    private ParticleSystem _spark;

    private bool _lightIsOn = false;

    private void Start()
    {
        _light = transform.Find("Light2D").GetComponent<Light2D>();
        _lightSmall = transform.Find("Light2DSmall").GetComponent<Light2D>();
        _spark = transform.Find("Spark").GetComponent<ParticleSystem>();
        _spark.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (WorldManager.Instance.dayState == WorldManager.DayState.NIGHT && !_lightIsOn)
            TurnOnLight();

        if (WorldManager.Instance.dayState == WorldManager.DayState.DAY && _lightIsOn)
            TurnOffLight();
    }

    private void TurnOnLight()
    {
        if (_light.intensity <= 1.3f)
            _light.intensity += Time.deltaTime / 2;
        else
            _light.intensity = 1.3f;

        if (_lightSmall.intensity <= 1.3f)
            _lightSmall.intensity += Time.deltaTime / 2;
        else
            _lightSmall.intensity = 1.3f;

        if (_light.intensity == 1.3f && _lightSmall.intensity == 1.3f)
        {
            _lightIsOn = true;
            _spark.gameObject.SetActive(true);
        }
    }
    private void TurnOffLight()
    {
        if (_light.intensity >= 0)
            _light.intensity -= Time.deltaTime / 2;
        else
            _light.intensity = 0;

        if (_lightSmall.intensity >= 0)
            _lightSmall.intensity -= Time.deltaTime / 2;
        else
            _lightSmall.intensity = 0;

        if (_light.intensity == 0 && _lightSmall.intensity == 0)
        {
            _lightIsOn = false;
            _spark.gameObject.SetActive(false);
        }
    }
}