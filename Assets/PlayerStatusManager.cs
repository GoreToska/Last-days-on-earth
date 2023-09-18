using UnityEngine;
using UnityEngine.Events;

public class PlayerStatusManager : MonoBehaviour
{
    [HideInInspector] public static PlayerStatusManager Instance;

    [Header("Player status")]
    [SerializeField] private float hp = 100f;
    [SerializeField] private float stamina = 100f;

    [SerializeField] private float staminaRegeneration = 4f;
    [SerializeField] private float staminaTimeToRegen = 4f;
    private float staminaRegenTimer = 0f;

    public event UnityAction deathEvent = delegate { };
    public event UnityAction fatigueEvent = delegate { };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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

    public void RegenHP(float value)
    {
        hp += value;

        //  Update UI
        PlayerUIManager.Instance.SetHP(hp);

        if (hp >= 100f)
        {
            hp = 100f;
        }
    }

    public void RegenStamina(float value)
    {
        stamina += value;

        // update UI
        PlayerUIManager.Instance.SetStamina(stamina);

        if (stamina >= 100f)
        {
            stamina = 100f;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        //  Update UI
        PlayerUIManager.Instance.SetHP(hp);

        if (hp <= 0)
        {
            hp = 0;
            deathEvent.Invoke();
        }
    }

    public void TakeStaminaDamage(float damage)
    {
        stamina -= damage;
        staminaRegenTimer = 0f;

        // update UI
        PlayerUIManager.Instance.SetStamina(stamina);

        if (stamina <= 0)
        {
            fatigueEvent.Invoke();
        }
    }
}