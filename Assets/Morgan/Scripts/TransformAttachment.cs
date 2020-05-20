using UnityEngine;

[ExecuteAlways]
public class TransformAttachment : MonoBehaviour
{
    public Transform m_Target;
    public bool m_MatchPosition = true;
    public bool m_MatchRotation = true;

    Transform m_Transform;

    void LateUpdate()
    {
        if (m_Target == null)
            return;

        if (m_Transform == null)
            m_Transform = transform;
        
        if (m_MatchPosition)
            m_Transform.position = m_Target.position;
        if (m_MatchRotation)
            m_Transform.rotation = m_Target.rotation;
    }
}
