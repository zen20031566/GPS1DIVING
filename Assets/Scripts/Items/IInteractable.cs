public interface IInteractable
{
    public void Interact(Player player);

    bool CanInteract { get; set; }
}
