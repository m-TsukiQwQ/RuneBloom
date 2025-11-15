using System.Collections;
using UnityEngine;

public class ObjectBuff : MonoBehaviour
{
    private SpriteRenderer _sr;
    private EntityStats _statsToModify;

    [Header("Buff")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private float _buffDuration = 4f;
    [SerializeField] private bool _canBeUsed = true;
    [SerializeField] private string _buffName;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canBeUsed)
            return;

        _statsToModify = collision.GetComponent<EntityStats>();
        StartCoroutine(BuffCo(_buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        _canBeUsed = false;
        _sr.color = Color.clear;


        Debug.Log("Buff is applied");
        ApplyBuff(true);
        yield return new WaitForSeconds(duration);

        _canBeUsed = true;

        ApplyBuff(false);

        Debug.Log("Buff is over");
        Destroy(gameObject);
    }

    private void ApplyBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if (apply)
                _statsToModify.GetStatByType(buff.type).AddModifier(buff.value, _buffName);
            else
                _statsToModify.GetStatByType(buff.type).RemoveModifier(_buffName);

        }
        
    }
}
