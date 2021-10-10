using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawPrimitives : MonoBehaviour
{
    public static DrawPrimitives instance;
    [SerializeField] bool isOn = true;
    [SerializeField] float time = 5f;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }

    // TODO: Make overload more clever
    public void DrawBall(Vector3 _position) {
        if (this.isOn) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Renderer render = sphere.GetComponent<Renderer>();
            Material material = sphere.GetComponent<Material>();

            sphere.transform.position = _position;
            sphere.transform.localScale = new Vector3(1, 1, 1);
            sphere.layer = 2;

            render.material = new Material(Shader.Find("Specular"));

            StartCoroutine(DestroyAfter(sphere));
        }
    }

    public void DrawBall(Vector3 _position, float _radius)
    {
        if (this.isOn)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Renderer render = sphere.GetComponent<Renderer>();
            Material material = sphere.GetComponent<Material>();

            sphere.transform.position = _position;
            sphere.transform.localScale = new Vector3(_radius * 2, _radius * 2, _radius * 2);
            sphere.layer = 2;

            render.material = new Material(Shader.Find("Specular"));

            StartCoroutine(DestroyAfter(sphere));
        }
    }

    public void DrawBall(Vector3 _position, float _radius, Color _color)
    {
        if (this.isOn)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Renderer render = sphere.GetComponent<Renderer>();
            Material material = sphere.GetComponent<Material>();

            sphere.transform.position = _position;
            sphere.transform.localScale = new Vector3(_radius, _radius, _radius);
            sphere.layer = 2;

            // render.material = new Material(Shader.Find("Specular"));
            render.material.color = _color;

            StartCoroutine(DestroyAfter(sphere));
        }
    }

    // TODO: Comment or use ray length to be the same as "_height"
    public void DrawVerticalRay(Vector3 _newPosition, float _height)
    {
        if (this.isOn)
        {
            Vector3 verticalRay = _newPosition;
            verticalRay.y = _height;

            Debug.DrawRay(verticalRay, new Vector3(0f, -100f, 0f), Color.green, this.time);
        }
    }

    // TODO: Call DrawRay overloads with a single DrawRay function with all the parameters
    public void DrawRay(Vector3 _newPosition, Vector3 _direction)
    {
        if (this.isOn)
        {
            Debug.DrawRay(_newPosition, _direction, Color.red, this.time);
        }
    }

    public void DrawRay(Vector3 _newPosition, Vector3 _direction, float _time)
    {
        if (this.isOn)
        {
            Debug.DrawRay(_newPosition, _direction, Color.red, _time);
        }
    }

    // public void DrawRay(Vector3 _newPosition, Vector3 _direction)
    // {
    //     if (this.isOn)
    //     {
    //         Debug.DrawRay(_newPosition, _direction, Color.red, this.time);
    //     }
    // }

    public void Draw3DRange(Vector3 _center, float _radius)
    {
        if (this.isOn)
        {
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, 0, 0), _center + new Vector3(2 * _radius, 0, 0));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(0, -_radius, 0), _center + new Vector3(0, 2 * _radius, 0));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(0, 0, -_radius), _center + new Vector3(0, 0, 2 * _radius));
        }
    }

    // TODO: Add angled cube
    // TODO: Make overload to draw cube with two points
    // TODO: Make overload to draw prism from one point and 3 vectors
    public void DrawCube(Vector3 _center, float _radius)
    {
        if (this.isOn)
        {
          // z = -1
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, -_radius, -_radius), _center + new Vector3(2 * _radius, 0, 0));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, -_radius, -_radius), _center + new Vector3(0, 2 * _radius, 0));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, _radius, -_radius), _center + new Vector3(2 * _radius, 0, 0));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(_radius, _radius, -_radius), _center + new Vector3(0, -2 * _radius, 0));

          // z = 1
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, -_radius, _radius), _center + new Vector3(2 * _radius, 0, 0));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, -_radius, _radius), _center + new Vector3(0, 2 * _radius, 0));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, _radius, _radius), _center + new Vector3(2 * _radius, 0, 0));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(_radius, _radius, _radius), _center + new Vector3(0, -2 * _radius, 0));

          // y = 1
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, _radius, -_radius), _center + new Vector3(0, 0, 2 * _radius));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(_radius, _radius, -_radius), _center + new Vector3(0, 0, 2 * _radius));

          // y = -1
          DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, -_radius, -_radius), _center + new Vector3(0, 0, 2 * _radius));
          DrawPrimitives.instance.DrawRay(_center + new Vector3(_radius, -_radius, -_radius), _center + new Vector3(0, 0, 2 * _radius));
        }
    }

    public void DrawSquare(Vector3 _center, float _radius) {
        if (this.isOn)
        {
          // DrawPrimitives.instance.DrawRay(_center + new Vector3(-_radius, 0, 0), _center + new Vector3(2 * _radius, 0, 0));
          // DrawPrimitives.instance.DrawRay(_center + new Vector3(0, -_radius, 0), _center + new Vector3(0, 2 * _radius, 0));
          // DrawPrimitives.instance.DrawRay(_center + new Vector3(0, 0, -_radius), _center + new Vector3(0, 0, 2 * _radius));
        }
    }

    // * Internals

    private IEnumerator DestroyAfter(GameObject _gameObject)
    {
        yield return new WaitForSeconds(this.time);
        Destroy(_gameObject);
    }
}
