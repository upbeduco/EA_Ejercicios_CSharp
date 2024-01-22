namespace EA_UPB {

    class Fecha {
        private int dia;
        private int mes;
        private int año;

        public Fecha(int año, int mes, int dia) {
            this.año = año;
            this.mes = mes;
            this.dia = dia;
        }



        public static void Main() {
            Fecha f1 = new Fecha(2024,12,31);
            Fecha f2 = new Fecha(2023,01,01);
            Console.WriteLine(f1);

        }

    }

}