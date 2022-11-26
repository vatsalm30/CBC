using System;
using static Utils;


namespace N_Blockchain
{
    class Block
    {
        int blockNumber;
        public string prevHash;
        public string hash;
        public int nonce;
        public int time;
        public List<Transaction> transactions;
        public bool isValid;
        string miningZeros;

        public Block(int _blockNumber, string _prevHash, List<Transaction> _transactions)
        {
            miningZeros = "00";
            blockNumber = _blockNumber;
            prevHash = _prevHash;
            time = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            transactions = _transactions;
            nonce = Mine(transactions, prevHash, blockNumber, time, miningZeros);
            hash = GetHash(transactions, prevHash, blockNumber, nonce, time);
            isValid = Validate(transactions, prevHash, blockNumber, nonce, time, miningZeros, hash);
        }

        string GetHash(List<Transaction> _transactions, string _prevHash, int blockNumber, int nonce, int time)
        {
            string encodingString = "";


            foreach(Transaction transaction in _transactions)
            {
                encodingString += transaction.sender;
                encodingString += transaction.reciver;
                encodingString += transaction.amt.ToString();
                encodingString += transaction.txnNumber.ToString();
                encodingString += transaction.time.ToString();
                encodingString += transaction.txnHash;
                transaction.txnBlockNum = blockNumber;
            }


            encodingString += _prevHash + blockNumber.ToString() + nonce.ToString() + time.ToString();

            return ToSHA512(encodingString);
        }

        int Mine(List<Transaction> _transactions, string _prevHash, int _blockNumber, int _time, string checkString)
        {
            int _nonce = 0;
            string _hash = GetHash(_transactions, _prevHash, _blockNumber, nonce, _time);
            while (_hash.Substring(0, checkString.Length) != checkString)
            {
                _hash = GetHash(_transactions, _prevHash, _blockNumber, _nonce, _time);
                _nonce++;
            }



            return _nonce-1;
        }

        bool Validate(List<Transaction> _transactions, string _prevHash, int _blockNumber, int _nonce,int _time, string checkString, string _hash)
        {

            if(_hash.Substring(0, checkString.Length) == checkString)
            {
                if(GetHash(_transactions, _prevHash, _blockNumber, _nonce, _time) == _hash)
                {
                    return true;
                }
                return false;
            }
            

            return false;
        }

    }

}
