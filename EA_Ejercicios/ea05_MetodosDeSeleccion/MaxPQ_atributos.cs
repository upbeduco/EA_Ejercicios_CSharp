using Algs4;

namespace EA_UPB
{

    /**
     *  ADT Mascota
     */
    class Mascota : IComparable<Mascota>
    {
        private string _nombre;
        private float _peso;
        private int _edad;
        public Mascota(String n, float p, int e)
        {
            _nombre = n;
            _peso = p;
            _edad = e;
        }
        public string Nombre
        {
            get => _nombre;
        }
        public float Peso
        {
            get => _peso;
            set { _peso = value; }
        }
        public int Edad
        {
            get => _edad;
            set { _edad = value; }
        }

        public int CompareTo(Mascota? other)
        {
            if (other == null) throw new Exception("Referencia other es nula");
            return this.Edad - other.Edad;
        }


        static void MaxPorEdad(Mascota[] pets)
        {
            MaxPQ<Mascota> pq = new MaxPQ<Mascota>(Comparer<Mascota>.Create(
                (m1, m2) => { return m1.Edad - m2.Edad; }
            ));
            foreach (Mascota m in pets)
                pq.Insert(m);
            Console.WriteLine("Mascota de mas edad: " + pq.Max().Nombre);
        }


        static void MaxPorPeso(Mascota[] pets)
        {
            IComparer<Mascota> comparadorPeso = Comparer<Mascota>.Create((m1, m2) =>
            {
                float diff = m1.Peso - m2.Peso;
                if (diff < 0) return -1;
                else if (diff > 0) return 1;
                else return 0;
            }); // Interfaz funcional
            MaxPQ<Mascota> pq = new MaxPQ<Mascota>(comparadorPeso);
            foreach (Mascota m in pets)
                pq.Insert(m);
            Console.WriteLine("Mascota de mas peso: " + pq.Max().Nombre);
        }


        public static void Main()
        {
            Mascota[] mascotas =
            {
            new Mascota("Firulais", 15.3f, 2),
            new Mascota("Santa's little helper", 5.1f, 3),
            new Mascota("Gardfield", 8.4f, 5),
            new Mascota("Snoopy", 3.8f, 4)
        };
            MaxPorPeso(mascotas);
            MaxPorEdad(mascotas);
        }

    }



}