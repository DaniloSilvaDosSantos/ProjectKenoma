using UnityEngine;

public class MagicUltimate : MagicBase
{
    public override void Cast()
    {
        controller.playerMovement.isMovementLocked = true;
        controller.health.SetInvulnerable(true);

        Vector3 spawnPos = controller.transform.position + controller.transform.forward * magicData.ultimateDistanteFromPlayer;

        GameObject instance = Instantiate(magicData.prefab, spawnPos, Quaternion.identity);

        UltimateBlackHole blackHole = instance.GetComponent<UltimateBlackHole>();

        if (blackHole != null)
        {
            blackHole.Initialize(magicData, controller);
        }
        else
        {
            Debug.LogWarning("Ultimate prefab does NOT contain UltimateBlackHole script!");
        }
    }
}
