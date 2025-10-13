using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager InventoryManager;
    public GearManager GearManager;
    public GoldManager GoldManager;
    public PlayerController PlayerController;
    public PlayerEquipment PlayerEquipment;
    public PlayerOxygen PlayerOxygen;
    public PlayerHealth PlayerHealth;

    public PlayerStateMachine PlayerStateMachine;
    public OnLandState OnLandState;
    public InWaterState InWaterState;
    public OnLadderState OnLadderState;
    public InUIOrDialog OnUIOrDialog;

    public GameObject PlayerHead;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerStateMachine = new PlayerStateMachine();

        OnLandState = new OnLandState(PlayerStateMachine, this);
        InWaterState = new InWaterState(PlayerStateMachine, this);
        OnLadderState = new OnLadderState(PlayerStateMachine, this);
        OnUIOrDialog = new InUIOrDialog(PlayerStateMachine, this);
    }

    private void Start()
    {
        PlayerStateMachine.Initialize(OnLandState);
    }

    private void Update()
    {
        PlayerStateMachine.CurrentState.FrameUpdate(); 
    }

    private void FixedUpdate()
    {
        PlayerStateMachine.CurrentState.PhysicsUpdate();
    }

}
