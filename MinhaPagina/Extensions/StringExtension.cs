using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtension
    {
        public static string ShortName(this string nome)
        {
            if (string.IsNullOrEmpty(nome)) return "";

            var array = nome.Trim().Split(" ");

            if (array.Length > 1)
            {
                return array[0] + " " + array[^1];
            }
            else
            {
                return array[0];
            }

        }

        public static string Cut(this string str, int size)
        {
            if (str.Length <= size)
            {
                return str;
            }
            else
            {
                return str[..size] + "...";
            }
        }

        public static string ToMd5(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            var password = (str += "|0EF4459A-6E56-40FC-9D70-72C687011250");
            var md5 = Security.Cryptography.MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(password));
            var sbString = new StringBuilder();

            foreach (var t in data)
                sbString.Append(t.ToString("x2"));

            return sbString.ToString();
        }

        public static string RandomLetra
        {
            get
            {
                List<string> letras = new()
                {
                    "a",
                    "b",
                    "c",
                    "d",
                    "e",
                    "f",
                    "g",
                    "h",
                    "i",
                    "j",
                    "k",
                    "l",
                    "m",
                    "n",
                    "o",
                    "p",
                    "q",
                    "r",
                    "s",
                    "t",
                    "u",
                    "v",
                    "w",
                    "x",
                    "y",
                    "z",
                    "A",
                    "B",
                    "C",
                    "D",
                    "E",
                    "F",
                    "G",
                    "H",
                    "I",
                    "J",
                    "K",
                    "L",
                    "M",
                    "N",
                    "O",
                    "P",
                    "Q",
                    "R",
                    "S",
                    "T",
                    "U",
                    "V",
                    "W",
                    "X",
                    "Y",
                    "Z"
                };

                int contLetras = letras.Count;
                Random random = new();
                int rLetra;
                do
                {
                    rLetra = random.Next(0, contLetras);

                } while (rLetra < 0 && rLetra >= contLetras);

                return letras[rLetra];
            }
        }
        
        public static string RandomLetraNumero
        {
            get
            {
                List<string> letras = new()
                {
                    "a",
                    "b",
                    "c",
                    "d",
                    "e",
                    "f",
                    "g",
                    "h",
                    "i",
                    "j",
                    "k",
                    "l",
                    "m",
                    "n",
                    "o",
                    "p",
                    "q",
                    "r",
                    "s",
                    "t",
                    "u",
                    "v",
                    "w",
                    "x",
                    "y",
                    "z",
                    "A",
                    "B",
                    "C",
                    "D",
                    "E",
                    "F",
                    "G",
                    "H",
                    "I",
                    "J",
                    "K",
                    "L",
                    "M",
                    "N",
                    "O",
                    "P",
                    "Q",
                    "R",
                    "S",
                    "T",
                    "U",
                    "V",
                    "W",
                    "X",
                    "Y",
                    "Z",
                    "0",
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9"
                };

                int contLetras = letras.Count;
                Random random = new();
                int rLetra;
                do
                {
                    rLetra = random.Next(0, contLetras);

                } while (rLetra < 0 && rLetra >= contLetras);

                return letras[rLetra];
            }
        }

        public static string Criptografar(this string dados)
        {
            var valueBytes = Encoding.UTF8.GetBytes(dados);
            string resultado = Convert.ToBase64String(valueBytes).Replace("=", "igual");

            return $"{resultado}{RandomLetraNumero}{RandomLetraNumero}{RandomLetraNumero}";
        }

        public static string Descriptografar(this string dados)
        {
            string resultado = dados[0..^3].Replace("igual", "=");
            var valueBytes = Convert.FromBase64String(resultado);

            return Encoding.UTF8.GetString(valueBytes);
        }
        
        public static string CriptografarAvancado(this string dados)
        {
            var valueBytes = Encoding.UTF8.GetBytes(dados);
            string resultado = Convert.ToBase64String(valueBytes).Replace("=", "igual");
            string primeira = $"{resultado}{RandomLetraNumero}{RandomLetraNumero}{RandomLetraNumero}";
            var newValueBytes = Encoding.UTF8.GetBytes(primeira);
            string newResultado = Convert.ToBase64String(newValueBytes).Replace("=", "igual");
            string ultima = $"{RandomLetra}{RandomLetraNumero}{RandomLetraNumero}{newResultado}{RandomLetraNumero}{RandomLetraNumero}{RandomLetraNumero}=";            
            return ultima;
        }

        public static string DescriptografarAvancado(this string dados)
        {
            string primeira = dados[3..^4].Replace("igual", "=");
            var valueBytes = Convert.FromBase64String(primeira);
            string resultado = Encoding.UTF8.GetString(valueBytes);
            string segunda = resultado[0..^3].Replace("igual", "=");
            var newValueBytes = Convert.FromBase64String(segunda);
            string valorfinal = Encoding.UTF8.GetString(newValueBytes);
            return valorfinal;
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new((Stream)cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new((Stream)cryptoStream);
            return streamReader.ReadToEnd();
        }

        public static string PrimeiroUltimoNome(this string nomecompleto)
        {
            string nome;
            try
            {
                string[] usuNome = nomecompleto.Split(' ');
                if (usuNome.Length > 1)
                {
                    nome = usuNome[0] + " " + usuNome[^1];
                }
                else
                {
                    nome = nomecompleto.PrimeirasMaiusculas();
                }
            }
            catch
            {
                nome = nomecompleto.PrimeirasMaiusculas();
            }
            return nome;
        }
        
        public static string PrimeiroUltimoNomeAbrev(this string nomecompleto)
        {
            string nome;
            try
            {
                string priMaiusculas = nomecompleto.PrimeirasMaiusculas();
                string[] usuNome = priMaiusculas.Split(' ');
                if (usuNome.Length > 1)
                {
                    nome = $"{usuNome[0]} {usuNome[^1][..1]}.";
                }
                else
                {
                    nome = priMaiusculas;
                }
            }
            catch
            {
                nome = nomecompleto.PrimeirasMaiusculas();
            }
            return nome;
        }

        public static string PrimeirasMaiusculas(this string frase)
        {
            string nova;
            try
            {
                nova = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(frase.ToLower());
            }
            catch
            {
                nova = frase;
            }
            return nova;
        }

        public static string SubstituirCaract(this string palavra, string de, string para)
        {
            string nova = palavra.Replace(de, para);
            while (nova.Contains(de))
            {
                nova = nova.Replace(de, para);
            }
            return nova;
        }

        public static string AcrescentarZerosCPF(this string CPFsemZero)
        {
            string CpfComZero = CPFsemZero;

            while (CpfComZero.Length < 11)
            {
                CpfComZero = "0" + CpfComZero;
            }

            return CpfComZero;
        }

        public static Guid ToGuid(this string id)
        {
            try
            {
                return Guid.Parse(id);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static List<T> ToList<T>(this string lista)
        {
            try
            {
                if (lista.Length > 0)
                {
                    return JsonConvert.DeserializeObject<List<T>>(lista) ?? new();
                }
                else
                {
                    return new();
                }
            }
            catch
            {
                return new();
            }
        }

        public static List<T> ToList<T>(this string lista, string separacao)
        {
            try
            {
                if (lista.Length > 0)
                {
                    if (lista.IndexOf(separacao) > -1)
                    {
                        string textoJson = JsonConvert.SerializeObject(lista.Split(separacao).ToList());
                        return JsonConvert.DeserializeObject<List<T>>(textoJson) ?? new();
                    }
                    else
                    {
                        string[] lst = new string[] { lista };
                        string textoJson = JsonConvert.SerializeObject(lst.ToList());
                        return JsonConvert.DeserializeObject<List<T>>(textoJson) ?? new();
                    }
                }
                else
                {
                    return new();
                }
            }
            catch
            {
                return new();
            }
        }
        
        public static T? DeserializeObject<T>(this string texto)
        {
            return JsonConvert.DeserializeObject<T>(texto);
        }

        public static string ToOcultPassword(this string senha)
        {
            string novasenha = "*";
            while (novasenha.Length < senha.Length)
            {
                novasenha += "*";
            }
            return novasenha;
        }

        public static string ToPartialOcultPassword(this string senha)
        {
            int qtdsenha = senha.Length;
            int qtdexibir = (int)Math.Round((double)qtdsenha * (double)0.25, 0);
            if (qtdexibir < 3)
            {
                qtdexibir = 3;
            }
            string novasenha = senha[..qtdexibir];
            while (novasenha.Length < qtdsenha)
            {
                novasenha += "*";
            }
            return novasenha;
        }

        public static string ToUTF8(this string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }

        public static string ASCIIToUTF8(this string text)
        {
            return Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(text));
        }

        public static string UnicodeToUTF8(this string text)
        {
            return Encoding.UTF8.GetString(Encoding.Unicode.GetBytes(text));
        }

        public static string ToConvertEncoding(this string text, Encoding encodingDe, Encoding encodingPara)
        {
            return encodingPara.GetString(encodingDe.GetBytes(text));
        }
    }
}
