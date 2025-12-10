using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class ConquestPointManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIUpgradeMenu upgradeMenu;
    [SerializeField] private ConquestPointsData conquestPointsDB;
    [SerializeField] private ConquestPointBehaviour conquestPoint;
    [SerializeField] private WavesAndRoundSystem wavesSystem;
    [Space]
    [SerializeField] private GameObject handAura; 

    [Header("SpawnPointsLocations")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Variables")]
    [SerializeField] private float timeToActivate = 30f;
    [SerializeField] private float activeDuration = 15f;
    [SerializeField] private bool firstInteractionDone = false;
    

    private void Start()
    {
        upgradeMenu = FindAnyObjectByType<UIUpgradeMenu>().GetComponent<UIUpgradeMenu>();
        wavesSystem = FindAnyObjectByType<WavesAndRoundSystem>().GetComponent<WavesAndRoundSystem>();

        if(wavesSystem != null) wavesSystem.allowWaves = false;

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

        if (!firstInteractionDone && handAura != null) handAura.SetActive(false);

        ActivatePointFirstTime();
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

    private void ActivatePointFirstTime()
    {
        conquestPoint.SetActiveVisual(true);
    }

    void ActivatePoint()
    {
        Debug.Log("Activating ConquestPoint");

        Transform spot = spawnPoints[Random.Range(0, spawnPoints.Count)];
        conquestPoint.transform.position = spot.position;
        conquestPoint.SetActiveVisual(true);

        Radio.Instance.PlaySFX("SFX/ConquestPointSpawn");
    }

    void DeactivatePoint()
    {
        //Debug.Log("Deactivating ConquestPoint");

        conquestPoint.SetActiveVisual(false);
    }

    public void PlayerInteractedWithPoint()
    {
        DeactivatePoint();

        if (!firstInteractionDone)
        {
            firstInteractionDone = true;

            Invoke("ActivateHandAura", 0.1f);

            if (wavesSystem != null) wavesSystem.allowWaves = true;

            StartCoroutine(Loop());


            upgradeMenu.OpenMenu(conquestMenu: true, firstTime:true);
            return;
        }

        upgradeMenu.OpenMenu(conquestMenu: true, firstTime:false);
    }

    private void ActivateHandAura()
    {
        if (handAura != null) handAura.SetActive(true);
    }
}


