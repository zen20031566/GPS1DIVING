using System;
using Unity.VisualScripting;

public class TrashEvents 
{
    public event Action<TrashSO> OnTrashCollected;

    public void TrashCollected(TrashSO trashData)
    {
        OnTrashCollected?.Invoke(trashData);
    }

}
