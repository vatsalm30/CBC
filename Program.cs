using System;
using Newtonsoft.Json;
using DB;
using N_Blockchain;
using System.Numerics;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace CBC
{
    class CBC
    {
        static void Main(string[] args)
        {

            Blockchain bc = new Blockchain();
            bc.CreateTransaction("8r55SaeZrBzAMbWEg4KEBYp5iU8K9Pcpp9BXw9mvz6XKLJiUPgAsxR2", "receiver", 13, "39a17d84a064db8a71b326383d1bec85802eaa7c1cb652a29233059ff9cde544ad3d95fafe875ae8127c3157e2438ef36e86be96e45ccb482eaf98717d41e906");
        }

    }
}
