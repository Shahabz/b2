using UnityEngine;

class DestroyAfter : MonoBehaviour
{
    public float Lifetime = 3.0f;
    float m_timeLeft;

    void Start()
    {
        m_timeLeft = Lifetime;
    }

    void Update()
    {
        m_timeLeft -= Time.deltaTime;

        if (m_timeLeft <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}

