using System;

public class InteractEvent 
{
    public event Action OnInteract;

    public void Interact()
    {
        OnInteract?.Invoke();
    }
}
