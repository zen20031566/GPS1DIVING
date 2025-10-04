using System;

public class InteractEvent 
{
    public event Action<Type> OnInteract;

    public void Interact(Type interactedObj)
    {
        OnInteract?.Invoke(interactedObj);
    }
}
