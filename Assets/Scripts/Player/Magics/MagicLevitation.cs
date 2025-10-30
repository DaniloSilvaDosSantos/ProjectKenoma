using UnityEngine;

public class MagicLevitation : MagicBase
{
    public override void Cast()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, magicData.range))
        {
            Debug.Log("Casting a levitation magic sphere in " + hit.point);

            if (magicData.prefab != null)
            {
                GameObject sphere = Instantiate(magicData.prefab, hit.point, Quaternion.identity);
                LevitationField field = sphere.GetComponent<LevitationField>();

                if (field != null)
                {
                    field.Initialize(magicData);
                }
            }
        }
        else
        {
            Vector3 endPoint = ray.origin + ray.direction * magicData.range;
            Debug.Log("No targets were hit by the raycast. Casting at the end point: " + endPoint);

            if (magicData.prefab != null)
            {
                GameObject sphere = Instantiate(magicData.prefab, endPoint, Quaternion.identity);
                LevitationField field = sphere.GetComponent<LevitationField>();

                if (field != null)
                {
                    field.Initialize(magicData);
                }
            }
        }
    }
}

