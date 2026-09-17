using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private int initialSize = 10;

    private List<Projectile> projectiles = new();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateProjectile();
        }
    }

    public Projectile GetProjectile()
    {
        foreach (Projectile projectile in projectiles)
        {
            if (!projectile.gameObject.activeSelf)
                return projectile;
        }

        return CreateProjectile();
    }

    public void Release(Projectile projectile)
    {
        projectile.ResetProjectile();
        projectile.gameObject.SetActive(false);
    }

    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab, transform);

        projectile.gameObject.SetActive(false);
        projectile.SetPool(this);

        projectiles.Add(projectile);

        return projectile;
    }
}
