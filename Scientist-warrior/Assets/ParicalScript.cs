using UnityEngine;

public class ParicalScript : MonoBehaviour
{
    private float m_Dlay = 0;
    [SerializeField] private AnimationClip m_AnimationClip;
    private void Start()
    {
        m_Dlay = m_AnimationClip.length;
        Destroy(gameObject,m_Dlay);
    }
}