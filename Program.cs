using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace dapper_demo
{
   public class Program
    {
        public static async Task Main(string[] args)
        {
             Console.WriteLine("SELECT USERS");
             WriteOutUserInfo(SelectUsers(), "Synchronous SELECT");
             Console.WriteLine("--------------------");
             WriteOutUserInfo(await SelectUsersAsync(), "Asynchronous SELECT");
        }


        public static IEnumerable<User> SelectUsers(){
            IEnumerable<User> users = null;
            using(var conn = DapperConnectionFactory.CreateConnection()){
                users = conn.Query<User>("SELECT * from User");
            }
            return users;
        }

        public static async Task<IEnumerable<User>> SelectUsersAsync(){
            IEnumerable<User> users = null;
            using(var conn = DapperConnectionFactory.CreateConnection()){
                users = await conn.QueryAsync<User>("SELECT * from User");
            }
            return users;
        }

        public static void WriteOutUserInfo(IEnumerable<User> users, string initialGroupMessage = "USERS"){
            Console.WriteLine($"***** {initialGroupMessage} *****");
            foreach(var user in users){
                Console.WriteLine($"{user.UserId}  |  {user.Username}  | {user.Password}");
            }
            Console.WriteLine("***************");
        }
    }
}
