using System;
using System.IO;

namespace Lector
{
    class Program
    {
        public static void Main(string[] args)
        {

            // Founders: Jesus & Adriana
            // Omega Project

            String path = @"C:\Libros\",
                   libro = "",
                   bandera_tipoLibro = "",
                   bandera_gradoLibro = "";

            Boolean exists_file = false;

            string[] guias = new string[6] { "PRIMERO", "SEGUNDO", "TERCERO", "CUARTO", "QUINTO", "SEXTO" };
            string[] libros = new string[100];

            int variable_array = 0;

            Archivos objArchivos = new Archivos();
            Libro objLibro = new Libro();
            Funciones objFunciones = new Funciones();

            DirectoryInfo dirPrincipal = new DirectoryInfo(@"C:\Libros");
            DirectoryInfo[] dirLibros = dirPrincipal.GetDirectories();


            // Recorrer los libros
            foreach (DirectoryInfo book in dirLibros)
            {
                libro = book.Name;
                Console.WriteLine("LIBRO: " + libro);
                objLibro.Bandera_final(0);

                //******************************************************************

                //Comprobar si existe EstiloLibro
                string exists = path + book.Name + @"\" + "assets" + @"\" + "css" + @"\" + "ejercicios.css";
                if (File.Exists(exists)) exists_file = true;
                                    
                // Comparar si es una GUIA
                foreach (string guia in guias) if (libro.Equals(guia)) bandera_tipoLibro = "guia";

                // Obtener el nivel del libro
                if (System.Text.RegularExpressions.Regex.IsMatch(libro, "-K")) bandera_tipoLibro = "preescolar";
                else if (System.Text.RegularExpressions.Regex.IsMatch(libro, "-P")) bandera_tipoLibro = "primaria";
                else if (System.Text.RegularExpressions.Regex.IsMatch(libro, "-S")) bandera_tipoLibro = "secundaria";
                else if (System.Text.RegularExpressions.Regex.IsMatch(libro, "-B")) bandera_tipoLibro = "bachillerato";

                // Obtener grado del libro
                if (System.Text.RegularExpressions.Regex.IsMatch(libro, "1-")) bandera_gradoLibro = "primero";
                else if (System.Text.RegularExpressions.Regex.IsMatch(libro, "2-")) bandera_gradoLibro = "segundo";
                else if (System.Text.RegularExpressions.Regex.IsMatch(libro, "3-")) bandera_gradoLibro = "tercero";
                else if (System.Text.RegularExpressions.Regex.IsMatch(libro, "4-")) bandera_gradoLibro = "cuarto";
                else if (System.Text.RegularExpressions.Regex.IsMatch(libro, "5-")) bandera_gradoLibro = "quinto";
                else if (System.Text.RegularExpressions.Regex.IsMatch(libro, "6-")) bandera_gradoLibro = "sexto";

                //******************************************************************

                //Agregar assets
                objLibro.agregarAssets(book, bandera_tipoLibro, bandera_gradoLibro);                                                                                           

                //Agregar el index
                objLibro.agregarIndex(book);

                //Obtener la lista de los ejercicios
                objLibro.obtenerListaEjercicio(book, bandera_tipoLibro, bandera_gradoLibro);

                //Agregar los ejercicios
                objLibro.agregarEjercicios(book, bandera_tipoLibro, bandera_gradoLibro, exists_file);                

                //Agregar el numero de hojas que tiene el libro
                objLibro.numeroDePaginas(book);

                //******************************************************************

                //Almacenar los libros que se convirtieron
                libros[variable_array] = libro;
                variable_array = variable_array + 1;
            }

            //// Copiar libros al servidor local
            //for (int x = 0; x < libros.Length; x++)
            //{
            //    if (libros[x] != null)
            //    {
            //        if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "1-K")) objFunciones.Copiar(objArchivos.kinder[0], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "2-K")) objFunciones.Copiar(objArchivos.kinder[1], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "3-K")) objFunciones.Copiar(objArchivos.kinder[2], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "1-P")) objFunciones.Copiar(objArchivos.Primaria[0], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "2-P")) objFunciones.Copiar(objArchivos.Primaria[1], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "3-P")) objFunciones.Copiar(objArchivos.Primaria[2], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "4-P")) objFunciones.Copiar(objArchivos.Primaria[3], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "5-P")) objFunciones.Copiar(objArchivos.Primaria[4], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "6-P")) objFunciones.Copiar(objArchivos.Primaria[5], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "1-S")) objFunciones.Copiar(objArchivos.Secundaria[0], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "2-S")) objFunciones.Copiar(objArchivos.Secundaria[1], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "3-S")) objFunciones.Copiar(objArchivos.Secundaria[2], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "1-B")) objFunciones.Copiar(objArchivos.Bachillerato[0], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "2-B")) objFunciones.Copiar(objArchivos.Bachillerato[1], libros[x]);
            //        else if (System.Text.RegularExpressions.Regex.IsMatch(libros[x], "3-B")) objFunciones.Copiar(objArchivos.Bachillerato[2], libros[x]);
            //    }
            //}

            if (objLibro.Bandera_final(1) >= 2) Console.WriteLine("Libro: " + libro + " finalizado con exito.");
            else Console.WriteLine("No se completo libro: " + libro + " si es la primera vez que se convierte, borrar el archivo ejercicio.css antes de volver a convertirlo");
            if (libro.Equals("")) Console.WriteLine("No se encontro ningun libro.");            

            System.Console.ReadLine();
        }
    }
}