using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class GOAPAction : MonoBehaviour 
{ 
    public float cost = 1f;
    protected Dictionary<string, object> preconditions = new Dictionary<string, object>();
    protected Dictionary<string, object> effects = new Dictionary<string, object>();
    public event System.Action<GOAPAction> OnActionCompleted; public Dictionary<string, object> Preconditions => preconditions;
    public Dictionary<string, object> Effects => effects; 
    [SerializeField] protected AnimationsManager animationsManager; 
    protected float currentAnimationDuration; 
    protected void AddPrecondition(string key, object value) => preconditions[key] = value;
    protected void AddEffect(string key, object value) => effects[key] = value; 
    private void Awake() 
    { 
        if (animationsManager == null) 
            animationsManager = GetComponent<AnimationsManager>(); 
    } 
    
    public bool ArePreconditionsMet(WorldState state) 
    { 
        foreach (var p in preconditions) 
        { 
            if (!state.ContainsKey(p.Key)) return false; 
            if (!state[p.Key].Equals(p.Value)) return false; 
        } 
        return true; 
    } 
    
    public void Execute(WorldState state) 
    { 
        StartCoroutine(PerformAction(state)); 
    } 
    
    protected abstract IEnumerator PerformAction(WorldState state); 
    protected void Complete(WorldState state) 
    { 
        foreach (var e in effects) state[e.Key] = e.Value;
        StopAllCoroutines(); 
        OnActionCompleted?.Invoke(this); 
    } 
}