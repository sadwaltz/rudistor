using System.Windows;
using GalaSoft.MvvmLight.Threading;
using rudistor.Services;
using Newtonsoft.Json;
using System;
using rudistor.Model;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using GalaSoft.MvvmLight.Ioc;

namespace rudistor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
           

            //User loginUser = new User();
            //loginUser.UserId = "hahaha";
            //loginUser.Pass = "blabla";
            //loginUser.BrokerId = "01020000";
            //loginUser.InvestorId = "jb";
            //Response<String> response = new Response<string>();
            //List<String> payload = new List<string>();
            //payload.Add("test1");
            //payload.Add("test2");
            //Message<User,String> login = new Message<User,String>(loginUser,payload,"login","01020000");
            //login.response.Payload.Add("test1");
            //login.response.Payload.Add("test2");
            //login.response = null;
            //Console.WriteLine(JsonConvert.SerializeObject(login));
            //System.Windows.MessageBox.Show(JsonConvert.SerializeObject(login));
            //MessageBox.Show(JsonConvert.SerializeObject(login));
            //String test = "{\"cmd\":\"login\",\"traceNo\":\"01020000\",\"request\":{\"UserId\":\"hahaha\",\"Pass\":\"blabla\",\"BrokerId\":\"01020000\",\"InvestorId\":\"jb\"},\"response\":{\"ErrorID\":null,\"ErrorMsg\":null,\"Payload\":[\"pay1\",\"pay2\",\"pay3\"]}}";
            //String test = JsonConvert.SerializeObject(login);
            //JObject loginResponse = JObject.Parse(test);
            //JEnumerable<JToken> results = loginResponse["response"].Children();
            //JEnumerable<JToken> payloads = loginResponse["response"]["Payload"].Children();
            //Message<User, String> loginRes = JsonConvert.DeserializeObject<Message<User, String>>(test);
            //LoginMessage loginRes = JsonConvert.DeserializeObject<LoginMessage>(test);
            //Console.WriteLine(JsonConvert.SerializeObject(loginRes.response.Payload.ToString()));
            //MessageBox.Show(loginResponse["request"]["Pass"].ToString());
        }
    }
}
