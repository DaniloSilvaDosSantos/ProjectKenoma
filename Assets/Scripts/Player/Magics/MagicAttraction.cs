using UnityEngine;

public class MagicAttraction : MagicBase
{
    public override void Cast()
    {
        Vector3 spawnPos = playerCamera.transform.position;

        Debug.Log("Casting a levitation magic sphere in " + spawnPos);

        GameObject sphere = Instantiate(magicData.prefab, spawnPos, Quaternion.identity);

        AttractionField field = sphere.GetComponent<AttractionField>();
        if (field != null)
        {
            Vector3 lookDirection = playerCamera.transform.forward;

            field.Initialize(magicData, lookDirection, controller);
        }
        else
        {
            Debug.Log("The AttractionField reference was not found or there is no reference for the AttractionField prefab!");
        }
    }
}
