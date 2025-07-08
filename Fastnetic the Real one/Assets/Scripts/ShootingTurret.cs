using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootingTurret : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab; 
    [SerializeField] private float _range = 10f;
	[SerializeField] private float _fireRate = 1f; 
	[SerializeField] private float _bulletTimeDestroy = 2f; 
	[SerializeField] private float _force = 10f; 
	private List<GameObject> _objectsToDestroy = new List<GameObject>();
	private SoundManager SoundManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _objectsToDestroy = GameObject.FindGameObjectsWithTag("Breakable").ToList();
		StartCoroutine(ShootObjects());
        SoundManager = FindFirstObjectByType<SoundManager>();
    }

	private IEnumerator ShootObjects()
	{
		while (true)
		{
			foreach (GameObject obj in _objectsToDestroy)
			{
				if (obj != null)
				{
					float distance = Vector3.Distance(transform.position, obj.transform.position);
					if (distance <= _range)
					{
						Shoot(obj);
						break; // Exit the loop after shooting one object
					}
				}
			}
			yield return new WaitForSeconds(_fireRate);
		}
	}

	private void Shoot(GameObject obj)
	{
		if (obj != null)
		{
            SoundManager.PlayFull("Shooting");
            Vector3 direction = (obj.transform.position - transform.position).normalized;
			GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
			projectile.GetComponent<Rigidbody2D>().AddForce(direction * _force);
			Destroy(projectile, _bulletTimeDestroy); // Destroy the projectile after 2 seconds
		}
	}
}
