using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lector
{
    class Funciones
    {
        int counter = 1,
            bandera = 0,        
            numPaginas = 0,
            inicio = 0,
            final = 0,
            width = 0,
            height = 0;

        public int bandera_final = 0;
        int[] borrar;

        string line,
               sPatternW = "^.w0{width:",
               sPatternH = "^.h0{height:",
               tipo_libro = "",
               path = @"C:\Libros\",
               pathFinal = "",
               rutaEjercicios,
               puzzle,
               classDivPuzzle,
               classBtn,
               idBoton,
               idWords,
               classIco;

        Boolean re = false;       
        StreamWriter escri;                               

        Archivos objArchivos = new Archivos();

        public void agregarAssets(DirectoryInfo libro)
        {
            string carpeta = EliminarAssets(@"C:\Libros\", libro);
            //Directory.CreateDirectory(carpeta);

            // Agregar carpeta assets
            String rutaOrigen = @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\LIBRO_PRUEBA\Matematicas1231-K\assets\";
            DirectoryInfo assets = new DirectoryInfo(rutaOrigen);
            DirectoryInfo[] carpetas = assets.GetDirectories();
            foreach (DirectoryInfo asset in carpetas)
            {
                string fileName = "";
                foreach (var pagina in asset.GetFiles())
                {
                    fileName = pagina.Name;
                }
                string sourcePath = rutaOrigen + asset.Name;
                string targetPath = carpeta + Path.DirectorySeparatorChar + asset.Name;

                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.Directory.CreateDirectory(targetPath);
                System.IO.File.Copy(sourceFile, destFile, true);
                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);
                    foreach (string s in files)
                    {
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                    bandera_final++;
                }
                else
                {
                    Console.WriteLine("Error al copiar carpeta en libro " + libro);
                }
            }

            //Copiar el sopa de letras correspondiente
            System.IO.File.Copy(System.IO.Path.Combine(@"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\LIBRO_PRUEBA\Matematicas1231-K\Archivos\guias", libro.Name + "_SOPA" + ".js"),
                                System.IO.Path.Combine(path + libro.Name + @"\assets\modulos\", "sopas.js"), true);            
        }
        
        public void agregarIndex(DirectoryInfo libro)
        {
            // Agregar información nueva al INDEX del libro
            foreach (var fi in libro.GetFiles("*.html"))
            {
                pathFinal = path + libro.Name + @"\" + fi.Name;
                System.IO.StreamReader file = new System.IO.StreamReader(pathFinal, System.Text.Encoding.Default);
                escri = File.CreateText(@"C:\Libros\temp.html");

                //Inicializar variables
                counter = 1;
                line = "";
                bandera = 0;
                re = false;
                borrar = new int[100];
                Boolean bandera_tipo = false;
                width = 0;
                height = 0;
                numPaginas = 0;

                while (line != null)
                {
                    if (line == "<script>" || line == "</script>")
                    {
                        //Console.WriteLine("line: " + line + "   counter: " + counter);
                        borrar[bandera] = counter;
                        bandera++;
                    }
                    else if ((line == "<!-- inicio etiquetas -->" || line == "<!-- fin etiquetas -->") ||
                            (line == "<!-- inicio divs -->" || line == "<!-- fin divs -->") ||
                            (line == "<!-- inicio js -->" || line == "<!-- fin js -->"))
                    {
                        //Console.WriteLine("line: " + line + "   counter: " + counter);
                        borrar[bandera] = counter;
                        bandera++;
                        bandera_tipo = true;
                    }

                    // Obtener tamaños de la hoja
                    if ((System.Text.RegularExpressions.Regex.IsMatch(line, sPatternW)) && width == 0)
                    {
                        // Width
                        string[] w = line.Split(".");
                        w = w[1].Split(":");
                        width = Int32.Parse(w[1]);
                    }

                    if ((System.Text.RegularExpressions.Regex.IsMatch(line, sPatternH)) && height == 0)
                    {
                        // Height
                        string[] h = line.Split(".");
                        h = h[1].Split(":");
                        height = Int32.Parse(h[1]);
                    }

                    line = file.ReadLine();
                    counter++;
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                String index = objArchivos.archivos("index");
                String js = objArchivos.archivos("js");
                String divs = objArchivos.archivos("divs");
                String etiquetas = objArchivos.archivos("etiquetas");
                String style = objArchivos.style();

                line = File.ReadLines(pathFinal).Skip(18).Take(1).First();
                for (int x = 1; x < counter - 1; x++)
                {
                    line = File.ReadLines(pathFinal).Skip(x - 1).Take(1).First();
                    if (!bandera_tipo)
                    {
                        if (!(x >= borrar[0] - 1 && x <= borrar[5]))
                        {
                            line = File.ReadLines(pathFinal).Skip(x - 1).Take(1).First();
                            escri.WriteLine(line);
                        }
                    }
                    else
                    {
                        if (!(x >= borrar[2] - 1 && x <= borrar[7] - 1) && !(x >= borrar[0] - 1 && x <= borrar[1] - 1) &&
                        !(x >= borrar[8] - 1 && x <= borrar[9] - 1) && !(x >= borrar[10] - 1 && x <= borrar[11] - 1))
                        {
                            line = File.ReadLines(pathFinal).Skip(x - 1).Take(1).First();
                            escri.WriteLine(line);
                        }
                    }

                    // Agregar     
                    if (line == "</style>")
                    {
                        if (re != true)
                        {
                            escri.WriteLine(index);
                            re = true;
                        }
                    }

                    if (line == @"<meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1""/>")
                    {
                        // Vertical
                        if (width == 306 && height == 396)
                            tipo_libro = "vertical";
                        else
                            tipo_libro = "horizontal";

                        escri.WriteLine(etiquetas);
                    }

                    if (line == @"<div id=""page-container"">")
                    {
                        escri.WriteLine(divs);
                    }

                    if (line == "<title></title>" && !bandera_tipo)
                    {
                        escri.WriteLine(style);
                    }

                    if (line == "</body>")
                    {
                        escri.WriteLine(js);
                    }
                }

                file.Close();
                escri.Close();
                File.Delete(pathFinal);
                File.Move(@"C:\Libros\temp.html", pathFinal);
            }
        }

        public void obtenerListaEjercicios(DirectoryInfo libro, string bandera_tipoLibro)
        {
            // Obtener lista de ejercicios            
            if (bandera_tipoLibro.Equals("guia")) rutaEjercicios = @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\LIBRO_PRUEBA\Matematicas1231-K\Archivos\guias\" + libro.Name + ".txt";
            else if (bandera_tipoLibro.Equals("preescolar")) rutaEjercicios = @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\LIBRO_PRUEBA\Matematicas1231-K\Archivos\preescolar\preescolar.txt";
            else rutaEjercicios = @"C:\xampp\htdocs\movil.sevalladolid.mx\mlsev\LIBRO_PRUEBA\Matematicas1231-K\Archivos\primaria\" + libro.Name + ".txt";

            System.IO.StreamReader file2 = new System.IO.StreamReader(rutaEjercicios, System.Text.Encoding.Default);
            String line2 = "";
            int counter2 = 0;
            string[] paginasInicio, paginasFinal;

            while (line2 != null)
            {
                if (counter2 == 1)
                {
                    // Paginas inicio
                    paginasInicio = line2.Split(",");
                    paginasInicio = paginasInicio[0].Split(":");
                    inicio = Int32.Parse(paginasInicio[1]);
                    // Paginas final
                    paginasFinal = line2.Split(",");
                    paginasFinal = paginasFinal[1].Split(":");
                    final = Int32.Parse(paginasFinal[1]);
                }

                // Obtener el numero de hojas sin canvas en libros de PREESCOLAR
                if (bandera_tipoLibro.Equals("preescolar") && counter2 == 3)
                {
                    Console.WriteLine("ES LIBRO DE PREESCOLAR Y ESTA BUSCANDO LAS HOJAS SIN CANVAS Y LOS AGREGARA!");
                    // No canvas
                    List<String> listaNoCanvas = new List<String>();
                    string[] paginasNoCanvas = line2.Split(",");
                    for (int i = 0; i < paginasNoCanvas.Length; i++)
                    {
                        listaNoCanvas.Add(paginasNoCanvas[i]);
                    }
                    int numFinal = numPaginas - inicio;
                    for (int m = 1; m <= final; m++)
                    {
                        if (final != 1) if (m != 1) numFinal = numFinal - 1;
                        listaNoCanvas.Add(Convert.ToString(numFinal));
                    }

                    // Agregar canvas a cada página a libros de PREESCOLAR
                    foreach (var fi in libro.GetFiles("*.baz"))
                    {
                        Boolean banderaCanvas = false;
                        String namePage = "page" + libro;
                        String ejemplo = fi.Name.Substring(namePage.Length);
                        String[] id = ejemplo.Split(".");
                        int idhoja = Int32.Parse(id[0]);
                        int numInicio = idhoja - inicio;

                        foreach (string pagina in listaNoCanvas)
                        {
                            if (pagina == Convert.ToString(numInicio))
                            {
                                banderaCanvas = true;
                            }
                        }

                        if (!banderaCanvas && idhoja > inicio)
                        {
                            String ruta = path + libro.Name + @"\" + fi.Name;
                            String readText = File.ReadAllText(ruta);
                            String encontrar = objArchivos.canvas();
                            String canvas = @"<canvas id=""canvas" + idhoja + @""" class=""canvas"" width=""" + width + @""" height=""" + height + @"""></canvas>";
                            int cc = readText.IndexOf(@"<canvas id=""canvas" + idhoja);
                            if (cc > 0)
                            {
                                // Existe canvas
                                String canvasactual = readText.Substring(cc);
                                readText = readText.Replace(canvasactual, canvas + encontrar);
                            }
                            else
                            {
                                readText = readText.Replace(encontrar, canvas + encontrar);
                            }
                            escri = File.CreateText(ruta);
                            escri.WriteLine(readText);
                            escri.Close();
                        }
                    }
                }

                line2 = file2.ReadLine();
                counter2++;
            }
        }

        public void agregarEjercicios(DirectoryInfo libro, string bandera_tipoLibro, Boolean exists_file)
        {
            // Agregar ejercicios por hoja en libros de PRIMARIA, SECUNDARIA y BACHILLERATO                
            if (!bandera_tipoLibro.Equals("preescolar"))
            {
                Console.WriteLine("ES LIBRO DE PRIMARIA, SECUNDARIA O BACHILLERATO!");
                foreach (var fi in libro.GetFiles("*.baz"))
                {
                    String namePage = "";
                    String ejemplo = "";
                    String[] id;

                    if (!bandera_tipoLibro.Equals("guia"))
                    {
                        namePage = "page" + libro;
                        ejemplo = fi.Name.Substring(namePage.Length);
                        id = ejemplo.Split(".");
                    }
                    else
                    {
                        namePage = "page";
                        ejemplo = fi.Name.Substring(namePage.Length);
                        id = ejemplo.Split("Guia");
                    }

                    int idhoja = Int32.Parse(id[0]);
                    int numInicio = idhoja - inicio;

                    System.IO.StreamReader file3 = new System.IO.StreamReader(rutaEjercicios, System.Text.Encoding.Default);
                    string line3 = "";
                    int counter3 = 0;
                    string[] ejer;
                    string placeholder = "Escribe tu respuesta aquí";

                    while (line3 != null)
                    {
                        // Recorrer hoja por hoja
                        string[] actual = line3.Split(":");
                        // Comparar si la pagina tiene ejercicios
                        if (actual[0].Equals("p" + numInicio))
                        {
                            int numEjercicio = 1;
                            int top = 20, left = 50;
                            string wid = "", hei = "";
                            string html = "<!-- inicio ejercicios -->";
                            ejer = actual[1].Split(",");
                            // Obtener ejercicios
                            for (int x = 0; x < ejer.Length; x++)
                            {
                                // Identificar número y tipo de ejercicio
                                Match match = Regex.Match(ejer[x], @"[a-z]");                                
                                // Validar si esta correcta la nomenclatura
                                if (match.Success)
                                {
                                    try
                                    {
                                        // Cantidad
                                        //Console.WriteLine(line3);
                                        int cantidad = Convert.ToInt32(ejer[x].Substring(0, match.Index));
                                        // Ejercicio
                                        string ejercicio = ejer[x].Substring(match.Index, ejer[x].Length - match.Index);
                                        // Recorrer la cantidad
                                        for (int m = 1; m <= cantidad; m++)
                                        {
                                            // Comparar ejercicio
                                            string nombreEjercicio = "";
                                            html += "\n\r\t\t";
                                            if (ejercicio.Equals("txt") || ejercicio.Equals("crucigrama"))
                                            {
                                                // TXT o CRUCIGRAMA
                                                nombreEjercicio = "p" + idhoja + "_" + ejercicio + numEjercicio;
                                                string clase = "respuesta";
                                                if (ejercicio.Equals("crucigrama"))
                                                {
                                                    wid = "10px"; hei = "10px";
                                                    clase += " " + ejercicio + " valn";
                                                }
                                                else wid = "70px"; hei = "8px";
                                                html += @"<input type=""text"" id =""" + nombreEjercicio + @""" class=""" + clase + @""" placeholder =""" + placeholder + @"""";
                                                if (ejercicio.Equals("crucigrama")) html += @" maxlength=""1""";
                                                html += ">";

                                            }
                                            else if (ejercicio.Equals("txtarea"))
                                            {
                                                // TXTAREA
                                                nombreEjercicio = "p" + idhoja + "_txtarea" + numEjercicio;
                                                html += @"<textarea id =""" + nombreEjercicio + @""" class=""respuesta"" placeholder =""" + placeholder + @"""></textarea>";
                                                wid = "70px"; hei = "8px";
                                            }
                                            else if (ejercicio.Equals("canvas"))
                                            {
                                                // CANVAS
                                                nombreEjercicio = "canvas" + idhoja;
                                                html += @"<canvas id =""" + nombreEjercicio + @""" class=""canvas"" width=""" + width + @""" height=""" + height + @"""></canvas>";
                                            }
                                            else if (System.Text.RegularExpressions.Regex.IsMatch(ejercicio, "relacionar"))
                                            {
                                                // RELACIONAR
                                                string[] relacionar = ejercicio.Split("#");
                                                nombreEjercicio = "p" + idhoja + "_relacionar" + numEjercicio;
                                                string nombreEjercicioRel = "";
                                                html += @"<div id=""" + nombreEjercicio + @""" name=""relacionar"">";
                                                int num = 0, leftR = 0, topR = 50;
                                                for (int l = 1; l <= Convert.ToInt32(relacionar[1]); l++)
                                                {
                                                    string valor = "";
                                                    if ((l % 2) == 0)
                                                    {
                                                        valor = "d";
                                                        leftR = 185;
                                                    }
                                                    else
                                                    {
                                                        valor = "i";
                                                        num++;
                                                        topR += 50;
                                                        leftR = 50;
                                                    }
                                                    nombreEjercicioRel = "p" + idhoja + "_rel_" + valor + "_" + num;
                                                    html += "\n\r\t\t\t\t" + @"<div id =""" + nombreEjercicioRel + @""" class=""relacionar""></div>";
                                                    if (exists_file != true)
                                                    {
                                                        guardarCSS(path, libro, nombreEjercicioRel, ejercicio, leftR, topR, "50px", "50px");
                                                    }
                                                }
                                                html += "\n\r\t\t" + @"</div>";
                                            }
                                            else if (System.Text.RegularExpressions.Regex.IsMatch(ejercicio, "encerrar") || ejercicio.Equals("subrayar"))
                                            {
                                                // ENCERRAR o SUBRAYAR
                                                nombreEjercicio = "p" + idhoja + "_" + ejercicio + "_" + numEjercicio;
                                                html += @"<div id =""" + nombreEjercicio + @""" class=""" + ejercicio + @"""></div>";
                                                wid = "10px"; hei = "10px";
                                            }
                                            else if (ejercicio.Equals("palomita") || ejercicio.Equals("tachita"))
                                            {
                                                // PALOMITA o TACHITA
                                                nombreEjercicio = "p" + idhoja + "_" + ejercicio + "_" + numEjercicio;
                                                html += @"<div id =""" + nombreEjercicio + @""" class=""" + ejercicio + @"""></div>";
                                                wid = "10px"; hei = "10px";
                                            }
                                            else if (System.Text.RegularExpressions.Regex.IsMatch(ejercicio, "sopa"))
                                            {
                                                // SOPA DE LETRAS
                                                nombreEjercicio = "p" + idhoja + "_sopa";
                                                puzzle = "puzzle" + "_" + idhoja;
                                                classDivPuzzle = puzzle + "_div";
                                                idBoton = "p" + idhoja + "_boton";
                                                idWords = "words_" + idhoja;
                                                html += @"<div id="""+puzzle+@"""><div id="""+nombreEjercicio+@""" class="""+puzzle+@" " +classDivPuzzle+@"""></div></div>";
                                                html += "\n\r\t\t";
                                                html += @"<div id="""+ idBoton + @""" onclick=""showPuzzle()"">Jugar</div>";
                                                html += "\n\r\t\t";
                                                html += @"<div id=""" + idWords + @"""></div>";                                                
                                            }
                                            else if (ejercicio.Equals("foto"))
                                            {
                                                // FOTOGRAFÍA
                                                nombreEjercicio = "p" + idhoja + "_" + "img" + numEjercicio + "_div";
                                                var foto = "imagen_" + idhoja;
                                                var idFoto = "p" + idhoja + "_img" + numEjercicio;
                                                html += @"<div id=""" + nombreEjercicio + @""" class=""imagen"" onclick=""Click(this.id)""> " + "\n\r\t\t" + @"  <i class=""fas fa-plus""></i>" + "\n\r\t\t" + "</div>" + "\n\r\t\t" + @" <input style=""display:none;"" type=""file"" id=""" + idFoto + @""" accept=""image/*"" onchange=""mostrar(this.id)""/>  ";
                                                wid = "40px"; hei = "70px";
                                            }
                                            else if (ejercicio.Equals("fotoC"))
                                            {
                                                // FOTOGRAFÍA CON CANVAS
                                                nombreEjercicio = "p" + idhoja + "_" + "img" + numEjercicio + "_div";
                                                var nombreFotoBoton = "p" + idhoja + "_" + "img" + numEjercicio + "_btn";
                                                var foto = "imagen_" + idhoja;
                                                var idFoto = "p" + idhoja + "_img" + numEjercicio;
                                                classBtn = "btn_" + idhoja + "_" + numEjercicio;
                                                classIco = "ico_" + idhoja + "_" + numEjercicio;
                                                html += @"<div id=""" + nombreEjercicio + @""" class=""imagen""> " + "</div>" + "\n\r\t\t" + @"<button type=""button"" id=""" + nombreFotoBoton + @""" onclick=""Click(this.id)"" class=""" + classBtn + @" botonImagen""> " + "\n\r\t\t" + @" <i class=""fas fa-plus " + classIco + @""" ></i> " + "\n\r\t\t" + "</button>" + "\n\r\t\t" + @"<input style=""display:none;"" type=""file"" id=""" + idFoto + @""" accept=""image/*"" onchange=""mostrar(this.id)""/>  ";
                                                wid = "40px"; hei = "70px";
                                            }
                                            else if (System.Text.RegularExpressions.Regex.IsMatch(ejercicio, "multiple"))
                                            {
                                                // OPCIÓN MÚLTIPLE
                                                string[] multiple = ejercicio.Split("#");
                                                for (int i = 1; i <= Convert.ToInt32(multiple[1]); i++)
                                                {
                                                    nombreEjercicio = "p" + idhoja + "_pregunta" + numEjercicio + "_opcion" + i;
                                                    string data = "p" + idhoja + "_pregunta" + numEjercicio;
                                                    html += @"<div id=""" + nombreEjercicio + @""" data=""" + data + @""" class=""" + multiple[0] + @"""></div>";
                                                    if (multiple[0].Equals("multipleS")) wid = "275px";
                                                    else wid = "20px";
                                                    hei = "20px";
                                                    if (exists_file != true)
                                                    {
                                                        guardarCSS(path, libro, nombreEjercicio, ejercicio, left, top, wid, hei);
                                                    }
                                                }
                                            }

                                            // Crear estilo del ejercicio
                                            if (!System.Text.RegularExpressions.Regex.IsMatch(ejercicio, "relacionar")
                                                && !ejercicio.Equals("canvas")
                                                && exists_file != true
                                                && !System.Text.RegularExpressions.Regex.IsMatch(ejercicio, "multiple"))
                                            {
                                                guardarCSS(path, libro, nombreEjercicio, ejercicio, left, top, wid, hei);
                                            }

                                            numEjercicio++;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("incorrectas: " + line3);
                                        bandera_final = 0;
                                    }                                    
                                }else
                                {
                                    Console.WriteLine("incorrectas: " + line3);
                                    bandera_final = 0;
                                }
                            }
                            html += "\n\r" + "<!-- fin ejercicios -->" + "\n\r";
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            // Guardar ejercicios en la hoja
                            String ruta = path + libro.Name + @"\" + fi.Name;
                            String readText = File.ReadAllText(ruta);
                            String encontrar = objArchivos.canvas();
                            int cc = readText.IndexOf("<!-- inicio ejercicios -->");
                            if (cc > 0)
                            {
                                // Hay ejercicios
                                String ejerciciosactuales = readText.Substring(cc);
                                readText = readText.Replace(ejerciciosactuales, html + encontrar);
                            }
                            else
                            {
                                // No hay ejercicios
                                readText = readText.Replace(encontrar, "\n\r" + html + encontrar);
                            }
                            escri = File.CreateText(ruta);
                            escri.WriteLine(readText);
                            escri.Close();
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        }

                        line3 = file3.ReadLine();
                        counter3++;
                    }
                }
            }
        }

        public void numeroDePaginas(DirectoryInfo libro)
        {
            // Asignar número de páginas, páginas de inicio y tipo de libro (vertical - horizontal)
            string[] filePaths = Directory.GetFiles(@"C:\Libros" + Path.DirectorySeparatorChar + libro.Name, "*.baz");
            numPaginas = filePaths.Length;
            foreach (var fi in libro.GetFiles("*.html"))
            {
                String ruta = path + libro.Name + @"\" + fi.Name;
                String readText = File.ReadAllText(ruta);
                // Diferencia páginas inicio
                String encontrar = "var diferencia_pagina_inicio = 2";
                readText = readText.Replace(encontrar, "var diferencia_pagina_inicio = " + inicio);
                // Diferencia páginas final
                String encontrar2 = "var diferencia_pagina_final = 1";
                readText = readText.Replace(encontrar2, "var diferencia_pagina_final = " + final);
                // Tipo de libro
                String encontrar3 = "let tipo_libro = '';";
                readText = readText.Replace(encontrar3, "let tipo_libro = '" + tipo_libro + "';");
                escri = File.CreateText(ruta);
                escri.WriteLine(readText);
                escri.Close();
            }
        }

        public void guardarCSS(string path, DirectoryInfo libro, string nombreEjercicio, string ejercicio, int left, int top, string width, string height)
        {
            // Se agrega el ejercicio al CSS
            string estilo = path + libro.Name + @"\" + "assets" + @"\" + "css" + @"\" + "ejercicios.css";            
            if (nombreEjercicio.Length != 0)
            {
                StreamWriter escri2 = File.AppendText(estilo);
                if (System.Text.RegularExpressions.Regex.IsMatch(ejercicio, "sopa"))
                {                    
                    escri2.WriteLine("." + puzzle + " .puzzleSquare" + " {");
                    escri2.WriteLine("  height: 16px;");
                    escri2.WriteLine("  width: 21px;");
                    escri2.WriteLine("  font-size: 12px;");
                    escri2.WriteLine("}");
                    escri2.WriteLine(" ");

                    escri2.WriteLine("." + puzzle + " .selected" + "," + " ." + puzzle + " .found" + "," + " ." + puzzle + " .solved" + " {");
                    escri2.WriteLine("  height: 18px;");
                    escri2.WriteLine("}");
                    escri2.WriteLine(" ");

                    escri2.WriteLine("#" + idBoton + " {");
                    escri2.WriteLine("  position: absolute;");
                    escri2.WriteLine("  left: 236px;");
                    escri2.WriteLine("  top: 56px;");
                    escri2.WriteLine("  font-size: 8px;");
                    escri2.WriteLine("  cursor: pointer;");
                    escri2.WriteLine("}");
                    escri2.WriteLine(" ");

                    escri2.Close();
                }
                else if (ejercicio == "fotoC")
                {
                    escri2.WriteLine("#" + nombreEjercicio + " {");
                    escri2.WriteLine("  left: " + left + "px;");
                    escri2.WriteLine("  top: " + top + "px;");
                    escri2.WriteLine("  height: " + height + ";");
                    escri2.WriteLine("  width: " + width + ";");
                    escri2.WriteLine("}");
                    escri2.WriteLine(" ");

                    escri2.WriteLine("." + classBtn + " {");
                    escri2.WriteLine("  left: 164px;");
                    escri2.WriteLine("  top: 49px;");
                    escri2.WriteLine("  height: 19px;");
                    escri2.WriteLine("  width: 55px;");
                    escri2.WriteLine("}");
                    escri2.WriteLine(" ");

                    escri2.Close();

                }
                else
                {
                    escri2.WriteLine("#" + nombreEjercicio + " {");
                    escri2.WriteLine("  left: " + left + "px;");
                    escri2.WriteLine("  top: " + top + "px;");
                    if(!ejercicio.Equals("sOpcion"))
                    {
                        escri2.WriteLine("  height: " + height + ";");
                        escri2.WriteLine("  width: " + width + ";");
                    }
                    escri2.WriteLine("}");
                    escri2.WriteLine(" ");
                    escri2.Close();
                }
            }            
        }

        public string EliminarAssets(string ruta, DirectoryInfo libro)
        {
            // Eliminar carpeta assets
            Boolean ba = false;
            string carpeta = ruta + Path.DirectorySeparatorChar + libro.Name + Path.DirectorySeparatorChar + "assets";
            if (System.IO.Directory.Exists(carpeta))
            {
                DirectoryInfo a = new DirectoryInfo(carpeta);
                DirectoryInfo[] b = a.GetDirectories();
                foreach (DirectoryInfo asset in b)
                {
                    foreach (var pagina in asset.GetFiles())
                    {
                        string exclude = pagina.ToString();
                        if (exclude != ruta + libro.Name + @"\" + "assets" + @"\" + "css" + @"\" + "ejercicios.css")
                        {                            
                            pagina.Delete();
                            ba = true;
                        }
                    }
                    if (ba == false) asset.Delete();
                }
                if (ba == false) System.IO.Directory.Delete(carpeta);
            }
            return carpeta;
        }

        public void Copiar(String Direccion, String libro)
        {
            Eliminar(Direccion, libro);
            DirectoryInfo di_origen = new DirectoryInfo(@"C:\Libros\" + libro);
            DirectoryInfo di_destino = new DirectoryInfo(Direccion + libro);
            CopyFilesRecursively(di_origen, di_destino);
            Console.WriteLine("LIBRO " + libro);
            Console.WriteLine("Direccion " + Direccion);
        }

        public void Eliminar(string ruta, string libro)
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
    }
}
