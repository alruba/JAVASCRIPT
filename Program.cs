using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Chicago_Crime_Data
{
    class Program 
    {
        static void Main(string[] args)
        {
            FileStream fileStreamreader = new FileStream(@"C:\Users\Training\Downloads\crimedata.csv", FileMode.OpenOrCreate);
            FileStream fileStreamwriter = new FileStream(@"C:\Users\Training\Desktop\alruba.json", FileMode.Append);
            FileStream fileStreamwriter2 = new FileStream(@"C:\Users\Training\Desktop\alruba2.json", FileMode.Append);
            FileStream fileStreamwriter3 = new FileStream(@"C:\Users\Training\Desktop\alruba3.json", FileMode.Append);
            StreamReader sr = new StreamReader(fileStreamreader, Encoding.UTF8);
            StreamWriter sw = new StreamWriter(fileStreamwriter);
            StreamWriter sw2 = new StreamWriter(fileStreamwriter2);
            StreamWriter sw3 = new StreamWriter(fileStreamwriter3);
            String[] Header = { "YEAR", "No. of Robbery", "No. of Burglary", "Criminal Damage done to Property", "Criminal Damage done to Vehicle", "Criminal Damage done to State Sup Property" };
            String[] data = sr.ReadLine().ToString().Split(','); 
            int[] NRobPY = new int[17], NBurgPY = new int[17], CDProp = new int[17], CDVeh = new int[17], CDStProp = new int[17];
            int x = 0, k = 5; ;
            Dictionary<String, int> robberyDictionary = new Dictionary<String, int>();           
            while (!sr.EndOfStream)
            {
                String[] data2 = sr.ReadLine().ToString().Split(',');
                Int32 a;
                if (Int32.TryParse(data2[17], out a))
                    if (a >= 2001 && a <= 2016)
                    {
                        x = a % 2000;
                        //FIRST_FILTER
                        int n = (data[5] == "Primary Type" && data2[5] == "ROBBERY") ? NRobPY[x] += 1 : NBurgPY[x] += 1;
                        //SECOND_FILTER
                        int m = (data[5] == "Primary Type" && data2[5] == "CRIMINAL DAMAGE" && data[6] == "Description" && data2[6] == "TO PROPERTY") ? CDProp[x] = CDProp[x] + 1 : ((data[5] == "Primary Type" && data2[5] == "CRIMINAL DAMAGE" && data[6] == "Description" && data2[6] == "TO VEHICLE") ? CDVeh[x] = CDVeh[x] + 1 : ((data[5] == "Primary Type" && data2[5] == "CRIMINAL DAMAGE" && data[6] == "Description" && data2[6] == "TO STATE SUP PROP") ? CDStProp[x] = CDStProp[x] + 1 : 0));
                    }
                //THIRD_FILTER            
                for (int i = 5; i <= 6; i++)
                {
                    if (data[k] == "Description" && data2[i - 1] == "ROBBERY")
                    {
                        if (!robberyDictionary.ContainsKey(data2[i]))
                        {
                            robberyDictionary.Add(data2[i], 1);
                        }
                        else
                        {
                            ++robberyDictionary[data2[i]];
                        }
                    }
                    k++;
                    k = (k == 7) ? 5 : k;
                }
            }
            sr.Dispose();
            sw.Flush();
            sw.Write("[");                   //writing first json file
            for (int p = 1; p < 17; p++)
            {
                sw.Write("\n"+"{" + "\"" + Header[0] + "\"" + ":" + (2000 + p) + "," + "\n" + Header[1] + "\"" + ":" + NRobPY[p] + "," + "\n"+"\"" +Header[2]+"\""+":"+NBurgPY[p]+"}" );
                if (p < 16)
                    sw.Write(",");
            }
            sw.Write("]");
            sw.Flush();
                           //writing second json file
            sw2.Write("[");
            for (int q = 1; q < 17; q++)
            {
                sw2.Write("\n" + "{" + "\"" + Header[0] + "\"" + ":" + (2000 + q) + "," + "\n" + Header[3] + "\"" + ":" + CDProp[q] + "," + "\n" + "\"" + Header[4] + "\"" + ":" + CDVeh[q] + "," + "\n" + "\"" + Header[5] + "\"" + ":" + CDStProp[q] + "}");
                if (q < 16)
                    sw2.Write(",");
            }
            sw2.Write("]");
            sw2.Flush();                     
           string json3 = JsonConvert.SerializeObject(robberyDictionary, Formatting.Indented);
            sw3.Write(json3);                      //writing third json file
            sw3.Flush();
        }
    }
}
