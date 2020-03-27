using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConvertirCsvJson();    
        }

        private static void ConvertirCsvJson() {

            ListaLinea lineas = new ListaLinea();

            //LeerCsv
            string csvFile = @"C:\dataPrueba\CRC05.csv";
            string[] lines = File.ReadAllLines(csvFile);

            foreach (var value in lines)
            {
                string[] columns = value.Split(',');
                lineas.Add(new Linea {
                    Categoria = columns[0],
                    CuentaOferta = columns[1],
                    CompuestaVariable = Convert.ToInt32(columns[2]),
                    CUC = columns[3],
                    SAP = columns[4],
                    FactorCuadre = Convert.ToInt32(columns[5]),
                    FactorRepeticion = Convert.ToInt32(columns[6]),
                    Palanca = columns[7],
                    Tactica = columns[8],
                    TipoOferta = columns[9],
                    IndicadorGratis = Convert.ToInt32(columns[10])
                });
            }

            //Obtener ofertas
            //lineas.Distinct()
            //IEnumerable<Oferta>
            //IEnumerable<Oferta> ofertas= lineas.Select(x => new Oferta
            List<Oferta> ofertas = lineas.Select(x => new Oferta
            {
                NumeroOferta = x.CuentaOferta,
                Palanca = x.Palanca,
                CompuestaVariable = x.CompuestaVariable,
                Tactica = x.Tactica
            }).ToList().Distinct(new OfertaComparer()).ToList();

            int NumeroOfertas = ofertas.Count();


            //Obtener productos por oferta

            //foreach (var ofer in ofertas)
            //{
            //    ofer.Productos = lineas.Where(y => y.CuentaOferta==ofer.NumeroOferta)
            //        .Select(x => new Producto(ofer.CompuestaVariable)
            //        {
            //            CUC = x.CUC,
            //            Categoria = x.Categoria,
            //            TO = x.TipoOferta,
            //            FactorCuadre = x.FactorCuadre,
            //            FactorRepeticion = x.FactorRepeticion
            //        }).ToList().Distinct(new ProductoComparer());
            //}

            Parallel.ForEach(ofertas, (ofer) =>
            {
                ofer.Productos = lineas.Where(y => y.CuentaOferta == ofer.NumeroOferta)
                    .Select(x => new Producto(ofer.CompuestaVariable)
                    {
                        CUC = x.CUC,
                        Categoria = x.Categoria,
                        TO = x.TipoOferta,
                        FactorCuadre = x.FactorCuadre,
                        FactorRepeticion = x.FactorRepeticion
                    }).ToList().Distinct(new ProductoComparer());

            });


            NumeroOfertas = ofertas.Count();

            //Obtener tonos por producto por oferta
            foreach (var ofer in ofertas) {
                foreach (var product in ofer.Productos) {
                    product.Tonos = lineas.Where(y => y.CuentaOferta == ofer.NumeroOferta && y.CUC == product.CUC)
                        .Select(x => new Tono
                        {
                            SAP = x.SAP
                        });
                }
            }

            NumeroOfertas = ofertas.Count();


            string[] bloques = SerializacionPorBloque(ofertas);


            #region PruebasEscritura

            /*Pocas ofertas
            //string ofertasJson = JsonConvert.SerializeObject(ofertas);

            //Escribir archivo
            string docPath =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Ofertas.json")))
            {

                int x= 0;
                foreach (var oferta in ofertas) {
                    string ofertaJson = JsonConvert.SerializeObject(oferta);
                    if (x == 0)
                        ofertaJson = "[" + ofertaJson + ",";
                    else if (x == ofertas.Count - 1)
                        ofertaJson = ofertaJson + "]";
                    else
                        ofertaJson = ofertaJson + ",";
                    outputFile.WriteLine(ofertaJson);
                    x++;
                }
            }
            */

            //using (StreamWriter file = File.CreateText(Path.Combine(docPath, "Ofertas.json")))
            //    {
            //         JsonSerializer serializer = new JsonSerializer();
            //         //serialize object directly into file stream
            //         serializer.Serialize(file, ofertas);
            //    }



            /*Muchas ofertas
            string docPath =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            List<Oferta> ofertasEscribir = null;
            string ofertasJson0=string.Empty;
            string ofertasJson1 = string.Empty;
            string ofertasJson2 = string.Empty;
            string ofertasJson3 = string.Empty;
            string ofertasJson4 = string.Empty;
            string ofertasJson5 = string.Empty;
            string ofertasJson6 = string.Empty;
            string ofertasJson7 = string.Empty;
            string ofertasJson8 = string.Empty;
            string ofertasJson9 = string.Empty;
            int CantidadTop = 100000;

            for (int i = 0; i< 9; i++) {
                ofertasEscribir = null;
                if (i == 0)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null) {
                        ofertasJson0 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);
                    }
                    

                }

                if (i == 1)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null)
                    {
                        ofertasJson1 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);
                    }
                    

                }

                if (i == 2)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null) {
                        ofertasJson2 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);
                    }

                }


                if (i == 3)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null)
                    {
                        ofertasJson3 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);
                    }
                    
                }

                if (i == 4)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null) {
                        ofertasJson4 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);
                    }
                }

                if (i == 5)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null)
                    {
                        ofertasJson5 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);
                    }
                }

                if (i == 6)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null)
                    {
                        ofertasJson6 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);

                    }

                }

                if (i == 7)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null) {
                        ofertasJson7 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);
                    }
                        

                }

                if (i == 8)
                {
                    ofertasEscribir = ofertas.Take(CantidadTop).ToList(); //top
                    if (ofertasEscribir != null)
                    {
                        ofertasJson8 = JsonConvert.SerializeObject(ofertasEscribir);
                        foreach (var of in ofertasEscribir)
                            ofertas.Remove(of);
                    }
                    

                }

                //if (i == 9)
                //{
                //    ofertasEscribir = ofertas.Take(1000).ToList(); //top
                //    if (ofertasEscribir != null) {
                //        ofertasJson9 = JsonConvert.SerializeObject(ofertasEscribir);
                //        foreach (var of in ofertasEscribir)
                //            ofertas.Remove(of);
                //    }
                //}
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Ofertas.json")))
            {
                if (!(ofertasJson0.Length == 2)) {
                    //ofertasJson0 = ofertasJson0.Remove(0, 1);
                    if (ofertasJson1.Length > 2) {
                        ofertasJson0 = ofertasJson0.Remove(ofertasJson0.Length - 1, 1);
                        ofertasJson0 = ofertasJson0 + ",";
                    }
                    outputFile.WriteLine(ofertasJson0);
                }

                if (!(ofertasJson1.Length ==2))
                {
                    ofertasJson1 = ofertasJson1.Remove(0, 1);
                    if (ofertasJson2.Length > 2) {
                        ofertasJson1 = ofertasJson1.Remove(ofertasJson1.Length - 1, 1);
                        ofertasJson1 = ofertasJson1 + ",";
                    }
                    outputFile.WriteLine(ofertasJson1);
                }

                if (!(ofertasJson2.Length == 2))
                {
                    ofertasJson2 = ofertasJson2.Remove(0, 1);
                    if (ofertasJson3.Length > 2) {
                        ofertasJson2 = ofertasJson2.Remove(ofertasJson2.Length - 1, 1);
                        ofertasJson2 = ofertasJson2 + ",";
                    }
                        
                    outputFile.WriteLine(ofertasJson2);
                }

                if (!(ofertasJson3.Length==2))
                {
                    ofertasJson3 = ofertasJson3.Remove(0, 1);
                    if (ofertasJson4.Length > 2) {
                        ofertasJson3 = ofertasJson3.Remove(ofertasJson3.Length - 1, 1);
                        ofertasJson3 = ofertasJson3 + ",";
                    }
                        
                    outputFile.WriteLine(ofertasJson3);
                }

                if (!(ofertasJson4.Length== 2))
                {
                    ofertasJson4 = ofertasJson4.Remove(0, 1);
                    if (ofertasJson5.Length > 2) {
                        ofertasJson4 = ofertasJson4.Remove(ofertasJson4.Length - 1, 1);
                        ofertasJson4 = ofertasJson4 + ",";
                    }
                        
                    outputFile.WriteLine(ofertasJson4);
                }

                if (!(ofertasJson5.Length ==2))
                {
                    ofertasJson5 = ofertasJson5.Remove(0, 1);
                    if (ofertasJson6.Length > 2) {
                        ofertasJson5 = ofertasJson5.Remove(ofertasJson5.Length - 1, 1);
                        ofertasJson5 = ofertasJson5 + ",";
                    }
                        
                    outputFile.WriteLine(ofertasJson5);
                }

                if (!(ofertasJson6.Length ==2))
                {
                    ofertasJson6 = ofertasJson6.Remove(0, 1);
                    if (ofertasJson7.Length > 2) {
                        ofertasJson6 = ofertasJson6.Remove(ofertasJson6.Length - 1, 1);
                        ofertasJson6 = ofertasJson6 + ",";
                    }
                        
                    outputFile.WriteLine(ofertasJson6);
                }

                if (!(ofertasJson7.Length ==2))
                {
                    ofertasJson7 = ofertasJson7.Remove(0, 1);
                    if (ofertasJson8.Length > 2) {
                        ofertasJson7 = ofertasJson7.Remove(ofertasJson7.Length - 1, 1);
                        ofertasJson7 = ofertasJson7 + ",";
                    }
                        
                    outputFile.WriteLine(ofertasJson7);
                }

                if (!(ofertasJson8.Length ==2))
                {
                    ofertasJson8 = ofertasJson8.Remove(0, 1);
                    if (ofertasJson9.Length > 2) {
                        ofertasJson8 = ofertasJson8.Remove(ofertasJson8.Length - 1, 1);
                        ofertasJson8 = ofertasJson8 + ",";
                    }
                        
                    outputFile.WriteLine(ofertasJson8);
                }

            //if (!(ofertasJson9.Length ==2))
            //{
            //    ofertasJson9 = ofertasJson9.Remove(0, 1);
            //    ofertasJson9 = ofertasJson9.Remove(ofertasJson9.Length - 1, 1);
            //    outputFile.Write(ofertasJson9);
            //}
            //outputFile.Write(ofertasJson0);
            //outputFile.Write(ofertasJson1);
            //outputFile.Write(ofertasJson2);
            //outputFile.Write(ofertasJson3);
            //outputFile.Write(ofertasJson4);
            //outputFile.Write(ofertasJson5);
            //outputFile.Write(ofertasJson6);
            //outputFile.Write(ofertasJson7);
            //outputFile.Write(ofertasJson8);
            //outputFile.Write(ofertasJson9);
        }*/

            #endregion
        }


        private static string[] SerializacionPorBloque(List<Oferta> ofertas)
        {

            string[] bloquesSerializados = null;


            var div = ofertas.Count / 5000;
            var mod = ofertas.Count % 5000;
            int cantidadBloques = div;

            if (mod > 0)
                cantidadBloques++;
            

            for (int i = 0; i < cantidadBloques; i++)
            {

            }





            return bloquesSerializados;
        }

    }
}
