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
        //if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        //if (collision.transform.TryGetComponent<IExplode>(out var ex)) ex.Explode();

        Destroy(gameObject);
    }

}
