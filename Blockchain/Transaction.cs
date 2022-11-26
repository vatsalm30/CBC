using System;
using static Utils;

namespace N_Blockchain
{
    public class Transaction
    {
        public string sender;
        public string reciver;
        public double amt;
        public int txnNumber;
        public string txnHash;
        public int time;
        public int txnBlockNum;
        public Transaction(string _sender, string _reciver, double _amt, int _txnNumber, int _txnBlockNum)
        {
            sender = _sender;
            reciver = _reciver;
            amt = _amt;
            txnNumber = _txnNumber;
            time = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            txnBlockNum = _txnBlockNum;
            txnHash = GetHash(sender, reciver, txnBlockNum, time, amt, txnNumber);
        }

        public string GetHash(string sender, string reciver, int txnBlockNum, int time, double amt, int txnNum)
        {
            string encodingString = "";

            encodingString += sender + reciver + txnBlockNum.ToString() + time.ToString() + txnNum.ToString() + amt.ToString();

            return ToSHA512(encodingString);
        }
    }
}
