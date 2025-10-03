using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController PlayerController;

    public PlayerStateMachine PlayerStateMachine;

    public OnLandState OnLandState;
    public InWaterState InWaterState;
    public OnLadderState OnLadderState;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        PlayerStateMachine = new PlayerStateMachine();

        OnLandState = new OnLandState(PlayerStateMachine, this);
        InWaterState = new InWaterState(PlayerStateMachine, this);
        OnLadderState = new OnLadderState(PlayerStateMachine, this); 
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
