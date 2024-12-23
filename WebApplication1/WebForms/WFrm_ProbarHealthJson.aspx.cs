using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Windows;

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

                string _apikey = JsonConvert.SerializeObject(apikey);
                string response = new Functions().GetToken("https://api.eh.ehealthcenter.io/apikey/", "login", _apikey);
                var jsonParse = JToken.Parse(response);
                var resToken = jsonParse.Value<JToken>("token").ToString();
                string resId = new Functions().GetIdContract("https://api.eh.ehealthcenter.io/", resToken);
                string idContract = (string)JArray.Parse(resId).Children()["id"].First();

                //ViewState["Token"] = resToken;

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
                    contractId = idContract,
                    customId = "",
                    cp = 0,
                    city = "Quito",
                    TyC_aceptados = false
                };

                var json = new JavaScriptSerializer().Serialize(newPatient);

                string resPatient = new Functions().PostCreatePatient("https://api.eh.ehealthcenter.io/", json, resToken);

                string idpatient = (string)JArray.Parse(resId).Children()["id"].First();


            }
            catch (Exception ex)
            {

            }

        }
    }
}