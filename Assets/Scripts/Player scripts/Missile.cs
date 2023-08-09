using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Missile : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] public Rigidbody _rb;
    [SerializeField] public Transform _target;
    [SerializeField] public GameObject _explosionPrefab;

    [Header("MOVEMENT")]
    [SerializeField] public float _speed = 15;
    [SerializeField] float _verticalFloat = 50;
    [SerializeField] public float _rotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] public float _maxDistancePredict = 100;
    [SerializeField] public float _minDistancePredict = 5;
    [SerializeField] public float _maxTimePrediction = 5;
    public Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] public float _deviationAmount = 50;
    [SerializeField] public float _deviationSpeed = 2;
    [SerializeField] float _timeToFollow = 1;

    [Header("EXPLOSION")]
    [SerializeField] private float _triggerForce = 0.5f;
    [SerializeField] private float _explosionRadius = 5;
    [SerializeField] private float _explosionForce = 500;
    [SerializeField] public GameObject _particles;
    float _time = 0;
    public void Start()
    {

        _target = FindObjectsOfType<Transform>().Where(x => x.GetComponent<Explosive>() || x.GetComponent<BaseEnemy>()).
                  OrderBy(x => Vector3.Angle(transform.forward, x.transform.position - transform.position)).
                  FirstOrDefault(); //busca el que este mas cerca de donde estabas apuntando con el misil
        Debug.Log(Vector3.Dot(transform.forward, _target.transform.position - transform.position));
    }

    private void FixedUpdate()
    {
        _time += Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward, Vector3.up, _time * 0.6f);
        _rb.velocity = transform.forward * _speed;

        if (_time > _timeToFollow)
            if (_target != null)
            {
                GetComponent<Collider>().enabled = true;
                transform.forward = Vector3.Slerp(transform.forward, _target.transform.position - transform.position, _time * 0.6f);
                _rb.velocity = transform.forward * _speed;

                var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));



                AddDeviation(leadTimePercentage);

                RotateRocket();
            }



    }


    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
    public LayerMask _enemyLayer; //la mascara de capa de enemigos es la 9, si lo queres pasar a int, tenes que hacer 2**9, lo que da 512
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= _triggerForce)
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius, _enemyLayer);

            foreach (var obj in surroundingObjects)
            {
                if (obj.GetComponent<Explosive>())
                    obj.GetComponent<Explosive>().Explosion();
                var enemy = obj.GetComponent<BaseEnemy>();

                if (enemy == null || enemy.rb == null) continue;


                var liftComponent = enemy.GetComponentInChildren<LiftProportion>();
                if (liftComponent)
                    liftComponent.Lift();
                enemy.TakeDamage(200);
            }

            Instantiate(_particles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

}
