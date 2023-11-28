using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSim : MonoBehaviour
{
    public Material circleMaterial;

    public float particleSize = 0.1f;
    public float collisionDamp = 0f;

    public float gravity = -9.8f;
    public int numParticles = 300;
    public float particleSpacing = 0.05f;
    public int particlesPerRow = 100;

    public float minSpeed = 0f;

    public float maxSpeed = 10f;

    public float initialRadius = 1f;

    public Color minColor = Color.blue;
    public Color maxColor = Color.red; 

    
    GameObject[] circleQuads;
    Vector2[] positions;
    Vector2[] velocities;


    public Vector2 boundsSize = new Vector2(10, 10);
    
    // Start is called before the first frame update
    void Start()
    {
       positions = new Vector2[numParticles];
       velocities = new Vector2[numParticles];
       circleQuads = new GameObject[numParticles];

       int particlesPerRow = (int)Mathf.Sqrt(numParticles);
       int particlesPerCol = (numParticles - 1) / particlesPerRow + 1;
       float spacing = particleSpacing;

       for (int i = 0; i < numParticles; i++) {
            float angle = 2 * Mathf.PI * i / positions.Length;
            float rx = initialRadius * Mathf.Cos(angle);
            float ry = initialRadius * Mathf.Sin(angle);
           positions[i] = new Vector2(rx, ry);
           circleQuads[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
       }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < positions.Length; i++) {
            positions[i] += velocities[i] * Time.deltaTime;
            float angle = 2 * Mathf.PI * i / positions.Length;
            float vx = Mathf.Cos(angle);
            float vy = Mathf.Sin(angle);
            Vector2 gravityDir = new Vector2(vx, vy);
            velocities[i] += gravityDir * gravity * Time.deltaTime;
            float speed = velocities[i].magnitude;

            Color particleColor;
            if (speed <= minSpeed) {
                particleColor = minColor;
            } else if (speed >= maxSpeed) {
                particleColor = maxColor;
            } else {
                float t = (speed - minSpeed) / (maxSpeed - minSpeed);
                particleColor = Color.Lerp(minColor, maxColor, t);
            }
            ResolveCollisions(i);

            DrawCircle(i, particleSize, particleColor);
        }
    }

    private void DrawCircle(int i, float radius, Color color)
{
    Vector2 position = positions[i];
    GameObject circleQuad = circleQuads[i];
    circleQuad.transform.position = new Vector3(position.x, position.y, 0); // Assuming 2D setup
    circleQuad.transform.localScale = new Vector3(radius * 2, radius * 2, 1);
    circleQuad.GetComponent<Renderer>().material = circleMaterial;
    circleMaterial.SetColor("_MainColor", color);
}

    void ResolveCollisions(int particleIndex) {
        Vector2 position = positions[particleIndex];
        Vector2 velocity = velocities[particleIndex];
        Vector2 halfBoundsSize = boundsSize / 2 - Vector2.one * particleSize;
        if (Mathf.Abs(position.x) > halfBoundsSize.x) {
            position.x = (halfBoundsSize.x - (Mathf.Abs(position.x) - halfBoundsSize.x)) * Mathf.Sign(position.x);
            velocity.x *= -1 * (1 - collisionDamp);
        }
        if (Mathf.Abs(position.y) > halfBoundsSize.y) {
            position.y = (halfBoundsSize.y - (Mathf.Abs(position.y) - halfBoundsSize.y)) * Mathf.Sign(position.y);
            velocity.y *= -1 * (1 - collisionDamp);
        }

        positions[particleIndex] = position;
        velocities[particleIndex] = velocity;
    }
}
