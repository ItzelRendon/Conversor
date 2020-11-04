using System;
using System.IO;

namespace Lector
{
    class Libro
    {
        Funciones objFunciones = new Funciones();

        public void agregarAssets(DirectoryInfo libro)
        {
            objFunciones.agregarAssets(libro);
        }

        public void agregarIndex(DirectoryInfo libro)
        {
            objFunciones.agregarIndex(libro);
        }

        public void obtenerListaEjercicio(DirectoryInfo libro, string bandera_tipoLibro)
        {
            objFunciones.obtenerListaEjercicios(libro, bandera_tipoLibro);
        }

        public void agregarEjercicios(DirectoryInfo libro, string bandera_tipoLibro, Boolean exists_file)
        {
            objFunciones.agregarEjercicios(libro, bandera_tipoLibro, exists_file);
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
