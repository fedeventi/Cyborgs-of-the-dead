using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState<T> : State<T>
{
    // Start is called before the first frame update
    HeavyEnemy _source;
    Vector3 _position;
    Vector3 _direction;
    bool _stopCharging;
    float _speed=400;
    bool _StartCharge;
    public ChargeState(HeavyEnemy source)
    {
        _source = source;
    }
    public override void Awake()
    {
        base.Awake();
        _position=_source.player.transform.position;
        _position.y=_source.transform.position.y;
        _direction = _position - _source.transform.position;
        _source.charging = true;
        _stopCharging = false;
        _StartCharge = false;
        _source.StartCoroutine(StartCharge());
        _source.ChargedPlayer();
    }
    public override void Execute()
    {
        base.Execute();
        if (_StartCharge)
        {

            var distance=Vector3.Distance(_position,_source.transform.position);
            if(!_source.stunned)
                if (!_stopCharging)
                {

                    if (distance <= 20) _source.StartCoroutine(ChangeStopCharging()); 
                    var rot = Quaternion.LookRotation(_direction);
                    _source.transform.rotation = Quaternion.Slerp(_source.transform.rotation, rot, Time.deltaTime * 5).normalized;
                    _source.transform.position += _direction.normalized * _speed * Time.deltaTime;
                }
                else
                { 
                        _source.Transition("Chase");
                }
        }
        
    }
    public override void Sleep()
    {
        base.Sleep();
        _source.enemyView.animator.SetBool("charging", false);
        _source.StopCoroutine(StartCharge());
        _source.charging=false;
    }
    IEnumerator ChangeStopCharging()
    {
        yield return new WaitForSeconds(1);
        _stopCharging = true;
    }
    IEnumerator StartCharge()
    {
        _source.enemyView.WaitForCharge();
        yield return new WaitForSeconds(1);
        _source.enemyView.Charge();
        _StartCharge = true;
    }
    

}
