using UnityEngine;

public interface IState 
{
    // enter state, update state, exit state
    void OnEnter(Enemy enemy);
    void OnExecute(Enemy enemy);
    void OnExit(Enemy enemy);
}
