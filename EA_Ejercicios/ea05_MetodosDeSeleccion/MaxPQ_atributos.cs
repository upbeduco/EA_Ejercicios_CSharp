namespace EA_UPB
{

    /**
     *  ADT Mascota
     */
    class Mascota: IComparable<Mascota> 
    {
        private String nombre;
        private float peso;
        private int edad;
        public Mascota(String n, float p, int e)
        {
            nombre = n;
            peso = p;
            edad = e;
        }
        public String GetNombre() { return nombre; }
        public float GetPeso() { return peso; }
        public int GetEdad() { return edad; }

        public int CompareTo(Mascota? other)
        {
            if (other==null) throw new Exception("Referencia other es nula");
            return this.GetEdad()-other.GetEdad();
        }
    }


    static void maxPorEdad(Mascota[] pets)
    {
        MaxPQ<Mascota> pq = new MaxPQ<>(new ComparatorEdad());
        for (Mascota m: pets)
            pq.insert(m);
        StdOut.println("Mascota de mas edad: " + pq.max().getNombre());
    }


    static void maxPorPeso(Mascota[] pets)
    {
        Comparator<Mascota> comparadorPeso = (m1, m2)-> {
            float diff = m1.getPeso() - m2.getPeso();
            if (diff < 0) return -1;
            else if (diff > 0) return 1;
            else return 0;
        }; // Interfaz funcional
        MaxPQ<Mascota> pq = new MaxPQ<>(comparadorPeso);
        for (Mascota m: pets)
            pq.insert(m);
        StdOut.println("Mascota de mas peso: " + pq.max().getNombre());
    }


    public static void main(String[] args)
    {
        Mascota[] mascotas = {
            new Mascota("Firulais", 15.3f, 2),
            new Mascota("Santa's little helper", 5.1f, 3),
            new Mascota("Gardfield", 8.4f, 5),
            new Mascota("Snoopy", 3.8f, 4)
        };
        maxPorPeso(mascotas);
        maxPorEdad(mascotas);
    }



}