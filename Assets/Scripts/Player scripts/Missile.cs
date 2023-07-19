using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] public Rigidbody _rb;
    [SerializeField] public BaseEnemy _target;
    [SerializeField] public GameObject _explosionPrefab;

    [Header("MOVEMENT")]
    [SerializeField] public float _speed = 15;
    [SerializeField] public float _rotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] public float _maxDistancePredict = 100;
    [SerializeField] public float _minDistancePredict = 5;
    [SerializeField] public float _maxTimePrediction = 5;
    public Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] public float _deviationAmount = 50;
    [SerializeField] public float _deviationSpeed = 2;

    [Header("EXPLOSION")]
    [SerializeField] private float _triggerForce = 0.5f;
    [SerializeField] private float _explosionRadius = 5;
    [SerializeField] private float _explosionForce = 500;
    [SerializeField] public GameObject _particles;

    public void Start()
    {
        _target = FindObjectOfType<BaseEnemy>();
    }

    private void FixedUpdate()
    {
        if(_target != null)
        {
            _rb.velocity = transform.forward * _speed;

            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));

            PredictMovement(leadTimePercentage);

            AddDeviation(leadTimePercentage);

            RotateRocket();
        }
        else
        {
            transform.position += transform.rotation * Vector3.forward * _speed * Time.deltaTime;
        }

    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = _target.rb.position + _target.rb.velocity * predictionTime;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= _triggerForce)
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

            foreach (var obj in surroundingObjects)
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb == null) continue;

                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1000);
            }

            Instantiate(_particles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

}
