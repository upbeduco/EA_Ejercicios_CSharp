using System.Diagnostics;

namespace EA_UPB {

    /// <summary>
    /// Interfaz de un punto en el plano 2D, independiente de su representación
    /// (cartesiana o polar).
    /// </summary>
    interface IPunto2D
    {
        double GetX();
        double GetY();
        double distancia(IPunto2D punto);
    }

    /// <summary>
    /// Punto 2D en representación cartesiana (x, y).
    /// </summary>
    class Punto2DCartesiano : IPunto2D
    {
        private double _x;
        private double _y;

        const double TOLERANCE = 1E-8;

        public Punto2DCartesiano()
        {
            _x=0.0;
            _y=0.0;
        }

        public Punto2DCartesiano(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>Distancia euclidiana entre este punto y otro.</summary>
        /// <param name="punto">El otro punto.</param>
        /// <returns>La distancia euclidiana.</returns>
        public double distancia(IPunto2D punto)
        {
            return Math.Sqrt((X-punto.GetX())*(X-punto.GetX())+(Y-punto.GetY())*(Y-punto.GetY()));
        }

        public double X
        {
            get => _x;
        }

        public double Y
        {
            get => _y;
        }

        public double GetX() { return X; }
        public double GetY() { return Y; }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override bool Equals(object? obj)
        {
            if (obj==null) return false;
            if (this.GetType()!=obj.GetType()) return false;
            IPunto2D otro = (IPunto2D) obj;
            return (this.distancia(otro)<TOLERANCE);
        }

        // PREGUNTA / DISCUSIÓN:
        // Equals considera "iguales" dos puntos cuya distancia es menor que TOLERANCE,
        // pero GetHashCode se calcula a partir de las coordenadas exactas (ToString).
        // ¿Por qué esto rompe el contrato Equals/GetHashCode?
        // (Pista: dos objetos iguales DEBEN tener el mismo hash. ¿Qué ocurre si se usan
        //  estos puntos como llaves de un Dictionary o elementos de un HashSet?)
        // ¿Cómo se podría diseñar la igualdad y el hash de forma consistente?
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
