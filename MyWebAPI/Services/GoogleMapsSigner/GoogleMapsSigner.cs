using System.Security.Cryptography;
using System.Text;

namespace MyWebAPI.Services.GoogleMapsSigner;

public struct GoogleMapsSigner
{

    public static string Sign(string url, string keyString)
    {
        ASCIIEncoding encoding = new();

        // converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
        string usablePrivateKey = keyString.Replace("-", "+").Replace("_", "/");
        byte[] privateKeyBytes = Convert.FromBase64String(usablePrivateKey);

        Uri uri = new(url);
        byte[] encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

        // compute the hash
        HMACSHA1 algorithm = new(privateKeyBytes);
        byte[] hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

        // convert the bytes to string and make url-safe by replacing '+' and '/' characters
        string signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");

        // Add the signature to the existing URI.
        return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
    }
}

