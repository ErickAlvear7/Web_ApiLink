using Newtonsoft.Json;
using System;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Windows;

namespace WebApplication1.WebForms
{
    public partial class WFrm_ProbarHealthJson : Page
    {

        Object[] objparam = new Object[3];
        Object[] objlinkid = new Object[3];
        DataSet api = new DataSet();
        DataSet link = new DataSet();
        string dtApi,dtlink;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {

            int codTitu = 212123;
            int codBene = 0;
            int codPro = 225;
            string _idcont = "";
            string _idserv = "";
            string _idespe = "";
            string _idpatient = "";
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = "";
                objparam[2] = 182;
                api = new Conexiones.Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                dtApi = api.Tables[0].Rows[0][0].ToString();

                ApiKey apikey = new ApiKey
                {
                    api_key = dtApi
                };

                //GET TOKEN
                string _apikey = JsonConvert.SerializeObject(apikey);
                string _token = new Functions().GetToken("https://api.eh.ehealthcenter.io/apikey/", _apikey);

                Array.Resize(ref objlinkid, 7);
                objlinkid[0] = 0;
                objlinkid[1] = codTitu;
                objlinkid[2] = codBene;
                objlinkid[3] = 0;
                objlinkid[4] = "";
                objlinkid[5] = "";
                objlinkid[6] = "";
                link = new Conexiones.Conexion(2, "").funConsultarSqls("sp_GrabarIdLink", objlinkid);
                //dtlink = link.Tables[0].Rows[0][0].ToString();

                foreach (DataRow dr in  link.Tables[0].Rows)
                {

                }

          
                //GET ID CONTRACT
                 _idcont = new Functions().GetIdContract("https://api.eh.ehealthcenter.io/", _token);
                //GET ID SERVICIOS
                 _idserv = new Functions().GetServicios("https://api.eh.ehealthcenter.io/", _idcont, _token);
                //GET ID ESPECIALIDAD
                _idespe = new Functions().GetEspecialidad("https://api.eh.ehealthcenter.io/", _token, _idcont);
                //CREAR PACIENTE Y OBTENER ID PATIENT
                var newPatient = new PostDataPatient
                {
                    name = "Erick",
                    surnames = "Alvear",
                    email = "erick.alvear7@gmail.com",
                    birthdate = "1982-08-15",
                    gender = "h",
                    phone = "022630922",
                    contractId = _idcont,
                };

                var data = new JavaScriptSerializer().Serialize(newPatient);
                _idpatient = new Functions().PostCreatePatient("https://api.eh.ehealthcenter.io/", data, _token);

                //GUARDAR EN LA BDDD ID GENERADOS


              
                //GENERAR POST CONSULTA LINK

                var newConsulta = new PostConsulta
                {
                    idPatient = _idpatient,
                    idContrato = _idcont,
                    idEspecialidad = _idespe,
                    idServicio = _idserv,
                    reason = "dolor de cabeza",
                };



            }
            catch (Exception ex)
            {

            }

        }
    }
}