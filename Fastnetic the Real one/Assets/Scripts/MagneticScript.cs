using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections;

public class MagneticScript : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputScript _inputScript;
    [Header("Range Object")]
    [SerializeField] private GameObject _rangeObject;
    [SerializeField] Transform _pointA;
    [SerializeField] Transform _pointB;
    [SerializeField] private float _maxDistance = 8f;
    [SerializeField] private float _minDistance = 2f;

    [SerializeField] private float _magneticForceStrength = 10f;
    [SerializeField] private float _attractionForceStrength = 10f;

    [SerializeField] private Color _magneticColor = Color.blue;
    [SerializeField] private Color _repelColor = Color.red;
    [SerializeField] private Color _defaultColor = Color.white;

    [SerializeField] private Animator _animator;

	private float _angleVelocity;
    private float _smoothTime = 0.1f;
    List<MagneticObjectsScript> _magneticObjects = new List<MagneticObjectsScript>();
    [SerializeField] private SoundManager _soundManager;
    bool _playedSound = false;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        _soundManager = FindFirstObjectByType<SoundManager>();
        _magneticObjects = FindObjectsByType<MagneticObjectsScript>(FindObjectsSortMode.None).ToList();
        SetRange();
    }
    private void SetRange()
    {
        _rangeObject.transform.localScale = new Vector3(-_maxDistance / 4, _maxDistance / 4, 0);
    }
    private void Update()
    {
        RotateRangeObject();
    }

    private void RotateRangeObject()
    {
        _rangeObject.transform.position = Vector3.Lerp(_rangeObject.transform.position, transform.position, 100f);

        if (_inputScript.ViewInput == Vector2.zero) return; // Check if the joystick is not pressed
        Vector2 joystickDirection = _inputScript.ViewInput.normalized;
        float targetAngle = Mathf.Atan2(joystickDirection.y, joystickDirection.x) * Mathf.Rad2Deg;
        targetAngle = (targetAngle + 360f) % 360f; 
        float currentAngle = _rangeObject.transform.eulerAngles.z;
        currentAngle = (currentAngle + 360f) % 360f;
        float angle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref _angleVelocity, _smoothTime);
        angle = (angle + 360f) % 360f; 
        _rangeObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_inputScript.MagneticInput)
        {
            StartMechanic(_magneticForceStrength, _attractionForceStrength, _minDistance, _magneticColor);
            _animator.SetBool("Attract", true);
			_animator.SetBool("Repel", false);
			if (!_playedSound)
			{
				_soundManager.Play("Magnetic");
				_playedSound = true;
			}
		}
		else if (_inputScript.RepelInput)
        {
            StartMechanic(-_magneticForceStrength, -_attractionForceStrength, 0, _repelColor);
            _animator.SetBool("Repel", true);
			_animator.SetBool("Attract", false);
            if (!_playedSound)
            {
                _soundManager.Play("Magnetic");
				_playedSound = true;
			}
		}
		else
        {
            foreach (MagneticObjectsScript magneticObject in _magneticObjects)
            {
                if (magneticObject != null)
                {
                    magneticObject.Material.SetColor("_SolidOutline", _defaultColor);
                }
            }
            _animator.SetBool("Repel", false);
            _animator.SetBool("Attract", false);
			_playedSound = false;
			_soundManager.Stop("Magnetic");
		}
    }
	private void StartMechanic(float magneticStrength, float attarctingStrength, float minDistance, Color color)
    {
        List<GameObject> inrangeM = new List<GameObject>();
        List<GameObject> inrangeI = new List<GameObject>();

        foreach (MagneticObjectsScript magneticObject in _magneticObjects)
        {
            if (magneticObject != null)
            {
                float distance = Vector2.Distance(transform.position, magneticObject.transform.position);
                if (IsBetweenPoints(magneticObject.Collider.ClosestPoint(this.transform.position)) && distance > minDistance)
                {
                    Debug.Log("In range: " + magneticObject.gameObject.name);
                    if (magneticObject.GotRepellet)
                    {
                        continue;
                    }
                    else if (magneticObject.IsAttractor)
                    {
                        inrangeI.Add(magneticObject.gameObject);
                    }
                    else
                    {
                        inrangeM.Add(magneticObject.gameObject);
                    }
                }
                else
                {
                    magneticObject.Material.SetColor("_SolidOutline", _defaultColor);
                }
            }
        }
        

        foreach (GameObject attractor in inrangeM)
        {
            float distance = Vector3.Distance(attractor.transform.position, this.transform.position);
            float distanceFactor = 1f - (distance / _maxDistance);
            distanceFactor = Mathf.Clamp01(distanceFactor);

            Vector3 direction = (transform.position - attractor.transform.position).normalized;
            attractor.GetComponent<Rigidbody2D>().AddForce(direction * magneticStrength  /*distanceFactor*/);

            attractor.GetComponent<MagneticObjectsScript>().Material.SetColor("_SolidOutline", color);
        }

        Vector3 playerDirection = Vector3.zero;
        foreach (GameObject attractor in inrangeI)
        {
            playerDirection += (attractor.transform.position - transform.position).normalized;
            attractor.GetComponent<MagneticObjectsScript>().Material.SetColor("_SolidOutline", color);
        }
        playerDirection.Normalize();
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(playerDirection * attarctingStrength);
    }

    private bool IsBetweenPoints(Vector2 objectPosition)
    {
        Vector2 pointA = _pointA.position;
        Vector2 pointB = _pointB.position;
        Vector2 pointC = this.transform.position;

        return PointInTriangle(objectPosition, pointA, pointB, pointC);
    }
    private bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        float denominator = ((b.y - c.y) * (a.x - c.x) + (c.x - b.x) * (a.y - c.y));
        float alpha = ((b.y - c.y) * (p.x - c.x) + (c.x - b.x) * (p.y - c.y)) / denominator;
        float beta = ((c.y - a.y) * (p.x - c.x) + (a.x - c.x) * (p.y - c.y)) / denominator;
        float gamma = 1.0f - alpha - beta;

        return alpha >= 0 && beta >= 0 && gamma >= 0;
    }
    private void OnDrawGizmos()
    {
        if (_rangeObject != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _maxDistance);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _minDistance);
        }
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
		var magneticObject = collision.gameObject.GetComponent<MagneticObjectsScript>();
		if (magneticObject == null) return;

		StartCoroutine(TemporarilyRepel(magneticObject));
	}

	private IEnumerator TemporarilyRepel(MagneticObjectsScript magnetic)
	{
		if (magnetic == null) yield break;

		magnetic.GotRepellet = true;
		yield return new WaitForSeconds(0.5f);
		magnetic.GotRepellet = false;
	}
}
