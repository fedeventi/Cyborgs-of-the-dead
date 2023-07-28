using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [Header("EXPLOSION")]
    [SerializeField] public float _triggerForce = 0.5f;
    [SerializeField] public float _explosionRadius = 5;
    [SerializeField] public float _explosionForce = 500;
    [SerializeField] public GameObject _particles;
    [SerializeField] public LayerMask _enemyLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= _triggerForce)
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius, _enemyLayer);

            foreach (var obj in surroundingObjects)
            {
                var enemy = obj.GetComponent<BaseEnemy>();
                if (enemy == null || enemy.rb == null) continue;
                enemy.TakeDamage(200);
            }

            Instantiate(_particles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
