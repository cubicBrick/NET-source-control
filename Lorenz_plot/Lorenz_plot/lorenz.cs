using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lorenz
{
    public class LorenzSystem
    {
        private double o;
        private double p;
        private double B;

        public LorenzSystem(double o = 10.0, double p = 28.0, double B = 8.0 / 3.0)
        {
            this.o = o;
            this.p = p;
            this.B = B;
        }

        public List<Tuple<double, double, double>> Solve(double x0, double y0, double z0, double tEnd, int numPoints)
        {
            // Define the differential equations for the Lorenz system
            Func<double, Vector<double>, Vector<double>> lorenzEquations = (t, state) =>
            {
                var x = state[0];
                var y = state[1];
                var z = state[2];
                var dxdt = o * (y - x);
                var dydt = p * x - y - x * z;
                var dzdt = x * y - B * z;
                return Vector<double>.Build.DenseOfArray(new[] { dxdt, dydt, dzdt });
            };

            // Runge-Kutta 4th order method
            var timeStep = tEnd / (numPoints - 1);
            var states = new List<Vector<double>>();
            var currentState = Vector<double>.Build.DenseOfArray(new[] { x0, y0, z0 });

            for (var t = 0.0; t <= tEnd; t += timeStep)
            {
                states.Add(currentState);

                var k1 = lorenzEquations(t, currentState);
                var k2 = lorenzEquations(t + timeStep / 2, currentState + timeStep / 2 * k1);
                var k3 = lorenzEquations(t + timeStep / 2, currentState + timeStep / 2 * k2);
                var k4 = lorenzEquations(t + timeStep, currentState + timeStep * k3);

                currentState += timeStep / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
            }

            return states.Select(v => Tuple.Create(v[0], v[1], v[2])).ToList();
        }
    }
}