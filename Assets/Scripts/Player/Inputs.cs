using UnityEngine;

public class Inputs : MonoBehaviour
{
    private Controles controles;
    private PlayerController playerController;

    private void Awake()
    {
        controles = new();
        playerController = GetComponent<PlayerController>();

        RegisterPlayerActions();
    }

    private void OnEnable()
    {
        controles.Enable();
        controles.Player.Enable();
    }

    private void OnDisable()
    {
        controles.Player.Disable();
        controles.Disable();
    }

    private void RegisterPlayerActions()
    {
        controles.Player.Move.performed += ctx => playerController.SetMoveInput(ctx.ReadValue<Vector2>());
        controles.Player.Move.canceled += _ => playerController.SetMoveInput(Vector2.zero);

        controles.Player.Jump.performed += _ => playerController.RequestJump();
    }
}