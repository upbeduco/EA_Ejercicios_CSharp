using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;

namespace EA_UPB {

    interface IPunto2D
    {
        double GetX();
        double GetY();
        double distancia(IPunto2D punto);
    }

    class Punto2DCartesiano : IPunto2D
    {
        private double x;
        private double y;

        const double TOLERANCE = 1E-8;

        public Punto2DCartesiano()
        {
            x=0.0;
            y=0.0;
        }

        public Punto2DCartesiano(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double distancia(IPunto2D punto)
        {
            return Math.Sqrt((x-punto.GetX())*(x-punto.GetX())+(y-punto.GetY())*(y-punto.GetY()));
        }

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public override bool Equals(Object? obj)
        {
            if (obj==null) return false;
            if (this.GetType()!=obj.GetType()) return false;
            IPunto2D otro = (IPunto2D) obj;
            return (this.distancia(otro)<TOLERANCE);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static void Main()
        {
            // Demo using the object
            IPunto2D p0 = new Punto2DCartesiano();      // The origen
            IPunto2D p1 = new Punto2DCartesiano(4,3);
            Console.WriteLine(p0);
            Console.WriteLine(p1);
            Console.WriteLine($"La distancia p0-p1 es {p0.distancia(p1)}");
        }

        public static void UnitTests()
        {
            // Unit tests
            IPunto2D p0 = new Punto2DCartesiano();      // The origen
            IPunto2D p1 = new Punto2DCartesiano(4,3);
            Debug.Assert(! p0.Equals(p1));
            // Debug.Assert(p0.Equals(p1));             // Este Assert debe fallar
            Debug.Assert(p0.distancia(p1)==5.0);        // Se deben evitar Assert con comparacion exacta de doubles. Por que?

        }


    }


    // TODO Finish implementation of the the class Punto2DPolar
    class Punto2DPolar : IPunto2D
    {
        public double distancia(IPunto2D punto)
        {
            throw new NotImplementedException();
        }

        public double GetX()
        {
            throw new NotImplementedException();
        }

        public double GetY()
        {
            throw new NotImplementedException();
        }
    }


}