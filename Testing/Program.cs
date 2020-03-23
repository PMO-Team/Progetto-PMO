using System;
using System.IO;
using BTC;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "../../../../generated.btc";


            /*BTCObject mObj = new BTCObject();
            mObj.Add("player", new BTCString("simo"));
            mObj.Add("pg-name", new BTCString("Kirox"));
            mObj.Add("pg-level", new BTCNumber(3));
            BTCObject wp = new BTCObject();
            wp.Add("weapon-type", new BTCString("bow"));
            wp.Add("weapon-range", new BTCNumber(64));
            wp.Add("weapon-destroyed", new BTCBool(false));
            mObj.Add("weapon", wp);
            BTCList items = new BTCList();
            IBTCData item = new BTCObject();
            ((BTCObject) item).Add("name", new BTCString("Kirinite"));
            ((BTCObject) item).Add("description", new BTCString("A fantasy mineral"));
            items.Add((BTCObject) item);
            items.Add(new BTCString("Health Potion (12)"));
            mObj.Add("inventory", items);
            items = new BTCList();
            items.Add(new BTCNumber(1));
            items.Add(new BTCNumber(2));
            items.Add(new BTCNumber(3));
            items.Add(new BTCNumber(4));
            //items.Add()
            ((BTCList) mObj.Tag("inventory")).Add(items);

            BTCParser.EncodeIntoFile(mObj, path, true);*/
            
            
            try
            {
                BTCObject myObject = BTCParser.DecodeFromFile(path);
                
                myObject.Add("new-message", new BTCString("It's a new message"));

                Console.WriteLine(((BTCString) myObject.Tag("example-text")).Encode());

                //Console.WriteLine(BTCParser.Encode(myObject, true));
                //BTCParser.EncodeIntoFile(myObject, path, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
