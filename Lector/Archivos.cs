using System;
using System.IO;

namespace Lector
{
    class Archivos
    {
        String cadena;
        public string archivos(string tipo)
        {
            cadena = File.ReadAllText(@"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\LIBRO_PRUEBA\Matematicas1231-K\Archivos\" + tipo + ".html");

            return cadena;
        }

        public string style()
        {
            cadena = @"<link rel=""stylesheet"" type=""text/css"" href=""assets/css/style.css"">";
            cadena += "\n\r" + @"<link rel=""stylesheet"" type=""text/css"" href=""assets/css/ejercicios.css"">";

            return cadena;
        }

        public string canvas()
        {
            cadena = @"</div><div class=""pi"" data-data='{""ctm"":[0.500000,0.000000,0.000000,0.500000,0.000000,0.000000]}'></div></div>";

            return cadena;
        }

        public void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            }
            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }
        }

        public void EliminarAssets(string ruta, string libro)
        {
            // Eliminar carpeta assets
            string carpeta = ruta + Path.DirectorySeparatorChar + libro + Path.DirectorySeparatorChar + "assets";
            if (System.IO.Directory.Exists(carpeta))
            {
                DirectoryInfo a = new DirectoryInfo(carpeta);
                DirectoryInfo[] b = a.GetDirectories();
                foreach (DirectoryInfo asset in b)
                {
                    foreach (var pagina in asset.GetFiles())
                    {
                        pagina.Delete();
                    }
                    asset.Delete();
                }
                System.IO.Directory.Delete(carpeta);
            }
        }

        public void Copiar(String Direccion, String libro)
        {
            EliminarAssets(Direccion, libro);
            DirectoryInfo di_origen = new DirectoryInfo(@"C:\Libros\" + libro);
            DirectoryInfo di_destino = new DirectoryInfo(Direccion + libro);
            CopyFilesRecursively(di_origen, di_destino);
            Console.WriteLine("LIBRO " + libro);
            Console.WriteLine("Direccion " + Direccion);
        }

        //Ruta para libros de Kinder
        //public String[] kinder = new string[] { @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PREESCOLAR\LIBRO\PRIMERO\",
        //                                  @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PREESCOLAR\LIBRO\SEGUNDO\",
        //                                  @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PREESCOLAR\LIBRO\TERCERO\"};

        //Ruta para libros de Primaria
        //public String[] Primaria = new string[] { @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PRIMARIA\LIBRO\PRIMERO\",
        //                                   @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PRIMARIA\LIBRO\SEGUNDO\",
        //                                   @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PRIMARIA\LIBRO\TERCERO\",
        //                                   @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PRIMARIA\LIBRO\CUARTO\",
        //                                   @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PRIMARIA\LIBRO\QUINTO\",
        //                                   @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\PRIMARIA\LIBRO\SEXTO\"};

        //Ruta para libros de Secundaria
        //public String[] Secundaria = new string[] { @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\SECUNDARIA\LIBRO\PRIMERO\",
        //                                     @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\SECUNDARIA\LIBRO\SEGUNDO\",
        //                                     @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\SECUNDARIA\LIBRO\TERCERO\"};

        //Ruta para libros de Bachillerato
        //public String[] Bachillerato = new string[] { @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\BACHILLERATO\LIBRO\PRIMERO\",
        //                                       @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\BACHILLERATO\LIBRO\SEGUNDO\",
        //                                       @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\BACHILLERATO\LIBRO\TERCERO\"};

        //Ruta para libros de Kinder
        public String[] kinder = new string[] { @"C:\xampp\htdocs\PRUEBAS\PREESCOLAR\LIBRO\PRIMERO\",
                                             @"C:\xampp\htdocs\PRUEBAS\PREESCOLAR\LIBRO\SEGUNDO\",
                                             @"C:\xampp\htdocs\PRUEBAS\PREESCOLAR\LIBRO\TERCERO\"};

        //Ruta para libros de Primaria
        public String[] Primaria = new string[] { @"C:\xampp\htdocs\PRUEBAS\PRIMARIA\LIBRO\PRIMERO\",
                                               @"C:\xampp\htdocs\PRUEBAS\PRIMARIA\LIBRO\SEGUNDO\",
                                               @"C:\xampp\htdocs\PRUEBAS\PRIMARIA\LIBRO\TERCERO\",
                                               @"C:\xampp\htdocs\PRUEBAS\PRIMARIA\LIBRO\CUARTO\",
                                               @"C:\xampp\htdocs\PRUEBAS\PRIMARIA\LIBRO\QUINTO\",
                                               @"C:\xampp\htdocs\PRUEBAS\PRIMARIA\LIBRO\SEXTO\"};

        //Ruta para libros de Secundaria
        public String[] Secundaria = new string[] { @"C:\xampp\htdocs\PRUEBAS\SECUNDARIA\LIBRO\PRIMERO\",
                                                 @"C:\xampp\htdocs\PRUEBAS\SECUNDARIA\LIBRO\SEGUNDO\",
                                                 @"C:\xampp\htdocs\PRUEBAS\SECUNDARIA\LIBRO\TERCERO\"};

        //Ruta para libros de Bachillerato
        public String[] Bachillerato = new string[] { @"C:\xampp\htdocs\PRUEBAS\BACHILLERATO\LIBRO\PRIMERO\",
                                                   @"C:\xampp\htdocs\PRUEBAS\BACHILLERATO\LIBRO\SEGUNDO\",
                                                   @"C:\xampp\htdocs\PRUEBAS\BACHILLERATO\LIBRO\TERCERO\"};
    }
}