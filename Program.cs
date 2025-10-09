/*
    - lista cercuri
    - operatii:
        - calculeaza aria totala ca suma ariilor individuale
        - raporteaza cercurile cresator dupa marime
        - raporteaza cercurile de la stanga la dreapta
        - raporteaza cercurile intersectate cu un punct dat
        - elimina cercurile mai mici decat o arie data

V2
    - Vreau sa adaug suport pentru dreptunghiuri
*/
namespace EditorGrafic
{
    abstract class FormaGeometrica
    {
        public abstract double CalculeazaAria();
        public abstract bool ContinePunct(Punct p);

        public double Raza { get; set; }
        public Punct? Centru { get; set; }
    }

    // Clasa care reprezinta un punct in planul 2D, avand coordonatele X si Y
    class Punct : FormaGeometrica
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

        public override double CalculeazaAria()
        {
            // Un punct nu are arie
            return 0.0;
        }

        public override bool ContinePunct(Punct p)
        {
            // Un punct contine doar el insusi
            return this.X == p.X && this.Y == p.Y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    // Clasa care reprezinta un cerc in planul 2D, avand un centru (de tip Punct) si o raza
    class Cerc : FormaGeometrica
    // Constructor pentru initializarea unui cerc cu centru si raza
    // Calculeaza aria cercului folosind formula π * r^2
    // Verifica daca un punct dat se afla in interiorul sau pe cerc
    // Suprascrie metoda ToString pentru afisare prietenoasa a cercului
    {
        //public Punct Centru { get; set; }
        //public double Raza { get; set; }

        public Cerc(Punct centru, double raza)
        {
            Centru = centru;
            Raza = raza;
        }

        public override double CalculeazaAria()
        {
            return Math.PI * Raza * Raza;
        }

        public override bool ContinePunct(Punct p)
        {
            if (Centru == null)
                return false;
            double dx = p.X - Centru.X;
            double dy = p.Y - Centru.Y;
            return dx * dx + dy * dy <= Raza * Raza;
        }

        public override string ToString()
        {
            return $"Cerc(Centru={Centru}, Raza={Raza:F2}, Aria={CalculeazaAria():F2})";
        }
    }
    class Dreptunghi : FormaGeometrica
    {
        public Punct ColtStangaSus { get; set; }
        public Punct ColtStangaJos { get; set; }
        public Punct ColtDreaptaSus { get; set; }
        public Punct ColtDreaptaJos { get; set; }
        public double Lungime { get; set; }
        public double Inaltime { get; set; }

        public Dreptunghi(Punct ColtStangaSus, Punct ColtStangaJos, Punct ColtDreaptaSus, Punct ColtDreaptaJos)
        {
            this.ColtStangaSus = ColtStangaSus;
            this.ColtStangaJos = ColtStangaJos;
            this.ColtDreaptaSus = ColtDreaptaSus;
            this.ColtDreaptaJos = ColtDreaptaJos;
            Lungime = ColtDreaptaSus.X - ColtStangaSus.X;
            Inaltime = ColtStangaSus.Y - ColtStangaJos.Y;
        }

        public override double CalculeazaAria()
        {
            return Lungime * Inaltime;
        }

        public override bool ContinePunct(Punct p)
        {
            return (p.X >= ColtStangaSus.X && p.X <= ColtDreaptaSus.X &&
                    p.Y <= ColtStangaSus.Y && p.Y >= ColtStangaJos.Y);
        }

        public override string ToString()
        {
            return $"Dreptunghi(ColtStangaSus={ColtStangaSus}, ColtStangaJos={ColtStangaJos}, ColtDreaptaSus={ColtDreaptaSus}, ColtDreaptaJos={ColtDreaptaJos}, Lungime={Lungime:F2}, Inaltime={Inaltime:F2}, Aria={CalculeazaAria():F2})";
        }

    }
    internal class Program
    {

        // Calculeaza aria totala a tuturor cercurilor din lista primita ca parametru
        // Parcurge fiecare cerc si aduna aria acestuia la suma totala
        static double calculeazaAriaTotala(List<FormaGeometrica> forme)
        {
            double ariaTotalaCerc = 0;
            double ariaTotalaDreptunghi = 0;
            double ariaTotala;
            foreach (var cerc in forme)
            {
                ariaTotalaCerc += cerc.CalculeazaAria();
            }

            foreach (var dreptunghi in forme)
            {
                ariaTotalaDreptunghi += dreptunghi.CalculeazaAria();
            }

            return ariaTotala = ariaTotalaCerc + ariaTotalaDreptunghi;

        }

        // Sorteaza lista de cercuri crescator dupa raza si afiseaza fiecare cerc
        // Se foloseste metoda Sort cu un comparator pe raza
        static void raporteazaCrescator(List<FormaGeometrica> forme)
        {
            forme.Sort((f1, f2) =>
            {
                double val1 = 0, val2 = 0;
                if (f1 is Cerc c1)
                    val1 = c1.Raza;
                else if (f1 is Dreptunghi d1)
                    val1 = d1.Lungime;

                if (f2 is Cerc c2)
                    val2 = c2.Raza;
                else if (f2 is Dreptunghi d2)
                    val2 = d2.Lungime;

                return val1.CompareTo(val2);
            });
            Console.WriteLine("-> Forme ordonate crescator dupa raza (cercuri) sau lungime (dreptunghiuri):");
            foreach (var forma in forme)
            {
                Console.WriteLine(forma);
            }
        }

        // Sorteaza lista de cercuri dupa coordonata X a centrului (de la stanga la dreapta)
        // si afiseaza fiecare cerc
        static void raporteazaDeLaStangaLaDreapta(List<FormaGeometrica> forme)
        {
            forme.Sort((f1, f2) =>
            {
                double x1 = double.MinValue;
                double x2 = double.MinValue;

                // Pentru Cerc, folosim X-ul centrului
                if (f1 is Cerc && f1.Centru != null)
                    x1 = f1.Centru.X;
                // Pentru Dreptunghi, folosim centrul geometric pe X
                else if (f1 is Dreptunghi d1)
                    x1 = (d1.ColtStangaSus.X + d1.ColtDreaptaJos.X) / 2.0;

                if (f2 is Cerc && f2.Centru != null)
                    x2 = f2.Centru.X;
                else if (f2 is Dreptunghi d2)
                    x2 = (d2.ColtStangaSus.X + d2.ColtDreaptaJos.X) / 2.0;

                return x1.CompareTo(x2);
            });

            Console.WriteLine("-> Forme de la stanga la dreapta (dupa X):");
            foreach (var forma in forme)
            {
            Console.WriteLine(forma);
            }
        }

        // Elimina din lista toate cercurile care au aria mai mica decat valoarea data
        // Se foloseste metoda RemoveAll cu o conditie pe aria cercului
        static void eliminaCercuriMaiMiciDecatOArie(List<FormaGeometrica> forme, double aria) //Aici am folosit testul de tip
        {
            forme.RemoveAll(f => {
                if (f is Cerc c)
                    return c.CalculeazaAria() < aria;
                else if (f is Dreptunghi d)
                    return d.CalculeazaAria() < aria;
                else
                    return false;
            });
        }

        // Afiseaza toate cercurile care contin punctul dat ca parametru
        // Pentru fiecare cerc, se verifica daca punctul se afla in interiorul sau pe cerc
        static void raporteazaIntersectateCuPunct(List<FormaGeometrica> forme, Punct punct)
        {
            Console.WriteLine($"-> Forme care contin punctul {punct}:");
            foreach (var forma in forme)
            {
                if (forma.ContinePunct(punct))
                {
                    Console.WriteLine(forma);
                }
            }
        }

        // Punctul de intrare in program
        // Creeaza o lista de cercuri, afiseaza aria totala, raporteaza diverse informatii
        // si elimina cercurile cu aria sub o anumita valoare
        static void Main(string[] args)
        {
            // Creeaza o lista de cercuri cu pozitii si raze diferite
            List<FormaGeometrica> forme = new List<FormaGeometrica>()
            {
                new Dreptunghi(new Punct(0, 4), new Punct(0, 0), new Punct(6, 4), new Punct(6, 0)), // Dreptunghi cu colturi in (0,4), (0,0), (6,4), (6,0)
                new Cerc(new Punct(0, 0), 2),      // Cerc cu centru in (0,0) si raza 2
                new Cerc(new Punct(5, 1), 3),      // Cerc cu centru in (5,1) si raza 3
                new Cerc(new Punct(-2, 4), 1.5),   // Cerc cu centru in (-2,4) si raza 1.5
                new Cerc(new Punct(3, 3), 2.5)     // Cerc cu centru in (3,3) si raza 2.5
            };

            // Afiseaza aria totala a tuturor formelor
            Console.WriteLine($"-> Aria totala este {calculeazaAriaTotala(forme):F2}");

            // Afiseaza formele ordonate crescator dupa raza
            raporteazaCrescator(forme);

            // Afiseaza formele ordonate de la stanga la dreapta dupa coordonata X a centrului
            raporteazaDeLaStangaLaDreapta(forme);

            // Elimina formele cu aria mai mica decat 10.0 si afiseaza lista ramasa
            eliminaCercuriMaiMiciDecatOArie(forme, 10.0);
            Console.WriteLine("-> Dupa eliminare forme cu aria < 10.0:");
            foreach (var forma in forme)
            {
                if (forma is Cerc)
                    Console.WriteLine(forma);
            }
            foreach (var forma in forme)
            {
                if (forma is Dreptunghi)
                    Console.WriteLine(forma);
            }

            // Afiseaza formele care contin punctul (3,4)
            raporteazaIntersectateCuPunct(forme, new Punct(3, 4));
        }
    }
}