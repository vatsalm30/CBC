using System.Security.Cryptography;
using System;
using System.Numerics;
using System.Text;
using Org.BouncyCastle.Asn1.Sec;
using SimpleBase;
using N_Blockchain;

public static class Utils
{
    public static string ToSHA512(string s)
    {
        using var sha512 = SHA512.Create();
        byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(s));

        var sb = new StringBuilder();

        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x2"));
        }
        return sb.ToString();
    }

    public static byte[] CreatePrivKey()
    {

        static byte[] Random512Bits()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[64];
                rng.GetBytes(bytes);

                return bytes;
            }
        }
        return Random512Bits();
    }



    public static string CreatepubKey(byte[] privKey)
    {
        Org.BouncyCastle.Math.BigInteger privKeyInt = new Org.BouncyCastle.Math.BigInteger(privKey);
        var parameters = SecNamedCurves.GetByName("secp256k1");
        Org.BouncyCastle.Math.EC.ECPoint qa = parameters.G.Multiply(privKeyInt);

        var pubKeyInt = qa.XCoord.ToString() + qa.YCoord.ToString();
        return pubKeyInt;
    }

    public static string GeneratAddress(string pubKey)
    {
        string hash320 = ToSHA512(pubKey);
        Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
        crypt.HashAlgorithm = "ripemd320";
        hash320 = crypt.HashStringENC(hash320);
        byte[] bytes = Convert.FromBase64String(hash320);
        BigInteger intHash = new BigInteger(bytes);

        hash320 = Base58.Bitcoin.Encode(bytes);

        return hash320;
    }

    public static List<Transaction> EncryptedTXNToNormalTXN(List<txnEncriptor> backLogEc)
    {
        List<Transaction> transactions = new List<Transaction>();
        foreach(txnEncriptor txn in backLogEc)
        {
            Transaction t = new Transaction(txn.sender, txn.reciver, txn.amt, txn.txnNumber, txn.txnBlockNum);
            transactions.Add(t);
        }
        return transactions;
    }
}
