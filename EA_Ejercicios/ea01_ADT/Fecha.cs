namespace EA_UPB {

    class Fecha {
        private int dia;
        private int mes;
        private int año;

        public Fecha(int año, int mes, int dia) 
        {
            this.año = año;
            this.mes = mes;
            this.dia = dia;
        }

        public override bool Equals(Object? obj)
        {
            if (obj==null) return false;
            if (this.GetType()!=obj.GetType()) return false;
            Fecha other = (Fecha) obj;
            return this.año==other.año && this.mes==other.mes && this.dia==other.dia;
        }

        public override string ToString()
        {
            return this.año+"-"+this.mes+"-"+this.dia;
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