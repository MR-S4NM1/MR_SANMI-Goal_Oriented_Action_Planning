using System;
using System.Collections.Generic;

public class WorldState : Dictionary<string, object>
{
    public event Action<string, object> OnWorldStateChanged;

    public new object this[string key]
    {
        get => base[key];
        set
        {
            base[key] = value;
            OnWorldStateChanged?.Invoke(key, value);
        }
    }
}
