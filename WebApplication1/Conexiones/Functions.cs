using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using System.Windows;

public class Functions
{

    public string GetToken(string url, string metodo, string _json)
    {

        try
        {

            string _urlmetodo = url + metodo;
            HttpClient _client = new HttpClient();
            {
                _client.BaseAddress = new Uri(url);
                //_client.DefaultRequestHeaders.Add("ContentType", "application/json");
                //_client.DefaultRequestHeaders.Add("Authorization", "Basic OTU4OTMzMjA1Om9NYkVhcFA5MGwzN21nalU=");
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            var _response = _client.PostAsync("login", _content).Result;

            if (_response.IsSuccessStatusCode)
            {
                var responseContent = _response.Content.ReadAsStringAsync().Result;
               
                return responseContent.ToString();
            }
            else
            {
                MessageBox.Show(_response.StatusCode.ToString());

                
            }
            //_response.Wait();
            //HttpResponseMessage _respuesta = _response.Result;
            //var _responseJason = _respuesta.Content.ReadAsStreamAsync();
            //dynamic token = JObject.Parse(); 
            
        }
        catch(Exception ex){
            return ex.ToString();
        }

        return "";

    }

    public dynamic Post(string url,string json, string autorizacion = null)
    {
        try
        {

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            if(autorizacion != null)
            {
                request.AddHeader("Authorization", autorizacion);
            }

            IRestResponse response = client.Execute(request);
            dynamic datos = JsonConvert.DeserializeObject(response.Content);

            



            return datos;
                    

        }catch(Exception ex){

            MessageBox.Show(ex.Message);
            return null;

        }

    }






}