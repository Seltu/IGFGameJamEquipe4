using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : EnemyController
{

    protected override void Update()
    {
        if (_cooldown > 0)
            _cooldown -= Time.deltaTime;
        else
            ShootPlayer();

        FollowPlayer();

    }
}
