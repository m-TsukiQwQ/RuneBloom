using UnityEngine;

public class PlayerAnimations : EntityAnimation
{
    [SerializeField] private PlayerInteraction _interaction;

    protected override void Awake()
    {
        base.Awake();
        _interaction = GetComponentInParent<PlayerInteraction>();
    }

    private void InstrumentTrigger()
    {

        _interaction.UseInstrument();
    }

}
