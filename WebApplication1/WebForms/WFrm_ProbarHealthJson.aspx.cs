using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
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

            //string tok = JObject.Parse(response.token);

            //ViewState["Token"] = response;
            MessageBox.Show(response);

            

            

        }
    }
}