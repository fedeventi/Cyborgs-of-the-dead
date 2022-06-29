using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack<T> : State<T>
{
    ExplosiveZombie _baseEnemy;
    EnemyView enemyView;
    float _speed;
    bool _hasShoot;
    public RangeAttack(ExplosiveZombie enemy, EnemyView view)
    {
        _baseEnemy = enemy;
        enemyView = view;
    }
    public override void Execute()
    {
        enemyView.AttackSound();

        var target = _baseEnemy.player.transform.position;
        target.y = _baseEnemy.transform.position.y;
        Vector3 dir = target - _baseEnemy.transform.position;

        var rot = Quaternion.LookRotation(dir);
        _baseEnemy.transform.rotation = Quaternion.Slerp(_baseEnemy.transform.rotation, rot, Time.deltaTime).normalized;
        var angle = Vector3.Angle(dir, _baseEnemy.transform.forward);
        if(angle <=3 && !_hasShoot)
        {
            _baseEnemy.DistanceAttack();
            _hasShoot = true;
            
        }
    }

    public override void Awake()
    {
        base.Awake();
        _hasShoot = false;
    }
    public override void Sleep()
    {
        base.Sleep();
    }
}
