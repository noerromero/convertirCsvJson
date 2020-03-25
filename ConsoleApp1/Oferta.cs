using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ConsoleApp1
{

    public class ListaLinea : List<Linea> { 

    }

    public class Linea {
        public string Categoria { get; set; }
        public string CuentaOferta { get; set; }
        public int? CompuestaVariable { get; set; }
        public string CUC { get; set; }
        public string SAP { get; set; }
        public int? FactorCuadre { get; set; }
        public int? FactorRepeticion { get; set; }
        public string Palanca { get; set; }
        public string Tactica { get; set; }
        public string TipoOferta { get; set; }

        public int? IndicadorGratis { get; set; }
    }

    public class ListaOferta : List<Oferta>
    {
    }
    public class Oferta
    {

        private string _formato;
        
        

        public string NumeroOferta { get; set; }
        public string Palanca { get; set; }
        
        public int? CompuestaVariable { get; set; }

        public string Tactica { get; set; }
        public IEnumerable<Producto> Productos { get; set; }
        public string Formato { 
            get {
                if (Tactica == "Sets")
                    _formato = "B";
                else if (Tactica == "Individual")
                    _formato = "I";
                else _formato = "V";
                return _formato;
            }
        } 
        

    }




    public class OfertaComparer : IEqualityComparer<Oferta>
    {

        public bool Equals(Oferta x, Oferta y)
        {
            if (x.NumeroOferta == y.NumeroOferta && 
                x.Palanca == y.Palanca
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public int GetHashCode(Oferta obj)
        {
            return obj.NumeroOferta.GetHashCode();
        }
    }


    public class ListaProducto: List<Producto> { 
    }

    public class Producto {

        private int? _compuestaVariable;
        private int? _unidades;

        public Producto(int? compuestaVariable) {
            _compuestaVariable = compuestaVariable;
        }

        public string CUC { get; set; }

        public string Categoria { get; set; }

        public string TO { get; set; }

        public int? FactorCuadre { get; set; }

        public int? FactorRepeticion { get; set; }

        public IEnumerable<Tono> Tonos { get; set; }

        public int? Unidades {
            get {
                if (_compuestaVariable == 0)
                     _unidades = FactorRepeticion;
                else
                     _unidades = FactorCuadre * FactorRepeticion;
                return _unidades;
            }
        }
        
    }

    public class ProductoComparer : IEqualityComparer<Producto>
    {
        public bool Equals(Producto x, Producto y)
        {
            if (x.CUC == y.CUC &&
                x.FactorCuadre == y.FactorCuadre &&
                x.FactorRepeticion == y.FactorRepeticion
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public int GetHashCode(Producto obj)
        {
            return obj.CUC.GetHashCode();
        }
    }

    public class ListaTono : List<Tono> { 
    }

    public class Tono {
        public string SAP { get; set; }

        public string CUV { get; set; }
    }
}
