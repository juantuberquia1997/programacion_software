using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using apiSusCopias.Models; // importo todo el contenido de models

namespace apiSusCopias.clases
{
    public class clsOpeSusCopias
    {
        public modSusCopias pMod { get; set; } 

        //metodos privados
        private bool validar() {

            if (pMod == null) {
                pMod.error = "sin informacion";
                return false;

            }

            if (pMod.vrC <= 0) {
                pMod.error = "valor de la copia carta no es valido";
                return false;

            }

            if (pMod.vrO <= 0)
            {
                pMod.error = "valor de la copia oficio no es valido";
                return false;

            }

            if (pMod.vrE <= 0)
            {
                pMod.error = "valor de la copia extra oficio no es valida";
                return false;

            }

            if (pMod.kC < 0)
            {
                pMod.error = "cantidad de copias carta no es valido";
                return false;

            }

            if (pMod.kO < 0)
            {
                pMod.error = "cantidad de oficio no es valido";
                return false;

            }

            if (pMod.kE < 0)
            {
                pMod.error = "cantidad de extra oficio no es valido";
                return false;

            }

            return true;

        }


        private void hallarDescuento() {

            float cantidadCartas = pMod.kC + pMod.kO + pMod.kE;
            pMod.vrSubtot =  pMod.kC * pMod.vrC + pMod.kO* pMod.vrO + pMod.kE* pMod.vrE ;

            if (cantidadCartas >= 50 && cantidadCartas <= 100)
            {
                pMod.porcDsto = 10f;

            }
            else if (cantidadCartas > 100)
            {
                pMod.porcDsto = 15f;
            }

            pMod.vrDsto = pMod.vrSubtot * pMod.porcDsto / 100;
            pMod.valorIva = (pMod.vrSubtot - pMod.vrDsto) * 0.75f;
            pMod.vrApag = pMod.vrSubtot - pMod.vrDsto + pMod.valorIva;
            pMod.vrToctC = pMod.kC * pMod.vrC;
            pMod.vrToTO = pMod.kO * pMod.vrO;
            pMod.vrTotE = pMod.kE * pMod.vrE;

        }


        //metodos publicos

        public void facturar() {

            if (!validar())
            {
                return;
            }
            else {

                hallarDescuento();

            }

        }


    }
}