using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    [SerializeField] bool horizontalOnly = true;
    [SerializeField] bool verticalFreeze = false;
    public float parallaxEffect;

    private Camera mainCamera;
    private float length, startpos;

    private Transform cameraTransform;

    private Vector3 startPos;

    private void Start()
    {
        SetDefault();
    }

    private void SetDefault()
    {
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        length = GetComponent<SpriteRenderer>().bounds.size.x; // length of sprite
        startPos = transform.position;
        
    }

    private void Update()
    {
        ParallaxEffect();
    }

    private void ParallaxEffect()
    {
        var position = startPos;
        float distance = (cameraTransform.position.x * parallaxEffect);

        if (horizontalOnly) // Only Horizontal Parallax Position 
        {
            if (verticalFreeze) // Parallax Y position follow y camera position
            {
                position.y = cameraTransform.position.y*1f;
            }
            position.x = startpos + distance;
        }
        else { position = parallaxEffect * cameraTransform.position; }

        transform.position = position;

        // Infinite Scrolling
        InfiniteParallaxScrolling();
    }

    private void InfiniteParallaxScrolling()
    {
        float temp = (cameraTransform.position.x * (1f - parallaxEffect));
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
