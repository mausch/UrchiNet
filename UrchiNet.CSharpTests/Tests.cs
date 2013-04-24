using System;
using System.Configuration;
using System.Linq;
using Fuchu;
using FSharpx;
using FSharpx.Collections;
using Microsoft.FSharp.Core;

namespace UrchiNet.CSharpTests {
    public class Program {
        static string GetOrThrow(string key) {
            var r = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(r))
                throw new Exception(string.Format("Configure {0} in your app.config", key));
            return r;
        }

        static Test Tests() {
            var config = new Config(host: GetOrThrow("urchin.hostport"), 
                login: GetOrThrow("urchin.login"), 
                password: GetOrThrow("urchin.password"));

            var service = new Service(config);

            return Test.List("C# tests", new[] {
                Test.Case("Get accounts", () => {
                    foreach (var account in service.GetAccountListOrThrow())
                        Console.WriteLine(account.Name);
                }), 

                Test.Case("Get profiles", () => {
                    var profiles = service.GetProfileListOrThrow(1).ToList();
                    foreach (var profile in profiles)
                        Console.WriteLine(profile.Name);
                }),

                Test.Case("Get tables", () => {
                    var tables = service.GetTableListOrThrow(1).ToList();
                    foreach (var table in tables)
                        foreach (var dimension in table.Dimensions)
                            Console.WriteLine(dimension.ToString());
                }),

                Test.Case("Query data", () => {
                    var query = Query.Create(profileId: 1, 
                        startDate: DateTime.Now, 
                        endDate: DateTime.Now, 
                        dimensions: NonEmptyList.Singleton(Dimension.Browser_base),
                        table: Table.BrowserPlatformConnectionSpeed1.Some());
                    var results = service.GetDataOrThrow(query).ToList();
                    foreach (var record in results) {
                        foreach (var dimension in record.Dimensions)
                            Console.WriteLine(dimension.Dimension.ToString());
                    }
                })
            });
        }

        private static int Main(string[] args) {
            return Tests().Run();
        }
    }
}