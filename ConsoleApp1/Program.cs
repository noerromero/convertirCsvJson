using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            IEnumerable<Oferta> ofertas= lineas.Select(x => new Oferta
            {
                NumeroOferta = x.CuentaOferta,
                Palanca = x.Palanca,
                CompuestaVariable = x.CompuestaVariable,
                Tactica = x.Tactica
            }).ToList().Distinct(new OfertaComparer());

            int NumeroOfertas = ofertas.Count();


            //Obtener productos por oferta

            foreach (var ofer in ofertas)
            {
                ofer.Productos = lineas.Where(y => y.CuentaOferta==ofer.NumeroOferta)
                    .Select(x => new Producto(ofer.CompuestaVariable)
                    {
                        CUC = x.CUC,
                        Categoria = x.Categoria,
                        TO = x.TipoOferta,
                        FactorCuadre = x.FactorCuadre,
                        FactorRepeticion = x.FactorRepeticion
                    }).ToList().Distinct(new ProductoComparer());
            }

            NumeroOfertas = ofertas.Count();

            //Obtener tonos por producto por oferta
            foreach (var ofer in ofertas) {
                foreach (var product in ofer.Productos) {
                    product.Tonos = lineas.Where(y=>y.CuentaOferta==ofer.NumeroOferta && y.CUC==product.CUC)
                        .Select(x => new Tono
                        {
                            SAP = x.SAP
                        });
                }
            }

            NumeroOfertas = ofertas.Count();

            string ofertasJson = JsonConvert.SerializeObject(ofertas);

            //Escribir archivo
            string docPath =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Ofertas.json")))
            {
                outputFile.WriteLine(ofertasJson);
            }


        }


    }
}
