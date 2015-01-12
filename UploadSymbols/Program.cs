using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UploadSymbols
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            string line;

            Console.Write("Cloud (Local)? : ");
            string DBType = Console.ReadLine();
            Console.Write("Exchange? : ");
            string DBExchange = Console.ReadLine();

            System.IO.StreamReader file = new System.IO.StreamReader("C:\\eoddata\\" + DBExchange + "\\" + DBExchange + ".txt");
            while ((line = file.ReadLine()) != null)
            {
                while (true)
                {
                    try
                    {
                        if (DBType.ToUpper() == "LOCAL")
                        {
                            LocalDBDataContext dbLocal = new LocalDBDataContext();
                            String[] localColumns = line.Split('\t');
                            String localSymbol = localColumns[0];
                            String localDescription = localColumns[1];
                            LocalMstSymbol localNewSymbol = new LocalMstSymbol();

                            localNewSymbol.Exchange = DBExchange.ToUpper();
                            localNewSymbol.Symbol = localSymbol;
                            localNewSymbol.Description = localDescription;

                            dbLocal.LocalMstSymbols.InsertOnSubmit(localNewSymbol);
                            dbLocal.SubmitChanges();

                            counter++;

                            Console.WriteLine(counter + "." + localSymbol);

                            break;
                        }
                        else
                        {
                            CloudDBDataContext dbCloud = new CloudDBDataContext();
                            String[] cloudColumns = line.Split('\t');
                            String cloudSymbol = cloudColumns[0];
                            String cloudDescription = cloudColumns[1];
                            CloudMstSymbol cloudNewSymbol = new CloudMstSymbol();

                            cloudNewSymbol.Exchange = DBExchange.ToUpper();
                            cloudNewSymbol.Symbol = cloudSymbol;
                            cloudNewSymbol.Description = cloudDescription;

                            dbCloud.CloudMstSymbols.InsertOnSubmit(cloudNewSymbol);
                            dbCloud.SubmitChanges();

                            counter++;
                            Console.WriteLine(counter + "." + cloudSymbol);

                            break;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Link failed.  Retrying...");
                        Thread.Sleep(1000);
                    }
                }
            }
            file.Close();
            Console.ReadLine();
        }
    }
}
