using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using apiSusCopias.clases;
using apiSusCopias.Models;


namespace apiSusCopias.Controllers
{

    [EnableCors(origins: "http://localhost:55823", headers: "*", methods:"*" )]
    public class servSusCopiasController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value " + id;
        }

        // POST api/<controller>
        public modSusCopias Post([FromBody] modSusCopias ObjectMod)
        {

            clsOpeSusCopias objectOpen = new clsOpeSusCopias();
            objectOpen.pMod = ObjectMod;
            objectOpen.facturar();
            return objectOpen.pMod;


        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}