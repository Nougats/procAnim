using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float detectionRange = 5f;

    private void Update()
    {
        DetectEnemies();
    }

    private void DetectEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRange, Vector3.forward, 0f);

        foreach (RaycastHit hit in hits)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.Detect();
            }
        }
    }


}
