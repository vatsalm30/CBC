using System;
using DB;
using static Utils;
using System.Numerics;
using System.Globalization;

namespace N_Blockchain
{
    class Blockchain
    {
        public List<Block> blockchain;
        int txnBackLogMax;
        CBCNoSQLBlockChainDB chain;
        BackLogedTXN backLog;
        public Blockchain()
        {
            blockchain = new List<Block>();
            txnBackLogMax = 5;
            chain = new CBCNoSQLBlockChainDB();
            backLog = new BackLogedTXN();
            CreateGeneseis();
        }

        void CreateBlock(int _blockNumber, string _prevHash, List<Transaction> _transactions)
        {
            Block block = new Block(_blockNumber, _prevHash, _transactions);

            blockchain.Add(block);
            chain.SerializeBlock(_blockNumber, _prevHash, block.hash, block.nonce, block.time, _transactions, block.isValid);
        }


        void CreateGeneseis()
        {
            if (!chain.GeneisisCheck())CreateBlock(1, "", new List<Transaction>());
        }

        public void CreateTransaction(string _sender, string _reciver, double _amt, string _key)
        {
            if(Sign(_sender, _key))
            {
                if (backLog.DeserializeTXN().txnNumber <= 0)
                {

                    Transaction txn = new Transaction(_sender, _reciver, _amt, chain.DeserializeTXN().txnNumber + 1, chain.DeserializeBlock().blockNumber + 1);

                    backLog.SerializeTransaction(txn.sender, txn.reciver, txn.amt, txn.txnNumber, txn.txnHash,txn.txnBlockNum, txn.time);
                }

                else
                {
                    Transaction txn = new Transaction(_sender, _reciver, _amt, backLog.DeserializeTXN().txnNumber + 1, backLog.DeserializeTXN().txnBlockNum);
                    backLog.SerializeTransaction(txn.sender, txn.reciver, txn.amt, txn.txnNumber, txn.txnHash, txn.txnBlockNum, txn.time);
                }

                if (backLog.GetBackLog().Count() >= txnBackLogMax)
                {
                    List<Transaction> transactions = EncryptedTXNToNormalTXN(backLog.GetBackLog());
                    CreateBlock(chain.DeserializeBlock().blockNumber + 1, chain.DeserializeBlock().hash, transactions);
                    foreach (Transaction t in transactions)
                    {
                        chain.SerializeTransaction(t.sender, t.reciver, t.amt, t.txnNumber, t.txnHash, t.txnBlockNum, t.time);
                    }
                    backLog.clear();
                }
            }
            
        }

        bool Sign(string sender, string signature)
        {
            if (signature.Length == 128 || (54 <= sender.Length && sender.Length <= 57))
            {
                BigInteger number = BigInteger.Parse(signature, NumberStyles.AllowHexSpecifier);
                byte[] privKey = number.ToByteArray();

                string pubKey = CreatepubKey(privKey);
                string address = GeneratAddress(pubKey);
                if (address == sender) return true;
                else return false;
            }

            return false;
        }

    }
}
