using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxForce = 0.5f;
    [SerializeField] private float separationDistance = 1.5f;
    [SerializeField] private float obstacleAvoidanceDistance = 5f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float avoidForce = 2f;
    [SerializeField] private Transform target;  // Target for boids to follow

    private List<AnimalController> neighbors = new();
    private Vector3 velocity;
    private Vector3 acceleration;
    private float yPosition;  // Store the initial Y position
    private float speedMultiplier = 2f;

    private void Start()
    {
        velocity = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)) * maxSpeed;
        yPosition = transform.position.y;  // Store the initial Y position to keep boids on the same plane
    }

    private void Update()
    {
        Vector3 separation = Separate(neighbors) * 1.5f;
        Vector3 alignment = Align(neighbors) * 0f;
        Vector3 cohesion = Cohere(neighbors) * 1f;
        Vector3 avoidance = AvoidObstacles() * avoidForce;
        Vector3 seekTarget = SeekTarget() * 1f;

        // Apply behaviors
        ApplyForce(separation);
        ApplyForce(alignment);
        ApplyForce(cohesion);
        ApplyForce(avoidance);
        ApplyForce(seekTarget);

        // Move the boid (restricted to X and Z)
        Move();
    }

    public void SetNeighbors(List<AnimalController> neighbors)
    {
        this.neighbors = neighbors;
    }

    private Vector3 Separate(List<AnimalController> neighbors)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            float distance = Vector3.Distance(transform.position, neighbor.transform.position);
            if (distance < separationDistance)
            {
                Vector3 diff = transform.position - neighbor.transform.position;
                diff.Normalize();
                if(distance>0)
                    diff /= distance;
                steer += diff;
                count++;
            }
        }

        if (count > 0)
        {
            steer /= count;
        }

        if (steer.magnitude > 0)
        {
            steer = steer.normalized * maxSpeed - velocity;
            steer = Vector3.ClampMagnitude(steer, maxForce);
        }

        return steer;
    }

    private Vector3 Align(List<AnimalController> neighbors)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            sum += neighbor.velocity;
            count++;
        }

        if (count > 0)
        {
            sum /= count;
            sum = sum.normalized * maxSpeed;
            Vector3 steer = sum - velocity;
            return Vector3.ClampMagnitude(steer, maxForce);
        }

        return Vector3.zero;
    }

    private Vector3 Cohere(List<AnimalController> neighbors)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            sum += neighbor.transform.position;
            count++;
        }

        if (count > 0)
        {
            sum /= count;
            return Seek(sum);
        }

        return Vector3.zero;
    }

    private Vector3 SeekTarget()
    {
        if (target != null)
        {
            return Seek(target.position)*Vector3.Distance(transform.position, target.position)/10;
        }
        return Vector3.zero;
    }

    private Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired = desired.normalized * maxSpeed;
        Vector3 steer = desired - velocity;
        return Vector3.ClampMagnitude(steer, maxForce);
    }

    private Vector3 AvoidObstacles()
    {
        Vector3 avoidanceForce = Vector3.zero;

        // Cast rays in the boid's current forward direction to check for obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, velocity.normalized, out hit, obstacleAvoidanceDistance, obstacleMask))
        {
            // Calculate a direction to steer away from the obstacle
            Vector3 avoidDirection = Vector3.Reflect(velocity, hit.normal);
            avoidanceForce = avoidDirection.normalized * maxSpeed;
            avoidanceForce -= velocity;
            avoidanceForce = Vector3.ClampMagnitude(avoidanceForce, maxForce);
        }

        return avoidanceForce;
    }

    private void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Move()
    {
        velocity += acceleration;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Constrain velocity to X and Z axes (set Y to 0)
        velocity.y = 0;

        // Apply movement (only in X and Z axes)
        transform.position += velocity * speedMultiplier * Time.deltaTime;

        // Keep the Y position constant
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);

        /* Rotate to face the direction it's moving (ignore Y rotation)
        if (velocity != Vector3.zero)
        {
            Vector3 flatForward = new Vector3(velocity.x, 0, velocity.z).normalized;
            transform.forward = flatForward;
        }*/

        acceleration = Vector3.zero;
    }

    internal void SetSpeed(float v)
    {
        speedMultiplier = v;
    }
}
