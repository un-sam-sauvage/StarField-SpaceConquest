using UnityEngine;

public class State
{
    public enum Event
    {
        //Quand tu rentre dans la variable , update et quand tu sors
        ENTER,
        UPDATE,
        EXIT
    }
    
    protected Event stage;
    protected State nextState;

    public State()
    {
        stage = Event.ENTER; //demarre le tour 
    }
    public virtual void Enter()
    {
        stage = Event.UPDATE;
    }

    public virtual void Update()
    {
        stage = Event.UPDATE;
    }
    public virtual void Exit()
    {
        stage = Event.EXIT;
    }

    public State Process()
    {
        if (stage == Event.ENTER)
        {
            Enter();
        }
        if (stage == Event.UPDATE)
        {
            Update();
        }
        if (stage == Event.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}
