namespace EA_UPB {

    class Fecha {
        private int _dia;
        private int _mes;
        private int _año;

        public Fecha(int año, int mes, int dia) 
        {
            this._año = año;
            this._mes = mes;
            this._dia = dia;
        }

        public int Dia {
            get => _dia;
        }
        public int Mes {
            get => _mes;
        }
        public int Año {
            get => _año;
        }

        public override bool Equals(Object? obj)
        {
            if (obj==null) return false;
            if (this.GetType()!=obj.GetType()) return false;
            Fecha other = (Fecha) obj;
            return this.Año==other.Año && this.Mes==other.Mes && this.Dia==other.Dia;
        }

        public override string ToString()
        {
            return this.Año+"-"+this.Mes+"-"+this.Dia;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static void Main() 
        {
            Fecha f1 = new Fecha(2024,12,31);
            Fecha f2 = new Fecha(2023,01,01);
            Console.WriteLine(f1);
            Console.WriteLine(f2);
            Console.WriteLine(f1.Equals(f2));
            Console.WriteLine(f1.GetHashCode());

        }

    }

}