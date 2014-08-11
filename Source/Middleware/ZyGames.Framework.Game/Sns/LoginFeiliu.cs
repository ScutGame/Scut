using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.Game.Sns
{

    class LoginFeiliu : AbstractLogin
    {

        private string _retailID = string.Empty;
        private string _retailUser = string.Empty;
        private string _uuid = string.Empty;
        private string _sign = string.Empty;
        private string _timestamp = string.Empty;
        private string _PublicKey = string.Empty;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.LoginTencent"/> class.
        /// </summary>
        public LoginFeiliu(string retailID, string retailUser, string sign, string timestamp)
        {
            this._retailID = retailID;
            this._retailUser = retailUser;
            this._uuid = retailUser;
            this._sign = sign;
            this._timestamp = timestamp;
        }

        public override string GetRegPassport()
        {
            return this.PassportID;
        }

        public override bool CheckLogin()
        {
            var signformbase64 = Decrypt(_sign);

            string[] arr = SnsManager.LoginByRetail(_retailID, _retailUser);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            QihooUserID = _retailUser;
            SessionID = GetSessionId();
            return true;
        }


        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="base64code">传入加密数据</param>
        /// <returns>返回解密数据</returns>
        static public string Decrypt(string base64code)
        {

            var a = new FileInfo("E:/100115_SignKey.pub").OpenRead();
            var b = new BufferedStream(a);
            //string c = 
            try
            {
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString("");

                RSAParameters rsaParameters = new RSAParameters();

                rsaParameters.Exponent = Convert.FromBase64String("AQAB");
                rsaParameters.Modulus =
                    Convert.FromBase64String(
                        "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyq3xJ3jtuWSWk4nCCgysplqV3DyFGaF7iP7PO2vEUsgEq+vqKr+frlwji2n7A1TbpV7KhEGJIT9LW/9WCdBhlu6gnBdErtAA4Or43ol2K1BnY6VBcLWccloMd3YFHG8gOohCVIDbw863Wg0FNS27SM25U+XQfrNFaqBIa093WgAbwRIK06uzC01sW+soutvk+yAYBtbH7I7/1/dFixHKS2KN/7y3pvmXYBIRuBvn35IqwY3Gk0duEfbEr9F6wm2VKhS1zQG760FrHfhbXR+IN5nSTQBHBkw4QukLLvUqueKYfVdp2/2RCnY/At0bbOcA2tAPohDAfUDRdOZsFiTIMQID");

                byte[] encryptedData;
                byte[] decryptedData;

                encryptedData = Convert.FromBase64String(base64code);

                decryptedData = RSADeCrtypto(encryptedData, rsaParameters, true);
                return ByteConverter.GetString(decryptedData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="DataToDeCrypto">要解密的数据</param>
        /// <param name="RSAKeyInfo"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        static public byte[] RSADeCrtypto(byte[] DataToDeCrypto, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                // System.Security.Cryptography.RSA 的参数。
                RSA.ImportParameters(RSAKeyInfo);
                //
                // 参数:
                //  
                //     要解密的数据。
                //
                //
                //     如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的 System.Security.Cryptography.RSA
                //     解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。
                return RSA.Decrypt(DataToDeCrypto, DoOAEPPadding);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }
}
