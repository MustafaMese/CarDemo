using System;
using UnityEngine;

namespace Component
{
    public class WheelComponent
    {
        private readonly float _radius;
        private readonly Transform[] _wheels;
    
        private float _circumference;
    
        public WheelComponent(float radius, Transform[] wheels)
        {
            _radius = radius;
            _wheels = new Transform[wheels.Length];
            Array.Copy(wheels, _wheels, wheels.Length);
        
            _circumference = 2 * Mathf.PI * _radius;
        }

        public void Rotate(float distance)
        {
            _circumference = 2 * Mathf.PI * _radius;
        
            for (int i = 0; i < _wheels.Length; i++)
            {
                var wheel = _wheels[i];
                wheel.RotateAround(wheel.position, Vector3.right, distance / _circumference);
            }
        }
    }
}
