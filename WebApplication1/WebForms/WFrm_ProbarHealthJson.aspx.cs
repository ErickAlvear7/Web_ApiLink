using Newtonsoft.Json;
using System;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace WebApplication1.WebForms
{
    public partial class WFrm_ProbarHealthJson : Page
    {

        Object[] objparam = new Object[3];
        DataSet api = new DataSet();
        string dtApi;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
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
                //GET ID CONTRACT
                string _idcont = new Functions().GetIdContract("https://api.eh.ehealthcenter.io/", _token);
                //GET ID SERVICIOS
                string _idserv = new Functions().GetServicios("https://api.eh.ehealthcenter.io/", _idcont, _token);
                //GET ID ESPECIALIDAD
                string _idespe = new Functions().GetEspecialidad("https://api.eh.ehealthcenter.io/", _token, _idcont);
                //CREAR PACIENTE Y OBTENER ID PATIENT
                var newPatient = new PostDataPatient
                {
                    name = "Erick",
                    surnames = "Alvear",
                    email = "erick.alvear7@gmail.com",
                    birthdate = "1982-08-15",
                    gender = "h",
                    phone = "",
                    role = 0,
                    oneclick = true,
                    contractId = _idcont,
                    customId = "",
                    cp = 0,
                    city = "Quito",
                    TyC_aceptados = false
                };

                var data = new JavaScriptSerializer().Serialize(newPatient);
                string _idpatient = new Functions().PostCreatePatient("https://api.eh.ehealthcenter.io/", data, _token);

                //GENERAR POST CONSULTA LINK

                var newConsulta = new PostConsulta
                {
                    idPatient = _idpatient,
                    idContrato = _idcont,
                    idEspecialidad = _idespe,
                    idServicio = _idserv,
                    date = "2024-12-27",
                    hour = "18:50",
                    timeZone = "",
                    reason = "dolor de cabeza",
                    idMedico = "",
                    customId = "",
                    oneclick = true

                };

            }
            catch (Exception ex)
            {

            }

        }
    }
}