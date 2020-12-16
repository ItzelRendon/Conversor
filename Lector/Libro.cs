using System;
using System.IO;

namespace Lector
{
    class Libro
    {
        Funciones objFunciones = new Funciones();

        public void agregarAssets(DirectoryInfo libro, string bandera_tipoLibro, string bandera_gradoLibro)
        {
            objFunciones.agregarAssets(libro, bandera_tipoLibro, bandera_gradoLibro);
        }

        public void agregarIndex(DirectoryInfo libro)
        {
            objFunciones.agregarIndex(libro);
        }

        public void obtenerListaEjercicio(DirectoryInfo libro, string bandera_tipoLibro, string bandera_gradoLibro)
        {
            objFunciones.obtenerListaEjercicios(libro, bandera_tipoLibro, bandera_gradoLibro);
        }

        public void agregarEjercicios(DirectoryInfo libro, string bandera_tipoLibro, string bandera_gradoLibro, Boolean exists_file)
        {
            objFunciones.agregarEjercicios(libro, bandera_tipoLibro, bandera_gradoLibro, exists_file);
        }

        public void numeroDePaginas(DirectoryInfo libro)
        {
            objFunciones.numeroDePaginas(libro);
        }

        public int Bandera_final(int valor)
        {
            if(valor!= 0)
            {
                return objFunciones.bandera_final;
            }
            else
            {
                return 0;
            }            
        }
    }
}
