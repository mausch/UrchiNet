﻿using System;
using System.Configuration;
using System.Linq;
using Fuchu;
using Microsoft.FSharp.Collections;
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
                    foreach (var account in service.GetAccountList())
                        Console.WriteLine(account.Name);
                }), 

                Test.Case("Query data", () => {
                    var query = DataParameters.Create(profileId: 1, 
                        startDate: DateTime.Now, 
                        endDate: DateTime.Now, 
                        dimensions: NonEmptyList.Singleton(Dimension.Browser_base),
                        table: FSharpOption<Table>.Some(Table.BrowserPlatformConnectionSpeed1));
                    var results = service.GetData(query).ToList();
                    foreach (var record in results) {
                        foreach (var dimension in record.Dimensions)
                            Console.WriteLine(dimension.Item1.ToString());
                    }
                })
            });
        }

        private static int Main(string[] args) {
            return Tests().Run();
        }
    }
}