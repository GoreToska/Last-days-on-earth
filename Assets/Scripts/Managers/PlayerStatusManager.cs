using UnityEngine;
using UnityEngine.Events;

public class PlayerStatusManager : MonoBehaviour
{
    [HideInInspector] public static PlayerStatusManager Instance;

    [Header("Player status")]
    [SerializeField] public bool isDead = false;
    [SerializeField] public float hp = 100f;
    [SerializeField] public float stamina = 100f;

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

        SetHP(hp);
        SetStamina(stamina);
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

    public void SetHP(float value)
    {
        if (value > 100)
        {
            value = 100;
        }

        hp = value;
        HUDManager.Instance.UpdateHP(hp);
    }

    public void SetStamina(float value)
    {
        if(value > 100)
        {
            value = 100;
        }

        stamina = value;
        HUDManager.Instance.UpdateStamina(stamina);
    }

    public void RegenHP(float value)
    {
        hp += value;

        //  Update UI
        HUDManager.Instance.UpdateHP(hp);

        if (hp >= 100f)
        {
            hp = 100f;
        }
    }

    public void RegenStamina(float value)
    {
        stamina += value;

        // update UI
        HUDManager.Instance.UpdateStamina(stamina);

        if (stamina >= 100f)
        {
            stamina = 100f;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        //  Update UI
        HUDManager.Instance.UpdateHP(hp);
        
        if (hp <= 0)
        {
            hp = 0;
            deathEvent.Invoke();
            isDead = true;
        }
    }

    public void TakeStaminaDamage(float damage)
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
}