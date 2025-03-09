using System;
using System.Collections.Generic;
using CustomMath;
using MathDebbuger;
using UnityEngine;

public class CustomExercises : MonoBehaviour
{
    private enum Exercise
    {
        Uno,
        Dos,
        Tres,
        Cuatro,
        Cinco,
        Seis,
        Siete,
        Ocho,
        Nueve,
        Diez
    }

    [SerializeField] private Exercise exercise;
    [SerializeField] private Color vectorColor;
        
    [SerializeField] private Vec3 a;
    [SerializeField] private Vec3 b;
    
    private Vec3 c;

    private float t;

    private Dictionary<Exercise, Func<Vec3>> exerciseMap;

    private void Awake()
    {
        exerciseMap = new Dictionary<Exercise, Func<Vec3>>
        {
            { Exercise.Uno, Exercise1 },
            { Exercise.Dos, Exercise2 },
            { Exercise.Tres, Exercise3 },
            { Exercise.Cuatro, Exercise4 },
            { Exercise.Cinco, () =>
                {
                    Vec3 result = Exercise5();
                    if (result == b) result = a;
                    return result;
                }
            },
            { Exercise.Seis, Exercise6 },
            { Exercise.Siete, Exercise7 },
            { Exercise.Ocho, Exercise8 },
            { Exercise.Nueve, Exercise9 },
            { Exercise.Diez, Exercise10 }
        };
    }

    private void Start()
    {
        Vector3Debugger.AddVector(a, Color.white, "Vector A");
        Vector3Debugger.AddVector(b, Color.black, "Vector B");
        Vector3Debugger.AddVector(c, vectorColor, "Vector C");
        
        Vector3Debugger.EnableEditorView("Vector A");
        Vector3Debugger.EnableEditorView("Vector B");
        Vector3Debugger.EnableEditorView("Vector C");
    }

    private void Update()
    {
        t += Time.deltaTime;
        t %= 1f;
        
        if (exerciseMap.TryGetValue(exercise, out Func<Vec3> getExercise))
        {
            c = getExercise();
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
        
        Vector3Debugger.UpdatePosition("Vector A", a);
        Vector3Debugger.UpdatePosition("Vector B", b);
        Vector3Debugger.UpdatePosition("Vector C", c);
    }

    private Vec3 Exercise1()
    {
        // Sum of A and B
        return new Vec3(a + b);
    }

    private Vec3 Exercise2()
    {
        // C is the difference between A and B
        return new Vec3(b - a);
    }

    private Vec3 Exercise3()
    {
        // C is the product of A and B
        return new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    private Vec3 Exercise4()
    {
        // Vec C parallel || to A and B 
        return Vec3.Cross(b, a);
    }

    private Vec3 Exercise5()
    {
        return Vec3.Lerp(a, b, t);
    }

    private Vec3 Exercise6()
    {
        // Vec C has the greatest value of each vec
        return Vec3.Max(a, b);
    }

    private Vec3 Exercise7()
    {
        // Vec C is the projection of A on B
        return Vec3.Project(a, b);
    }

    private Vec3 Exercise8()
    {
        // Vec C is the middle point of vec A and B with the magnitude of the distance of A to B
        return Vec3.Lerp(a, b, 0.5f).normalized * Vec3.Distance(a, b);
    }

    private Vec3 Exercise9()
    {
        // Calculates the reflection of vec A over B
        return Vec3.Reflect(a, b.normalized);
    }

    private Vec3 Exercise10()
    {
        // 5, 6, 7
        // -25, -24, -23
        
        // -4, -3, -2
        // 56, 57, 58
        Vec3 cross = Vec3.Cross(a, b);

        // Reflection of the cross product in the plane perpendicular to A
        Vec3 reflection = Vec3.Reflect(b, a);
        
        return Vec3.Lerp(b, reflection, t);
    }
}
