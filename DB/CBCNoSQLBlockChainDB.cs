using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using N_Blockchain;

namespace DB
{
    class CBCNoSQLBlockChainDB
    {
        string jsonFile;

        public CBCNoSQLBlockChainDB()
        {
            //Getting Json File
            jsonFile = @"../../../DB/blockchain.json";
        }

        //Function to put block in json file
        public void SerializeBlock(int blockNumber, string prevHash, string hash, int nonce, int time, List<Transaction> transactions, bool isValid)
        {
            // Reading Json
            string json = File.ReadAllText(jsonFile);

            //Deserializing Json
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //Getting Blocks
            JObject blocks = jsonObj["blocks"];

            //Block Function
            var unserializedBlock = new blocksEncriptor
            {
                blockNumber = blockNumber,
                prevHash = prevHash,
                hash = hash,
                nonce = nonce,
                time = time,
                isValid = isValid,
                transactions = transactions
            };

            //Serializing Block
            var serializedBlock = JsonConvert.SerializeObject(unserializedBlock);
            //unSerializing block and then adding it
            blocks.Add(new JProperty("block" + blockNumber.ToString(), JsonConvert.DeserializeObject(serializedBlock)));
            //Adding block to latest block for later access
            jsonObj.Property("latestBlock").Remove();
            jsonObj.Add(new JProperty("latestBlock", JsonConvert.DeserializeObject(serializedBlock)));

            //Saving 
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(jsonFile, output);

        }

        public void SerializeTransaction(string _sender, string _reciver, double _amt, int _txnNumber, string txnHash, int txnBlockNum, int _time)
        {
            // Reading Json
            string json = File.ReadAllText(jsonFile);

            //Deserializing Json
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //Getting Transaction
            JObject transactions = jsonObj["transactions"];

            //txn Function
            var unserializedTXN = new txnEncriptor
            {
                sender = _sender,
                reciver = _reciver,
                amt = _amt,
                txnNumber = _txnNumber,
                txnHash = txnHash,
                time = _time,
                txnBlockNum = txnBlockNum
            };

            //Serializing txn
            var serializedtxn = JsonConvert.SerializeObject(unserializedTXN);
            //unSerializing txn and then adding it
            transactions.Add(new JProperty("transactions" + _txnNumber.ToString(), JsonConvert.DeserializeObject(serializedtxn)));

            jsonObj.Property("latesttransaction").Remove();
            jsonObj.Add(new JProperty("latesttransaction", JsonConvert.DeserializeObject(serializedtxn)));

            //Saving 
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(jsonFile, output);
        }
         
        public blocksEncriptor DeserializeBlock()
        {
            string json = File.ReadAllText(jsonFile);

            //Deserializing Json
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //Getting Blocks
            JObject blocks = jsonObj["latestBlock"];


            blocksEncriptor decryptedBlock = JsonConvert.DeserializeObject<blocksEncriptor>(value: blocks.ToString(Formatting.None));


            return decryptedBlock;

        }

        public txnEncriptor DeserializeTXN()
        {
            string json = File.ReadAllText(jsonFile);

            //Deserializing Json
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //Getting Blocks
            JObject blocks = jsonObj["latesttransaction"];


            txnEncriptor decreptedtxn = JsonConvert.DeserializeObject<txnEncriptor>(blocks.ToString(Formatting.None));


            return decreptedtxn;


        }

        public bool GeneisisCheck()
        {
            // Reading Json
            string json = File.ReadAllText(jsonFile);

            //Deserializing Json
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //Getting Transaction
            JObject transactions = jsonObj["latestBlock"];

            chechGenesis decryptedGeneisis = JsonConvert.DeserializeObject<chechGenesis>(transactions.ToString(Formatting.None));

            if(decryptedGeneisis.blockNumber == 0)
            {
                return false;
            }

            return true;
        }

    }

    class BackLogedTXN
    {
        string jsonFile;

        public BackLogedTXN()
        {
            //Getting Json File
            jsonFile = @"../../../DB/backlog.json";
        }

        public void SerializeTransaction(string _sender, string _reciver, double _amt, int _txnNumber, string txnHash, int txnBlockNum, int _time)
        {
            // Reading Json
            string json = File.ReadAllText(jsonFile);

            //Deserializing Json
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //Getting Transaction
            JObject transactions = jsonObj["transactions"];

            //txn Function
            var unserializedTXN = new txnEncriptor
            {
                sender = _sender,
                reciver = _reciver,
                amt = _amt,
                txnNumber = _txnNumber,
                txnHash = txnHash,
                time = _time,
                txnBlockNum = txnBlockNum
            };

            //Serializing txn
            var serializedtxn = JsonConvert.SerializeObject(unserializedTXN);
            //unSerializing txn and then adding it
            transactions.Add(new JProperty("transactions" + _txnNumber.ToString(), JsonConvert.DeserializeObject(serializedtxn)));

            jsonObj.Property("latesttransaction").Remove();
            jsonObj.Add(new JProperty("latesttransaction", JsonConvert.DeserializeObject(serializedtxn)));

            //Saving 
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(jsonFile, output);
        }

        public txnEncriptor DeserializeTXN()
        {
            string json = File.ReadAllText(jsonFile);

            //Deserializing Json
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //Getting Blocks
            JObject blocks = jsonObj["latesttransaction"];


            txnEncriptor decreptedtxn = JsonConvert.DeserializeObject<txnEncriptor>(blocks.ToString(Formatting.None));


            return decreptedtxn;


        }

        txnEncriptor DeserializeTXN(JToken txn)
        {
            txnEncriptor decreptedtxn = JsonConvert.DeserializeObject<txnEncriptor>(txn.ToString(Formatting.None));
            return decreptedtxn;
        }

        JObject GetTXNS()
        {
            string json = File.ReadAllText(jsonFile);

            //Deserializing Json
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //Getting Blocks
            JObject TXNS = jsonObj["transactions"];
            return TXNS;
        }

        public List<txnEncriptor> GetBackLog()
        {
            JObject test = GetTXNS();
            List<txnEncriptor> backLog = new List<txnEncriptor>();

            foreach (var t in test)
            {
                JToken token = t.Value;
                if (DeserializeTXN(token).txnNumber>0) backLog.Add(DeserializeTXN(token));
            }

            return backLog;

        }

        public void clear()
        {
            string json = File.ReadAllText(jsonFile);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            var unserializedTXN = new txnEncriptor
            {
                sender = "",
                reciver = "",
                amt = 0,
                txnNumber = 0,
                txnHash = "",
                time = 0,
                txnBlockNum = 0
            };

            var zerotxn = new zeroTXN
            {
                transactions0 = unserializedTXN
            };

            var serializedtxn = JsonConvert.SerializeObject(zerotxn);


            jsonObj.Property("transactions").Remove();
            jsonObj.Property("latesttransaction").Remove();
            jsonObj.Add(new JProperty("transactions", JsonConvert.DeserializeObject(serializedtxn)));
            jsonObj.Add(new JProperty("latesttransaction", JsonConvert.DeserializeObject(serializedtxn)));

            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(jsonFile, output);
        }

    }
}

//Blocks Function
public class blocksEncriptor
{
    public int blockNumber { get; set; }
    public string prevHash { get; set; }
    public string hash { get; set; }
    public int nonce { get; set; }
    public int time { get; set; }
    public bool isValid { get; set; }
    public List<Transaction> transactions { get; set; }

}


public class txnEncriptor
{
    public string sender { get; set; }
    public string reciver { get; set; }
    public double amt { get; set; }
    public int txnNumber { get; set; }
    public string txnHash { get; set; }
    public int time { get; set; }
    public int txnBlockNum { get; set; }
}

public class chechGenesis
{
    public int blockNumber { get; set; }

}

public class zeroTXN
{
    public txnEncriptor transactions0 { get; set; }
}