using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CGP2_1.Clase
{
    public class Token
    {
        public static string Test11(string val)
        {
            string enc = Encriptar(val);
            return enc;
        }

        public static string Encriptar(string texto)
        {
            try
            {

                string key = "qualityinfosolutions"; //llave para encriptar datos

                byte[] keyArray;

                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);

                //Se utilizan las clases de encriptación MD5

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                //Algoritmo TripleDES
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

                tdes.Clear();

                //se regresa el resultado en forma de una cadena
                texto = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);

            }
            catch (Exception)
            {

            }
            return texto;
        }

        public static string Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "qualityinfosolutions";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);

                //algoritmo MD5
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception)
            {

            }
            return textoEncriptado;
        }


        //genero una estructura JSON a partir de unas clases
        public void json2()
        {
            string a = "{ 'Cont': 'Test', 'Meth': 'methodtest', 'par':{ 'clie': 'cliente234', 'otro': 'otro_fdd' } }";
            var a1 = JObject.Parse(a);

            var b = new jsonmeth { cont = "controller", meth = "method", clas = new par { clien = "cliente", otro = "otro" } };
            var b1 = JObject.FromObject(b);


            var ser = JsonConvert.SerializeObject(b1);
            var enc = Encriptar(ser);
            var des = Desencriptar(enc);
            var for_url = System.Web.HttpUtility.UrlEncode(enc);

            var enc2 = Encriptar(b1.ToString());
            var des2 = Desencriptar(enc2);

            jsonmeth js = JsonConvert.DeserializeObject<jsonmeth>(des);
            jsonmeth js2 = JsonConvert.DeserializeObject<jsonmeth>(des2);
        }

        class jsonmeth
        {
            public string cont { get; set; }
            public string meth { get; set; }
            public object clas { get; set; }
        }

        class par
        {
            public string clien { get; set; }
            public string otro { get; set; }
        }
    }
}