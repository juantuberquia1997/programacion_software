using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiSusCopias.Models
{
    public class modSusCopias
    {

        public float vrC { get; set; }
        public float vrO { get; set; }
        public float vrE { get; set; }
        public float kC { get; set; }
        public float kO { get; set; }
        public float kE { get; set; }
        public float vrToctC { get; set; }
        public float vrToTO { get; set; }
        public float vrTotE { get; set; }
        public float vrSubtot { get; set; }
        public float porcDsto { get; set; }
        public float vrDsto { get; set; }
        public float valorIva { get; set; }
        public float vrApag { get; set; }

        public string error { get; set; }

        public modSusCopias()
        {
            this.vrC = 0;
            this.vrO = 0;
            this.vrE = 0;
            this.kC = 0;
            this.kE = 0;
            this.kO = 0;
            this.vrToctC = 0;
            this.vrToTO = 0;
            this.vrTotE = 0;
            this.vrSubtot = 0;
            this.porcDsto = 0;
            this.vrDsto = 0;
            this.valorIva = 0;
            this.vrApag = 0;
            this.error = "";
        }
    }
}