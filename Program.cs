using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Security.Principal;

namespace ConsoleApp2
{
    class Program
    {

        public static List<DomainController> GetDomainControllers()
        {
            List<DomainController> domainControllers = new List<DomainController>();
            try
            {
                Domain domain = Domain.GetCurrentDomain();
                Console.WriteLine("CurrentDomain\t" + domain);
                foreach (DomainController dc in domain.DomainControllers)
                {
                    domainControllers.Add(dc);
                }
            }
            catch { }
            return domainControllers;
        }

        public static void GetComputerAddresses(List<string> computers)
        {
            foreach (string computer in computers)
            {
                try
                {
                    IPAddress[] ips = System.Net.Dns.GetHostAddresses(computer);
                    foreach (IPAddress ip in ips)
                    {
                        if (!ip.ToString().Contains(":"))
                        {
                            Console.WriteLine("{0}: {1}", computer, ip);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("[X] ERROR: {0}", ex.Message);
                }
            }
        }

        public static List<string> GetComputers()
        {
            List<string> computerNames = new List<string>();
            List<DomainController> dcs = GetDomainControllers();
            if (dcs.Count > 0)
            {
                try
                {
                    Domain domain = Domain.GetCurrentDomain();
                    //domain.
                    string currentUser = WindowsIdentity.GetCurrent().Name.Split('\\')[1];


                    using (DirectoryEntry entry = new DirectoryEntry(String.Format("LDAP://{0}", dcs[0])))
                    {
                        using (DirectorySearcher mySearcher = new DirectorySearcher(entry))
                        {
                            mySearcher.Filter = ("(objectClass=computer)");

                            // No size limit, reads all objects
                            mySearcher.SizeLimit = 0;

                            // Read data in pages of 250 objects. Make sure this value is below the limit configured in your AD domain (if there is a limit)
                            mySearcher.PageSize = 250;

                            // Let searcher know which properties are going to be used, and only load those
                            mySearcher.PropertiesToLoad.Add("name");

                            foreach (SearchResult resEnt in mySearcher.FindAll())
                            {
                                // Note: Properties can contain multiple values.
                                if (resEnt.Properties["name"].Count > 0)
                                {
                                    string computerName = (string)resEnt.Properties["name"][0];
                                    computerNames.Add(computerName);
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            else
            {
                Console.WriteLine("ERROR: Could not get a list of Domain Controllers.");
            }
            return computerNames;
        }


        static void Main(string[] args)
        {

            var computers = GetComputers();
            Console.WriteLine("==================================");
            Console.WriteLine("Name                  IP");
            foreach (string computer in computers)
            {
                IPAddress[] ips = System.Net.Dns.GetHostAddresses(computer);
                foreach (IPAddress ip in ips)
                {
                    if (!ip.ToString().Contains(":"))
                    {
                        Console.WriteLine("{0}\t\t{1}", computer, ip);
                    }
                }
            }
        }
    }
}
