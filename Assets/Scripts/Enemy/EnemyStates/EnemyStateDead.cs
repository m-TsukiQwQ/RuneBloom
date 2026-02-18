using UnityEngine;

public class EnemyStateDead : EnemyState
{
    public EnemyStateDead(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.GetComponentInChildren<Collider2D>().enabled = false;
        _enemy.GetComponent<LootSpawner>()?.SpawnLoot();
        SkillManager.instance.AddSkillPointsByType(UISkillBookPageType.ElementIce, 1);


        _enemy.Death();
        _stateMachine.SwithOffMachine();
    }


}
