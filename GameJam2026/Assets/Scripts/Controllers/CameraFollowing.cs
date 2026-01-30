using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private GameObject m_Player;

    [SerializeField] private Vector3 m_Offset = new (0, 0, -10);

    private void OnValidate()
    {
        if (m_Camera == null) m_Camera = this.GetComponent<Camera>();

        if (m_Player == null) m_Player = GameObject.FindGameObjectWithTag("Player");

        if (m_Player != null) m_Camera.transform.position = m_Player.transform.position + m_Offset;
    }

    private void Start()
    { 
        if (m_Player != null) m_Camera.transform.position = m_Player.transform.position + m_Offset; 
    }

    private void LateUpdate()
    {
        if (m_Player != null)
        {
            m_Camera.transform.position = m_Player.transform.position + m_Offset;
        }
    }


}
