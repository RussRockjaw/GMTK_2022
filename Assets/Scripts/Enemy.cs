using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int Health{get; set;}
    public int MaxHealth{get; set;}
    
    public void Kill()
    {
        transform.gameObject.SetActive(false);
    }

    public void Respawn(Vector3 position)
    {
        transform.position = position;
        Health = MaxHealth;
    }

    public virtual void Resume()
    {
        
    }

    public virtual void Delete()
    {
        Destroy(this.gameObject);
    }
}
