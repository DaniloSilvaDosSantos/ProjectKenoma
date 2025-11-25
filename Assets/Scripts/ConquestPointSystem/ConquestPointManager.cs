using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class ConquestPointManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIUpgradeMenu upgradeMenu;
    [SerializeField] private ConquestPointsData conquestPointsDB;
    [SerializeField] private ConquestPointBehaviour conquestPoint;

    [Header("SpawnPointsLocations")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Variables")]
    [SerializeField] private float timeToActivate = 30f;
    [SerializeField] private float activeDuration = 15f;

    private void Start()
    {
        upgradeMenu = FindAnyObjectByType<UIUpgradeMenu>().GetComponent<UIUpgradeMenu>();

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
        upgradeMenu.OpenMenu(true);
    }
}

