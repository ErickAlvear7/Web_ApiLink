using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        string dtApi;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {

            int codTitu = 88888;
            int codBene = 0;
            int codPro = 227;
            string _idcont = "";
            string _idserv = "";
            string _idespe = "";
            string _idpatient = "";
            string _datalink = "";
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

                Array.Resize(ref objparam, 3);
                objparam[0] = codTitu;
                objparam[1] = "";
                objparam[2] = 183;
                link = new Conexiones.Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if(link != null && link.Tables[0].Rows.Count>0)
                {
                    foreach (DataRow dr in link.Tables[0].Rows)
                    {
                        _idpatient = dr[0].ToString();
                        _idcont = dr[1].ToString();
                        _idespe = dr[2].ToString();
                        _idserv = dr[3].ToString();
                    }
                }
                else
                {
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
                        email = "ealvear@prestasalud.com",
                        birthdate = "1982-08-15",
                        gender = "m",
                        phone = "0962655679",
                        contractId = _idcont,
                    };

                    var data = new JavaScriptSerializer().Serialize(newPatient);
                    _idpatient = new Functions().PostCreatePatient("https://api.eh.ehealthcenter.io/", data, _token);

                    Array.Resize(ref objlinkid, 8);
                    objlinkid[0] = 0;
                    objlinkid[1] = codTitu;
                    objlinkid[2] = codBene;
                    objlinkid[3] = codPro;
                    objlinkid[4] = _idpatient;
                    objlinkid[5] = _idcont;
                    objlinkid[6] = _idespe;
                    objlinkid[7] = _idserv;

                    link = new Conexiones.Conexion(2, "").funConsultarSqls("sp_GrabarIdLink", objlinkid);

                    foreach (DataRow dr in link.Tables[0].Rows)
                    {
                        _idpatient = dr[0].ToString();
                        _idcont = dr[1].ToString();
                        _idespe = dr[2].ToString();
                        _idserv = dr[3].ToString();
                    }
                }

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

                var dataconsulta = new JavaScriptSerializer().Serialize(newConsulta);
                _datalink = new Functions().Consultas("https://api.eh.ehealthcenter.io/", dataconsulta, _token);
                dynamic urlLink = JObject.Parse(_datalink);
                //string doc = urlLink.doctor.nombre;
                string url = urlLink.url_llamada;
                string fecha = urlLink.fecha;
                string motivo = urlLink.motivo;

                //MessageBox.Show(url);
                MessageBox.Show(fecha);
                //MessageBox.Show(motivo);




            }
            catch (Exception ex)
            {

            }

        }
    }
}