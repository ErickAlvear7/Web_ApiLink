using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
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


    public string GetIdContract(string url,string auth)
    {
        try
        {

            HttpClient _contract = new HttpClient();
            {
                _contract.BaseAddress = new Uri(url);
                _contract.DefaultRequestHeaders.Add("Authorization","Bearer " + auth);
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var resConId = _contract.GetAsync("contratos").Result;

            if (resConId.IsSuccessStatusCode)
            {
                var responseId = resConId.Content.ReadAsStringAsync().Result;
                return responseId.ToString();
            }
            else
            {
                MessageBox.Show(resConId.StatusCode.ToString());
            }

        }
        catch (Exception ex)
        {
            return ex.ToString();

        }


        return "";
    }

    public string PostCreatePatient(string url,string dataPatient, string auth)
    {

        HttpClient _patient = new HttpClient();
        {
            _patient.BaseAddress = new Uri(url);
            _patient.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpContent _content = new StringContent(dataPatient, Encoding.UTF8, "application/json");
            var _resPatient = _patient.PostAsync("patient", _content).Result;

            if (_resPatient.IsSuccessStatusCode)
            {
                var responseContent = _resPatient.Content.ReadAsStringAsync().Result;

                return responseContent.ToString();
            }
            else
            {
                MessageBox.Show(_resPatient.StatusCode.ToString());


            }


        }
        return "";
    }
}