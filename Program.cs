using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace dapper_demo
{
   public class Program
    {
        public static async Task Main(string[] args)
        {
            //* Get the DB created for the examples
            await GettingStarted();

            WriteOutUserInfo(SelectUsers(), "Sync SELECT");
            Console.WriteLine("--------------------");

            // WriteOutUserInfo(await SelectUsersAsync(), "Async SELECT");
            // Console.WriteLine("--------------------");

            // await AddUserAsync(new User {
            //     Username = "Tom_Tester",
            //     Password = "MyCoolPassword"
            // });
            // WriteOutUserInfo(await SelectUsersAsync(), "Insert (Async)");
            // Console.WriteLine("--------------------");

            // await AddManyUsersAsync(new User [] {
            //     new User { Username = "Susan_Berry", Password = "Password123" },
            //     new User { Username = "Kerry_Weiss", Password = "123Wow" },
            //     new User { Username = "Antara_Patel", Password = "$1mpl3P@$$w0rd" }
            // });
            // WriteOutUserInfo(await SelectUsersAsync(), "Many Insert (Async)");

            // Console.WriteLine("--------------------");
            // await UpdateUsersAsync(1);
            // WriteOutUserInfo(await SelectUsersAsync(), "Update (Async)");

            // Console.WriteLine("--------------------");
            // await UpdateUsersAsync(1);
            // WriteOutUserInfo(await SelectUsersAsync(), "Update (Async)");
        }

        //* Initialize an example DB
        public static async Task GettingStarted(){
            string dropTableSql = "drop table user;";
            string createTableSql = "create table user(userid integer primary key autoincrement, username varchar(300), password varchar(300) default \"password\", email varchar(300) null);";
            string populateTableSql = "insert into user(username) values (@uname);";
            string[] initialUsers = new string[] {
                "user1",
                "user2",
                "user3"
            };

            using(var conn = DapperConnectionFactory.CreateConnection()){
                //* Try to drop DB, and its ok to fail if not there
                await conn.ExecuteAsync(dropTableSql);

                //* Creating a table to use
                await conn.ExecuteAsync(createTableSql);

                //* Add basic users
                var users = initialUsers.Select(u => new { uname = u }).ToArray();
                await conn.ExecuteAsync(populateTableSql, users);
            }
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

        public static async Task AddUserAsync(User user){
            using(var conn = DapperConnectionFactory.CreateConnection()){
                var affectedRows = await conn.ExecuteAsync("insert into user (username, password) values (@username, @password)", new { username = user.Username, password = user.Password });
                Console.WriteLine($"Number of rows effected {affectedRows}");
            }
        }

        public static async Task AddManyUsersAsync(User[] users){
            using(var conn = DapperConnectionFactory.CreateConnection()){
                var affectedRows = await conn.ExecuteAsync("insert into user (username, password) values (@username, @password)", users.Select(u => new { username = u.Username, password = u.Password }));
                Console.WriteLine($"Number of rows effected {affectedRows}");
            }
        }

        public static async Task UpdateUsersAsync(int id) {
            using(var conn = DapperConnectionFactory.CreateConnection()){
                await conn.ExecuteAsync("UPDATE User SET username = @username where userid = @userid", new { username = "My Cool User Name", userid = id });
            }
        }

        public static async Task<bool> DeleteUserAsync(int id) {
            using(var conn = DapperConnectionFactory.CreateConnection()){
                var rowsAffected = await conn.ExecuteAsync("delete User where userid = @userid", new { userid = id });
                return rowsAffected > 0;
            }
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
