using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class ConquestPointManager : MonoBehaviour
{
    [Header("References")]
    public ConquestPointsData conquestPointsDB;
    public ConquestPointBehaviour conquestPoint;

    [Header("SpawnPointsLocations")]
    public List<Transform> spawnPoints;

    [Header("Variables")]
    public float timeToActivate = 30f;
    public float activeDuration = 15f;

    private void Start()
    {
        if (conquestPointsDB != null)
        {
            timeToActivate = conquestPointsDB.conquestPointTimeToActivate;
            activeDuration = conquestPointsDB.conquestPointActiveDuration;
        }
        else
        {
            Debug.Log("ConquestPoints database is empty.");
        }

        conquestPoint.Init(this);
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToActivate);

            ActivatePoint();

            yield return new WaitForSeconds(activeDuration);

            DeactivatePoint();
        }
    }

    void ActivatePoint()
    {
        Debug.Log("Activating ConquestPoint");

        Transform spot = spawnPoints[Random.Range(0, spawnPoints.Count)];
        conquestPoint.transform.position = spot.position;
        conquestPoint.SetActiveVisual(true);
    }

    void DeactivatePoint()
    {
        Debug.Log("Deactivating ConquestPoint");

        conquestPoint.SetActiveVisual(false);
    }

    public void PlayerInteractedWithPoint()
    {
        DeactivatePoint();
    }
}

