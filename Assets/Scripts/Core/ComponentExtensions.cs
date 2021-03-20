using UnityEngine;

public static class ComponentExtensions
{
    public static void GetComponentAndAssertExists<T1, T2>(this T1 c, ref T2 component) where T1 : Component
    {
        component ??= c.GetComponent<T2>();
        Debug.Assert(component != null, $"{typeof(T2).Name} is missing on gameObject and is required by {typeof(T1).Name}", c);
    }
    public static void GetComponentInChildrenAndAssertExists<T1, T2>(this T1 c, ref T2 component) where T1 : Component 
    {
        component ??= c.GetComponentInChildren<T2>();
        Debug.Assert(component != null, $"{typeof(T2).Name} is missing from children and is required by {typeof(T1).Name}", c);
    }
    
    public static void GetComponentInParentAndAssertExists<T1, T2>(this T1 c, ref T2 component) where T1 : Component 
    {
        component ??= c.GetComponentInParent<T2>();
        Debug.Assert(component != null, $"{typeof(T2).Name} is missing from children and is required by {typeof(T1).Name}", c);
    }
}