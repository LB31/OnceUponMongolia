using UnityEngine;
using System.Collections;

public class QuestMarkerController : MonoBehaviour
{

    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    private bool goingUp = true;
    public float floatRate;
    private float floatTimer;

    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

    private Vector3 originPosition;
    private Vector3 originStartScale;
    private Vector3 originEndScale;

    private void Awake()
    {
        originPosition = transform.position;
        originStartScale = startScale;
        originEndScale = endScale;
    }

    private void OnEnable()
    {
        StartCoroutine(ReactToVeroDistance());
    }

    private void OnDisable()
    {
        StopCoroutine(ReactToVeroDistance());
    }

    private IEnumerator ReactToVeroDistance()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, GameManager.Instance.Vero.position);
            float scale;
            if (distance > 100)
                scale = 3;
            else if (distance > 50)
                scale = 2;
            else if (distance > 20)
                scale = 1.5f;
            else
                scale = 1;

            startScale = originStartScale * scale;
            endScale = originEndScale * scale;
            transform.position = originPosition + new Vector3(0, scale * 10, 0);

            yield return new WaitForSeconds(3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimated)
        {
            if (isRotating)
            {
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
            }

            if (isFloating)
            {
                floatTimer += Time.deltaTime;
                Vector3 moveDir = new Vector3(0.0f, 0.0f, floatSpeed);
                transform.Translate(moveDir);

                if (goingUp && floatTimer >= floatRate)
                {
                    goingUp = false;
                    floatTimer = 0;
                    floatSpeed = -floatSpeed;
                }

                else if (!goingUp && floatTimer >= floatRate)
                {
                    goingUp = true;
                    floatTimer = 0;
                    floatSpeed = +floatSpeed;
                }
            }

            if (isScaling)
            {
                scaleTimer += Time.deltaTime;

                if (scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
                }
                else if (!scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                }

                if (scaleTimer >= scaleRate)
                {
                    if (scalingUp) { scalingUp = false; }
                    else if (!scalingUp) { scalingUp = true; }
                    scaleTimer = 0;
                }
            }
        }
    }
}
