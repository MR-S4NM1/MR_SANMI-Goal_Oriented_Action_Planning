using System.Collections.Generic;

public class GOAPGoal
{
    public string name;
    public float priority;
    public Dictionary<string, object> desiredState = new Dictionary<string, object>();

    public GOAPGoal(string name, float priority)
    {
        this.name = name;
        this.priority = priority;
    }
}