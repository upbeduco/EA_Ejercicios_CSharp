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
            // TODO Hacer mas inteligente la validacion de dia para que tenga en cuenta los dias de cada mes
            // TODO* Tener en cuenta que febrero en año biciesto puede tener 29 dias
            set
            {
                if ((value>0) && (value<32)) {
                    _dia = value;
                }
            }
        }
        public int Mes {
            get => _mes;
            set
            {
                if ((value>0) && (value<13)) {
                    _mes = value;
                }
            }
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

        public int diaDelAño() {
            // TODO Calcular el dia del año correspondiente a la fecha
            return default;
        }

        public static Boolean isLeapYear(int año) {
            // TODO Implementar la función de biblioteca para determinar si una año es biciesto
            return default;
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

// Ejercicios:
// ==========
// 1. Completar la implementacion del ADT
// 2. Asegurar la encapsulacion del valor del ADT
// 3. Implementar la operación (API) diaDelAño
// 4. Sobre-escribir los métodos toString, equals heredados de Object (✓)
// 5. Implementar un main que ejemplifique el uso del ADT (✓)
// 6. Hacer una aplicación "cliente" que calcule cuantos dias faltan para el cumpleaños
// 6. Implementar un método estático "Factory" que cree instancias a partir de datos ingresados por consola
// 7. Implementar la interface Comparable y hacerle pruebas unitarias
