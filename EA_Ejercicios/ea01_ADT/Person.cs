namespace EA_UPB
{
    public class Person : IComparable<Person>
    {

        private string _nombres;
        private string _apellidos;
        private int _edad;
        private float _peso;

        public Person(string n, string a, int e, float p)
        {
            _nombres = n;
            _apellidos = a;
            _edad = e;
            _peso = p;
        }

        public string Nombres
        {
            get => _nombres;
        }

        public string Apellidos
        {
            get => _apellidos;
        }

        public int Edad
        {
            get => _edad;
        }

        public float Peso
        {
            get => _peso;
        }

        public override string ToString()
        {
            return Nombres + " " + Apellidos + " : " + Edad + ", " + Peso;
        }

        public int CompareTo(Person? o)
        {
            // TODO Auto-generated method stub
            return 0;
        }


    }

}
