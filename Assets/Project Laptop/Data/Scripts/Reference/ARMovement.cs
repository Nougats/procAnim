using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ARMovement : MonoBehaviour
{
    //Step Distance berechnet die Schritthöhe
    //Wenn die Schrittweite nicht das Maximum erreicht, gibt es keinen Grund das Bein die komplette Schritthöhe anzuheben
    [SerializeField] private float stepDistance = 1f;
    [SerializeField] private float stepHeight = 1f;
    [SerializeField] private float stepSpeed = 5f;

    //Multiplikator für die Geschwindigkeit, wenn man die Geschwindigkeit eines Schrittes erhöhen möchte
    [SerializeField] private float velocityMulti = .4f;
    [SerializeField] private float cycleSpeed = 1f;
    //Anzahl der Male, die sich das Bein pro Sekunde bewegen soll
    [SerializeField] private float cycleLimit = 1f;
    //boolean für das einstellen ob sich Beine unabhängig von einander bewegen lassen können
    [SerializeField] private bool setTimingsManually;
    //Liste der Beine und welche beine sich zuerst bewegen sollen, in Sekunden
    [SerializeField] private float[] manualTimings;
    //Wenn sich alle Beine einzeln Bewegen sollen...timing muss (1/anzahl der beine) gerechnet werden dass das offset gescheit funktioniert
    [SerializeField] private float timingsOffset = 0.25f;
    //Clamp um die höchstgeschwindigkeit der Beine zu limitieren
    [SerializeField] private float velocityClamp = 4f;
    //Das Layer auf dem die Beine sich bewegen und mit welchem sie colliden sollen
    [SerializeField] private LayerMask groundLayer;
    //AnimationCurve für das anheben und senken der Beine in einem Arch
    [SerializeField] private AnimationCurve legArcYAxis = new AnimationCurve(new Keyframe(0, 0, 0, 2.5f), new Keyframe(0.5f, 1), new Keyframe(1, 0, -2.5f, 0));
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    //Liste aller Beine
    [SerializeField] private Transform[] numberOfLegs;

    //Debug option zum darstellen der Raycasts
    public bool showGizmos = true;
    [SerializeField] float legRayOffset = 3;
    [SerializeField] float legRayLength = 6;
    //GroundCheck Radius
    [SerializeField] float sphereCastRadius = 1;

    //Rate in der die Beine sich updaten, höherer Wert = langsamere Bewegungen, kleiner Wert = schnelle Bewegung
    [SerializeField] private float refreshTiming = 60f;

    public EventHandler<Vector3> OnStepFinished;

    private Vector3[] lastLegPositions;
    private Vector3[] defaultLegPositions;
    private Vector3[] raycastPoints;
    private Vector3[] targetStepPosition;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastBodyPos;

    private float[] footTimings;
    private float[] targetTimings;
    private float[] totalDistance;
    private float clampDevider;
    private float[] arcHeightMulti;

    private int intLegs;
    private int indexToMove;

    private bool[] isLegMoving;

    private void Start()
    {
        indexToMove = -1;
        intLegs = numberOfLegs.Length;

        defaultLegPositions = new Vector3[intLegs];
        lastLegPositions = new Vector3[intLegs];
        targetStepPosition = new Vector3[intLegs];

        isLegMoving = new bool[intLegs];
        footTimings = new float[intLegs];
        arcHeightMulti = new float[intLegs];
        totalDistance = new float[intLegs];

        if (setTimingsManually && manualTimings.Length != intLegs)
        {
            Debug.LogError("Die Length von manualTimings von die gleiche Anzahl sein wie es Beine gibt ");
        }

        for (int i = 0; i < intLegs; i++)
        {
            if (setTimingsManually)
            {
                footTimings[i] = manualTimings[i];
            }
            else
            {
                footTimings[i] = i * timingsOffset;
            }

            lastLegPositions[i] = numberOfLegs[i].position;
            defaultLegPositions[i] = numberOfLegs[i].localPosition;
        }

        StartCoroutine(UpdateTimings(refreshTiming));
    }

    private void Update()
    {
        velocity = (transform.position - lastBodyPos) / Time.deltaTime;
        velocity = Vector3.MoveTowards(lastVelocity, velocity, Time.deltaTime * 45f);
        clampDevider = 1 / Remap(velocity.magnitude, 0f, velocityClamp, 1f, 2f);

        for (int i = 0; i < intLegs; i++)
        {
            //Beine am Boden fixieren
            if (i == indexToMove) continue;
            numberOfLegs[i].position = Pointer.SetOnGround(lastLegPositions[i], groundLayer, legRayOffset, legRayLength, sphereCastRadius);
        }

        //Remap, um Beine öfter bewegen zu können wenn man max Speed erreicht
        float cycleSpeedMulti = Remap(velocity.magnitude, legRayOffset, velocityClamp, 1f, 2f);

        for (int i = 0; i < intLegs; ++i)
        {
            //Beine bewegen sich erst dann wenn die maximale Distanz berechnet durch Foot timings erreicht wird
            footTimings[i] += Time.deltaTime * cycleSpeed * cycleSpeedMulti;

            if(footTimings[i] >= cycleLimit)
            {
                footTimings[i] = 0;

                indexToMove = i;
                SetUp(i);
            }
        }

        lastBodyPos = transform.position;
    }

    public void SetUp(int index)
    {
        //den geziehlten Punkt für den Schritt finden
        Vector3 v = transform.TransformPoint(
            defaultLegPositions[index]) +
            velocity.normalized * Mathf.Clamp(velocity.magnitude, 0, velocityClamp + clampDevider) * velocityMulti;
        targetStepPosition[index] = Pointer.SetOnGround(v, groundLayer, legRayOffset, legRayLength, sphereCastRadius);

        totalDistance[index] = GetDistanceToTarget(index);

        float distance = Vector3.Distance(numberOfLegs[index].position, targetStepPosition[index]);
        arcHeightMulti[index] = distance / stepDistance;

        if(targetStepPosition[index] != Vector3.zero && Pointer.IsValidPoint(targetStepPosition[index], groundLayer, legRayOffset, legRayLength,sphereCastRadius))
        {
            StartCoroutine(MakeStep(targetStepPosition[index], indexToMove));
        }
    }

    private IEnumerator MakeStep(Vector3 targetPosition, int index)
    {
        float current = 0;

        while(current < 1)
        {
            current += Time.deltaTime * stepSpeed;
            float positionY = legArcYAxis.Evaluate(current) * stepHeight * Mathf.Clamp(arcHeightMulti[index], 0, 1f);

            Vector3 desiredStepPosition = new Vector3(
                targetPosition.x,
                positionY + targetPosition.y,
                targetPosition.z);

            numberOfLegs[index].position = Vector3.Lerp(lastLegPositions[index], desiredStepPosition, easingCurve.Evaluate(current));

            yield return null;
        }

        LegReachedTargetPosition(targetPosition, index);
    }
    private void LegReachedTargetPosition(Vector3 targetPosition, int index)
    {
        indexToMove = -1;
        numberOfLegs[index].position = targetPosition;
        lastLegPositions[index] = numberOfLegs[index].position;

        isLegMoving[index] = false;
    } 
    public bool IsLegMoving(int index)
    {
        return isLegMoving[index];
    }
    private float GetDistanceToTarget(int index)
    {
        return Vector3.Distance(numberOfLegs[index].position, transform.TransformPoint(defaultLegPositions[index]));
    }

   

    public static float Remap(float input, float oldLow, float oldHigh, float newLow, float newHigh)
    {
        float t = Mathf.InverseLerp(oldLow, oldHigh, input);
        return Mathf.Lerp(newLow, newHigh, t);
    }
    private IEnumerator UpdateTimings(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        for (int i = 0; i < intLegs; i++)
        {
            if (setTimingsManually)
            {
                footTimings[i] = manualTimings[i];
            }
            else
            {
                footTimings[i] = i * timingsOffset;
            }
        }

        StartCoroutine(UpdateTimings(refreshTiming));
    }
}
