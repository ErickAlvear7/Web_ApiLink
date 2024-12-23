using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Token
{
    public string TokenGet { get; set; }
    public string TokenRefresh { get; set; }
}
public class ApiKey
{
    public string api_key { get; set; }
}

public class Idcontract
{
    public string idContrac { get; set; }
}

public class PostDataPatient
{

    public string name { get; set; }
    public string surnames { get; set; }
    public string email { get; set; }
    public string birthdate { get; set; }
    public string gender { get; set; }
    public string phone { get; set; }
    public int role { get; set; }
    public bool oneclick { get; set; }
    public string contractId { get; set; }
    public string customId { get; set; }
    public int cp { get; set; }
    public string city { get; set; }
    public bool TyC_aceptados { get; set; }







}