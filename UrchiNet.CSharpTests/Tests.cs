using System;
using System.Configuration;
using Fuchu;

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
            });
        }

        private static int Main(string[] args) {
            return Tests().Run();
        }
    }
}