using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadaca
{
    class Osoba : IComparable<Osoba>   //konstruktori za ime i prezime??
    {

        public string ime;
        public string prezime;
        public int zero_friends;
        public List<Osoba> prijatelji;

        Osoba() { }
        public Osoba(string Ime, string Prezime)
        {
            prijatelji = new List<Osoba>();
            ime = Ime;
            prezime = Prezime;
            zero_friends = 0;
        }

        Osoba(Osoba osoba)
        {
            ime = osoba.ime;
            prezime = osoba.prezime;
            prijatelji = osoba.prijatelji;
        }

        public int brojPrijatelja()
        {
            return prijatelji.Count;
        }

        public int CompareTo(Osoba o)
        {
            return prezime.CompareTo(o.prezime);
        }

        public HashSet<Osoba> Prijatelji()
        {
            return new HashSet<Osoba>(prijatelji);
        }

        public HashSet<Osoba> Meduprijatelji(Osoba drugaOsoba)
        {
            HashSet<Osoba> mojiPrijatelji = new HashSet<Osoba>(this.prijatelji);
            HashSet<Osoba> prijateljiDrugeOsobe = new HashSet<Osoba>(drugaOsoba.prijatelji);
            prijateljiDrugeOsobe.IntersectWith(mojiPrijatelji);

            return prijateljiDrugeOsobe;
        }

        public class OsobaUsporedivac : IComparer<Osoba>
        {

            public enum TipUsporedbe { ime, prezime, brojPrijatelja };
            public Osoba.OsobaUsporedivac.TipUsporedbe tipUsporedbe;

            public int Compare(Osoba prva, Osoba druga)
            {
                return prva.CompareTo(druga, tipUsporedbe);
            }

        }

        public static OsobaUsporedivac DohvatiUsporedivac()
        {
            return new OsobaUsporedivac();
        }

        public int CompareTo(Osoba drugaOsoba, Osoba.OsobaUsporedivac.TipUsporedbe tipUsporedbe)
        {
            switch (tipUsporedbe)
            {
                case Osoba.OsobaUsporedivac.TipUsporedbe.ime:
                    return this.ime.CompareTo(drugaOsoba.ime);
                case Osoba.OsobaUsporedivac.TipUsporedbe.prezime:
                    return this.ime.CompareTo(drugaOsoba.prezime);
                case Osoba.OsobaUsporedivac.TipUsporedbe.brojPrijatelja:
                    return prijatelji.Count.CompareTo(drugaOsoba.brojPrijatelja());
            }
            return 0;
        }

     
        public void DodajPrijatelja(Osoba osoba, int x)
        {
            int i;
            for (i = 0; i < prijatelji.Count; i++)
            {
                if (osoba.ime.CompareTo(prijatelji[i].ime) > 0)
                {
                    prijatelji.Insert(i, osoba);
                    break;
                }
                else
                {
                    if (osoba.ime.CompareTo(prijatelji[i].ime) == 0)
                    {
                        if (osoba.prezime.CompareTo(prijatelji[i].prezime) > 0)
                        {
                            prijatelji.Insert(i, osoba);
                            break;
                        }
                    }
                }
            }
            if (i.Equals(prijatelji.Count))
            {
                prijatelji.Add(osoba);
            }

            if (x.Equals(0))
                osoba.DodajPrijatelja(this, 1);
        }

        public void ObrisiPrijatelja(Osoba osoba, int x)
        {
            for (int i = 0; i < prijatelji.Count; i++)
                if (prijatelji[i] == osoba)
                {
                    prijatelji.RemoveAt(i);
                    break;
                }
            if (x.Equals(0))
                osoba.ObrisiPrijatelja(this, 1);
        }

        public static Osoba operator +(Osoba A, Osoba B)
        {
            A.DodajPrijatelja(B, 0);
            return A;
        }

        public static Osoba operator -(Osoba A, Osoba B)
        {
            A.ObrisiPrijatelja(B, 0);
            return A;
        }

        public override string ToString()
        {
            return ime + " " + prezime;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            else if (obj is Osoba)
            {
                Osoba osoba = obj as Osoba;
                if (this.ime == osoba.ime && this.prezime == osoba.prezime && prijatelji.Count == osoba.prijatelji.Count)
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + ime.GetHashCode();
            hash = (hash * 7) + prezime.GetHashCode();
            return hash;
        }

    }
    class Medurez : IEnumerable<Osoba>
    {

        List<Osoba> lista;

        public Medurez(List<Osoba> A)
        {
            lista = new List<Osoba>();
            while (A.Count != 0)
            {
                int max = 0;
                for (int j = 0; j < A.Count; j++)
                {
                    if (A[j].prijatelji.Count > A[max].prijatelji.Count)
                    {
                        max = j;
                    }
                    else
                    {
                        if (A[j].prijatelji.Count == A[max].prijatelji.Count)
                        {

                            if (A[j].ime.CompareTo(A[max].ime) < 0)
                            {
                                max = j;
                            }
                            else
                            {
                                if (A[j].ime.CompareTo(A[max].ime) == 0)
                                {
                                    if (A[j].prezime.CompareTo(A[max].prezime) < 0)
                                        max = j;
                                }

                            }
                        }
                    }

                }
                lista.Add(A[max]);
                A.RemoveAt(max);

            }

        }

        public IEnumerator<Osoba> GetEnumerator()
        {

            foreach (Osoba o in this.lista)
                yield return o;

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    class Medurezultat : IEnumerable<Osoba>
    {

        List<Osoba> lista;

        internal Medurezultat(List<Osoba> A)
        {
            lista = new List<Osoba>();
            while (A.Count != 0)
            {
                int max = 0;
                for (int j = 0; j < A.Count; j++)
                {
                    if (A[j].prijatelji.Count > A[max].prijatelji.Count)
                    {
                        max = j;
                    }
                    else
                    {
                        if (A[j].prijatelji.Count == A[max].prijatelji.Count)
                        {

                            if (A[j].prezime.CompareTo(A[max].prezime) < 0)
                            {
                                max = j;
                            }
                            else
                            {
                                if (A[j].prezime.CompareTo(A[max].prezime) == 0)
                                {
                                    if (A[j].ime.CompareTo(A[max].ime) < 0)
                                        max = j;
                                }

                            }
                        }
                    }

                }
                lista.Add(A[max]);
                A.RemoveAt(max);

            }

        }

        public Medurez this[string ime]
        {
            get
            {
                List<Osoba> ubaci = new List<Osoba>();
                if (ime.Equals("*"))
                {
                    foreach (Osoba A in this.lista)
                        ubaci.Add(A);
                    return new Medurez(ubaci);
                }

                foreach (Osoba A in this.lista)
                    if (ime.Equals(A.ime))
                        ubaci.Add(A);
                return new Medurez(ubaci);
            }
        }

        public IEnumerator<Osoba> GetEnumerator()
        {

            foreach (Osoba o in this.lista)
                yield return o;

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    class Fejs : IEnumerable<Osoba>
    {

        public  List<Osoba> osobe;

        public Fejs()
        {
            osobe = new List<Osoba>();
        }

        public Fejs(Osoba osoba)
        {
            osobe = new List<Osoba>();
            osobe.Add(osoba);
        }

        public Fejs(string i, string p)
        {
            osobe = new List<Osoba>();
            osobe.Add(new Osoba(i, p));

        }

        public Fejs(Fejs fejs)
        {
            osobe = new List<Osoba>(fejs.osobe);
        }
        public void DodajOsobu(string i, string p)
        {
            osobe.Add(new Osoba(i, p));
        }

        public void DodajOsobu(Osoba osoba)
        {
            osobe.Add(osoba);
        }

        public void IzbaciOsobu(String Ime, String Prezime)
        {
            try
            {

                int nadjena_osoba = 0;
                foreach (Osoba osoba in osobe)
                {
                    if (osoba.ime == Ime && osoba.prezime == Prezime)
                    {
                        if (osoba.zero_friends == 1)
                            throw new System.Exception();
                        //svakome od prijatelja osobe x koju izbacujemo treba maknuti tog x iz popisa prijatelja
                        //i provjeriti je li tko ostao sada bez prijatelja
                        foreach (Osoba prijatelj in osoba.Prijatelji())
                        {
                            prijatelj.Prijatelji().Remove(osoba);
                            if (prijatelj.brojPrijatelja() == 0)
                                prijatelj.zero_friends = 1;
                        }
                        osoba.zero_friends = 1;
                        osobe.Remove(osoba);
                        nadjena_osoba = 1;
                        break;
                    }
                }
                if (nadjena_osoba == 0)
                {
                    Console.WriteLine("Iznimka - osobe nema medju korisnicima pa ju nije bilo moguce ni izbaciti.");
                    return;
                }
                provjeri();
            }
            catch
            {
                Console.WriteLine("Iznimka - osoba je vec izbacena pa ju nije bilo moguce izbaciti opet.");
            }

        }

        public void provjeri()
        {
            osobe.RemoveAll(bez_prijatelja);

        }

        private static bool bez_prijatelja(Osoba o)
        {
            if (o.zero_friends == 1)
            {
                Console.WriteLine("Izbacena je osoba " + o.ime + " " + o.prezime + " jer je ostala bez prijatelja.");
                return true;
            }
            return false;
        }
        public static Fejs operator +(Fejs fejs, Osoba osoba)
        {
            fejs.DodajOsobu(osoba);
            return fejs;
        }

        public static Fejs operator -(Fejs fejs, Osoba osoba)
        {
            foreach (Osoba o in fejs.osobe)
            {
                if (o.prijatelji.Contains(osoba))
                {
                    o.prijatelji.Remove(osoba);
                }
            }
            fejs.IzbaciOsobu(osoba.ime, osoba.prezime);
            return fejs;
        }

        public IEnumerator<Osoba> GetEnumerator()
        {

            foreach (Osoba o in this.osobe)
                yield return o;
        }

        public Medurezultat this[string ime]
        {
            get
            {

                List<Osoba> ubaci = new List<Osoba>();
                if (ime.Equals("*"))
                {
                    foreach (Osoba A in this.osobe)
                        ubaci.Add(A);
                    return new Medurezultat(ubaci);
                }
                foreach (Osoba A in this.osobe)
                    if (ime.Equals(A.prezime))
                        ubaci.Add(A);
                return new Medurezultat(ubaci);
            }
        }

        public void Sort(IComparer<Osoba> comparer)
        {
            Osoba.OsobaUsporedivac osobaUsporedivac = (Osoba.OsobaUsporedivac)comparer;

            if (osobaUsporedivac.tipUsporedbe.ToString().Equals("brojPrijatelja"))
            {
                osobe.Sort(comparer);
                return;
            }

            List<string> lista = new List<string>();
            foreach (Osoba o in osobe)
            {
                if (osobaUsporedivac.tipUsporedbe.ToString().Equals("prezime"))
                {
                    lista.Add(o.ime + " " + o.prezime);
                }
                else
                {
                    lista.Add(o.prezime + " " + o.ime);
                }
            }
            lista.Sort();
            List<Osoba> novifejs = new List<Osoba>();

            while (osobe.Count != 0)
            {

                for (int i = 0; i < osobe.Count; i++)
                {

                    if (osobaUsporedivac.tipUsporedbe.ToString().Equals("ime"))
                    {
                        if (lista[0].Equals(osobe[i].prezime + " " + osobe[i].ime))
                        {
                            novifejs.Add(osobe[i]);
                            lista.Remove(lista[0]);
                            osobe.Remove(osobe[i]);
                            break;
                        }
                    }
                    else
                    {
                        if (lista[0].Equals(osobe[i].ime + " " + osobe[i].prezime))
                        {
                            novifejs.Add(osobe[i]);
                            lista.Remove(lista[0]);
                            osobe.Remove(osobe[i]);
                            break;
                        }
                    }
                }

            }

            this.osobe = novifejs;

        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Fejs Facebook = new Fejs();

            Osoba A = new Osoba("Ante", "Kovačić");
            Osoba B = new Osoba("Ivan", "Mažuranić");
            Osoba C = new Osoba("Ivana", "Brlić- Mažuranić");
            Osoba D = new Osoba("Ida", "Marulic");
            Osoba E = new Osoba("Tin", "Ujevic");
            Osoba F = new Osoba("Ida", "Pavlic");
            Osoba G = new Osoba("Petar", "Pavlic");
            Osoba H = new Osoba("Marko", "Pavlic");

            // trenutno u programu imamo jednu instancu klase Fejs
            // i instance klase Osoba imena A-H

            // nadodajemo osobe u Facebook
            Facebook.DodajOsobu("Mirko", "Katic"); // ovdje smo nadodali i jednu novu osobu
            Facebook.DodajOsobu(A);
            Facebook += B;
            Facebook += C;
            Facebook += D;
            Facebook += E;
            Facebook += F;
            Facebook += G;
            Facebook += H;


            // Sprijateljiti ćemo neke od Osoba
            // A je prijatelj sa osobama B,C,D 
            A += B;
            A += C;
            A += D;

            // Osoba F je prijatelj sa A,C,E,B
            F += C;
            F += E;
            F += B;
            F += A;

            // prijatelji osobe F
            Console.WriteLine("Prijatelji osobe " + F.ime + " " + F.prezime);
            foreach (Osoba o in F.prijatelji)
                Console.WriteLine(o.ime + " " + o.prezime);
            Console.WriteLine("--------------------------------------------------------");
            // Osoba C je prijatelj sa A,F,G
            C += G;

            // Osoba D je prijatelj sa osobama A, F, i C
            // s time da je sada i F prijatelj sa D
            D += C;
            D += F;

            Console.WriteLine("Prijatelji osobe " + D.ime + " " + D.prezime);
            foreach (Osoba o in D.Prijatelji())
                Console.WriteLine(o.ime + " " + o.prezime);
            Console.WriteLine("--------------------------------------------------------");

            // ponovno prijatelji osobe B da vidimo da li radi
            Console.WriteLine("Prijatelji osobe " + F.ime + " " + F.prezime);
            foreach (Osoba o in F.Prijatelji())
                Console.WriteLine(o.ime + " " + o.prezime);
            Console.WriteLine("--------------------------------------------------------");

            // koristenje foreach na klasi Fejs + funkcija brojPrijatelja na klasi Osoba
            foreach (Osoba o in Facebook)
                Console.WriteLine("Osoba " + o.ime + " " + o.prezime + " je na Fejsu i ima " + o.brojPrijatelja() + " prijatelja");

            Console.WriteLine("--------------------------------------------------------");


            // koristenje foreach na Fejs uz indeks["prezime"]
            Console.WriteLine("koristenje foreach petlje na klasi Fejs[\"prezime\"] i funkcije Osoba.MeduPrijatelji");
            Console.WriteLine("Trebali bi biti sortirani po broju prijatelja ( pa broj prijatelja ide u zagradu ) ");

            foreach (Osoba o in Facebook["Marulic"])
            {
                foreach (Osoba p in Facebook["Pavlic"])
                {
                    HashSet<Osoba> skup = o.Meduprijatelji(p);
                    // trebali bi biti sortirani po broju prijatelja ( pa broj prijatelja ide u zagradu )
                    Console.WriteLine("Meduprijatelji osoba " + o.ToString() + " (" + o.brojPrijatelja() + ") i " + p.ToString() + "(" + p.brojPrijatelja() + ")");
                    foreach (Osoba r in skup)
                    {
                        Console.WriteLine(r.ToString());
                    }
                }
                Console.WriteLine("--------------------------------------------------------");

            }

            Console.WriteLine("Ispisi sve osobe sa imenom Ida koje su na Fejsu");
            foreach (Osoba o in Facebook["*"]["Ida"]) // ispisi sve ide sa Fejsa
                Console.WriteLine(o.ToString());
            Console.WriteLine("--------------------------------------------------------");


            Console.WriteLine("Svi prijatelji osobe " + A.ToString());
            foreach (Osoba o in A.Prijatelji())
                Console.WriteLine(o.ToString());
            Console.WriteLine("--------------------------------------------------------");

            Console.WriteLine("Sve osobe na facebooku: ");
            foreach (Osoba o in Facebook["*"])
            {
                Console.WriteLine(o.ToString() + " (" + o.brojPrijatelja() + ")");
            }
            Console.WriteLine("--------------------------------------------------------");

            Console.WriteLine("Osobu " + A.ToString() + " cemo maknuti sa Fejsa \npa cemo opet pogledati kako izgleda stanje prijateljstva na Fejsu");
            Facebook -= A;

            foreach (Osoba o in Facebook["*"])
            {
                Console.WriteLine(o.ToString() + " (" + o.brojPrijatelja() + ")");
            }

            Console.WriteLine("ohoho");

            Osoba.OsobaUsporedivac sorting = Osoba.DohvatiUsporedivac();

            sorting.tipUsporedbe = Osoba.OsobaUsporedivac.TipUsporedbe.brojPrijatelja;

            Facebook.Sort(sorting);

            Console.WriteLine("--------------------------------------------------------");

            Console.WriteLine("Sve osobe sa Fejsa sortirane po broju prijatelja");
            foreach (Osoba o in Facebook)
                Console.WriteLine(o.ToString() + " (" + o.brojPrijatelja() + ")");

            sorting.tipUsporedbe = Osoba.OsobaUsporedivac.TipUsporedbe.ime;

            Facebook.Sort(sorting);

            Console.WriteLine("--------------------------------------------------------");

            Console.WriteLine("Sve osobe sa Fejsa sortirane po prezimenima");
            foreach (Osoba o in Facebook)
                Console.WriteLine(o.ToString());

            sorting.tipUsporedbe = Osoba.OsobaUsporedivac.TipUsporedbe.prezime;

            Facebook.Sort(sorting);

            Console.WriteLine("--------------------------------------------------------");

            Console.WriteLine("Sve osobe sa Fejsa sortirane po imenima");
            foreach (Osoba o in Facebook)
                Console.WriteLine(o.ToString());
        }
    }
 }

