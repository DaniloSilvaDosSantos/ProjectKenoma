using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class LevitationField : MonoBehaviour
{
    private MagicData magicData;

    private float currentScale;
    private float fade = 1f;
    private Material sphereMaterial;
    private SphereCollider sphereCollider;
    private HashSet<EnemyController> affectedEnemies = new HashSet<EnemyController>();

    public void Initialize(MagicData data)
    {
        magicData = data;
    }

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) sphereMaterial = renderer.material;

        transform.localScale = Vector3.one * magicData.sphereStartScale;
        currentScale = magicData.sphereStartScale;
    }

    private void Update()
    {
        if (magicData == null) return;

        if (currentScale < magicData.sphereFinalScale)
        {
            currentScale += magicData.sphereGrowSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * currentScale;
        }
        else
        {
            fade -= magicData.sphereFadeSpeed * Time.deltaTime;

            if (sphereMaterial != null)
            {
                Color color = sphereMaterial.color;
                color.a = Mathf.Clamp01(fade);
                sphereMaterial.color = color;
            }

            if (fade <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null && !affectedEnemies.Contains(enemy))
        {
            affectedEnemies.Add(enemy);
            Debug.Log(other.gameObject.name + "affected by the Levitation Magic");
            enemy.ApplyLevitationEffect(magicData);
        }
    }
}
