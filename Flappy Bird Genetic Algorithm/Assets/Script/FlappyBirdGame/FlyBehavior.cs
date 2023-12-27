using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float _velocity = 1.5f;
    [SerializeField] private float _jumpStrength = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    public bool wasCollided = false;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.None;
        SetRandomColor();
    }

    private void SetRandomColor()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // Generate random RGB values
            float randomR = Random.value;
            float randomG = Random.value;
            float randomB = Random.value;

            // Create a random color
            Color randomColor = new Color(randomR, randomG, randomB);

            // Set the color to the object's material
            renderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the GameObject.");
        }
    }

    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Jump();
        }
    }

    public void Jump()
    {
        _rb.velocity = Vector2.up * _velocity * _jumpStrength;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, _rb.velocity.y * _rotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collised");
        wasCollided = true;

        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        GameObject pipeCenterTransform = GameObject.FindGameObjectWithTag("PopulationController");
        PopulationController controller = pipeCenterTransform.GetComponent<PopulationController>();
        controller.AddBird(gameObject);

        gameObject.transform.position = new Vector3 ( -10, -10, 0 );
    }
}
