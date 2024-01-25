namespace EA_UPB {
public class Person : IComparable<Person> {

    private String nombres;
    private String apellidos;
    private int edad;
    private float peso;

    public Person(String n, String a, int e, float p) {
        nombres = n;
        apellidos = a;
        edad = e;
        peso = p;
    }

    public String getNombres() {
        return nombres;
    }

    public String getApellidos() {
        return apellidos;
    }

    public int getEdad() {
        return edad;
    }

    public float getPeso() {
        return peso;
    }

    public override String ToString() {
        return nombres+" "+apellidos+" : "+edad+", "+peso;
    }

    public int CompareTo(Person? o) {
        // TODO Auto-generated method stub
        return 0;
    }
    

}

}
