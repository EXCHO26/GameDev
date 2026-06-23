using UnityEngine;

public abstract class EnemyController : EntityController
{
    protected Transform m_playerTransform;

    protected override void Start()
    {
        base.Start();
        FindPlayer();
    }
    
    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            m_playerTransform = playerObj.transform;
        }
    }
}