/*
    -lista cercuri
    -operatii:
        -calculeaza aria totala ca suma ariilor individuale
        -raporteaza cercurile cresator dupa marime
        -raporteaza cercurile de la stanga la dreapta
        -raporteaza cercurile intersectate cu un punct dat
        -elimina cercurile mai mici decat o arie data
*/
namespace EditorGrafic
{
    // Clasa care reprezinta un punct in planul 2D, avand coordonatele X si Y
    class Punct
    // Constructor pentru initializarea unui punct cu coordonatele date
    // Suprascrie metoda ToString pentru afisare prietenoasa a punctului
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Punct(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    // Clasa care reprezinta un cerc in planul 2D, avand un centru (de tip Punct) si o raza
    class Cerc
    // Constructor pentru initializarea unui cerc cu centru si raza
    // Calculeaza aria cercului folosind formula π * r^2
    // Verifica daca un punct dat se afla in interiorul sau pe cerc
    // Suprascrie metoda ToString pentru afisare prietenoasa a cercului
    {
        public Punct Centru { get; set; }
        public double Raza { get; set; }

        public Cerc(Punct centru, double raza)
        {
            Centru = centru;
            Raza = raza;
        }

        public double CalculeazaAria()
        {
            return Math.PI * Raza * Raza;
        }

        public bool ContinePunct(Punct p)
        {
            double dx = p.X - Centru.X;
            double dy = p.Y - Centru.Y;
            return dx * dx + dy * dy <= Raza * Raza;
        }

        public override string ToString()
        {
            return $"Cerc(Centru={Centru}, Raza={Raza:F2}, Aria={CalculeazaAria():F2})";
        }
    }
    internal class Program
    {

    // Calculeaza aria totala a tuturor cercurilor din lista primita ca parametru
    // Parcurge fiecare cerc si aduna aria acestuia la suma totala
    static double calculeazaAriaTotala(List<Cerc> cercuri)
        {
            double ariaTotala = 0;
            foreach (var cerc in cercuri)
            {
                ariaTotala += cerc.CalculeazaAria();
            }
            return ariaTotala;
        }

    // Sorteaza lista de cercuri crescator dupa raza si afiseaza fiecare cerc
    // Se foloseste metoda Sort cu un comparator pe raza
    static void raporteazaCrescator(List<Cerc> cercuri)
        {
            cercuri.Sort((c1, c2) => c1.Raza.CompareTo(c2.Raza));
            Console.WriteLine("Cercuri ordonate crescator dupa raza:");
            foreach (var cerc in cercuri)
            {
                Console.WriteLine(cerc);
            }
        }

    // Sorteaza lista de cercuri dupa coordonata X a centrului (de la stanga la dreapta)
    // si afiseaza fiecare cerc
    static void raporteazaDeLaStangaLaDreapta(List<Cerc> cercuri)
        {
            cercuri.Sort((c1, c2) => c1.Centru.X.CompareTo(c2.Centru.X));
            Console.WriteLine("Cercuri de la stanga la dreapta (dupa X):");
            foreach (var cerc in cercuri)
            {
                Console.WriteLine(cerc);
            }
        }

    // Elimina din lista toate cercurile care au aria mai mica decat valoarea data
    // Se foloseste metoda RemoveAll cu o conditie pe aria cercului
    static void eliminaCercuriMaiMiciDecatOArie(List<Cerc> cercuri, double aria)
        {
            cercuri.RemoveAll(c => c.CalculeazaAria() < aria);
        }

    // Afiseaza toate cercurile care contin punctul dat ca parametru
    // Pentru fiecare cerc, se verifica daca punctul se afla in interiorul sau pe cerc
    static void raporteazaIntersectateCuPunct(List<Cerc> cercuri, Punct punct)
        {
            Console.WriteLine($"Cercuri care contin punctul {punct}:");
            foreach (var cerc in cercuri)
            {
                if (cerc.ContinePunct(punct))
                {
                    Console.WriteLine(cerc);
                }
            }
        }

    // Punctul de intrare in program
    // Creeaza o lista de cercuri, afiseaza aria totala, raporteaza diverse informatii
    // si elimina cercurile cu aria sub o anumita valoare
    static void Main(string[] args)
        {
            // Creeaza o lista de cercuri cu pozitii si raze diferite
            List<Cerc> cercuri = new List<Cerc>()
            {
                new Cerc(new Punct(0, 0), 2),      // Cerc cu centru in (0,0) si raza 2
                new Cerc(new Punct(5, 1), 3),      // Cerc cu centru in (5,1) si raza 3
                new Cerc(new Punct(-2, 4), 1.5),   // Cerc cu centru in (-2,4) si raza 1.5
                new Cerc(new Punct(3, 3), 2.5)     // Cerc cu centru in (3,3) si raza 2.5
            };

            // Afiseaza aria totala a tuturor cercurilor
            Console.WriteLine($"Aria totala este {calculeazaAriaTotala(cercuri):F2}");

            // Afiseaza cercurile ordonate crescator dupa raza
            raporteazaCrescator(cercuri);

            // Afiseaza cercurile ordonate de la stanga la dreapta dupa coordonata X a centrului
            raporteazaDeLaStangaLaDreapta(cercuri);

            // Elimina cercurile cu aria mai mica decat 10.0 si afiseaza lista ramasa
            eliminaCercuriMaiMiciDecatOArie(cercuri, 10.0);
            Console.WriteLine("Dupa eliminare cercuri cu aria < 10.0:");
            foreach (var cerc in cercuri)
                Console.WriteLine(cerc);

            // Afiseaza cercurile care contin punctul (3,4)
            raporteazaIntersectateCuPunct(cercuri, new Punct(3, 4));
        }
    }
}