using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abm_carness
{
    class Productos
    {
        int codigo;
        double precio;
        int stock;
        int tipo;
        int subtipo;



        public int pCodigo
        {
            get { return codigo; }
            set { codigo = value; }
        }




        public double pPrecio
        {
            get { return precio; }
            set { precio = value; }
        }
        public int pStock
        {
            get { return stock; }
            set { stock = value; }
        }


        public int pTipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
        int marca;

        public int pSubtipo
        {
            get { return subtipo; }
            set { subtipo = value; }
        }


        override public string ToString()
        {
            return codigo + " - " + precio;
        }

    }
}
    

