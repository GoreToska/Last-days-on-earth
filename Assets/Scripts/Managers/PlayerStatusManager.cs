using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStatusManager : CharacterStatusManager
{
    [Header("Player status")]
    [SerializeField] private bool isDead = false;
    [SerializeField] public float hp = 100f;
    [SerializeField] public float stamina = 100f;

    [SerializeField] private float cameraShakeDuration;

    [SerializeField] private float staminaRegeneration = 4f;
    [SerializeField] private float staminaTimeToRegen = 4f;
    private float staminaRegenTimer = 0f;

    public event UnityAction deathEvent = delegate { };
    public event UnityAction fatigueEvent = delegate { };

    public override void Start()
    {
        SetHP(hp);
        SetStamina(stamina);

        deathEvent += GetComponent<Ragdoll>().EnableRagdoll;
        deathEvent += PlayerInputManager.Instance.DisableInput;
        deathEvent += () => GetComponent<CharacterController>().enabled = false;
    }

    public override void OnDisable()
    {
        deathEvent -= GetComponent<Ragdoll>().EnableRagdoll;
        deathEvent -= PlayerInputManager.Instance.DisableInput;
        deathEvent += () => GetComponent<CharacterController>().enabled = false;
    }

    private void Update()
    {
        if (staminaRegenTimer <= staminaTimeToRegen)
        {
            staminaRegenTimer += Time.deltaTime;
        }
        else
        {
            RegenStamina(staminaRegeneration * Time.deltaTime);
        }
    }

    public override void SetHP(float value)
    {
        if (value > 100)
        {
            value = 100;
        }

        hp = value;
        HUDManager.Instance.UpdateHP(hp);
    }

    public override void SetStamina(float value)
    {
        if (value > 100)
        {
            value = 100;
        }

        stamina = value;
        HUDManager.Instance.UpdateStamina(stamina);
    }

    public override void RegenHP(float value)
    {
        hp += value;

        //  Update UI
        HUDManager.Instance.UpdateHP(hp);

        if (hp >= 100f)
        {
            hp = 100f;
        }
    }

    public override void RegenStamina(float value)
    {
        stamina += value;

        // update UI
        HUDManager.Instance.UpdateStamina(stamina);

        if (stamina >= 100f)
        {
            stamina = 100f;
        }
    }

    public override void TakeDamage(float damage)
    {
        hp -= damage;

        //  Update UI
        HUDManager.Instance.UpdateHP(hp);
        CameraActions.Instance.ImpactShake(cameraShakeDuration, damage / 10);

        if (hp <= 0)
        {
            hp = 0;
            deathEvent.Invoke();
            isDead = true;
        }
    }

    public override void TakeStaminaDamage(float damage)
    {
        if (stamina == 0)
        {
            stamina = 0;
            fatigueEvent.Invoke();
            Debug.Log("Fatigue");
            return;
        }

        stamina -= damage;
        staminaRegenTimer = 0f;

        // update UI
        HUDManager.Instance.UpdateStamina(stamina);
    }

    public override bool IsDead { get => isDead; set => isDead = value; }
}